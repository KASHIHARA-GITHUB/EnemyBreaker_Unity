using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[System.Serializable]
public class EnemyCount
{
    public enum NameMaster
    {
        OB_Enemy1,
        OB_Boss1,
        OB_Enemy2_1,
        OB_Enemy2_2,
        OB_Boss2,
        OB_Enemy3_1,
        OB_Enemy3_2,
        OB_Enemy3_3,
        OB_Boss3,
        OB_Enemy4_1,
        OB_Enemy4_2,
        OB_Enemy4_3,
        OB_Boss4,
        OB_Enemy5_1,
        OB_Enemy5_2,
        OB_Enemy5_3,
        OB_Enemy5_4,
        OB_Boss5_1,
        OB_Boss5_2,
        OB_Enemy6_1,
        OB_Enemy6_2,
        OB_Enemy6_3,
        OB_Enemy6_4,
        OB_Enemy6_5,
        OB_Enemy6_6,
        OB_Boss6,
        OB_Enemy7_1,
        OB_Enemy7_2,
        OB_Enemy7_3,
        OB_Enemy7_4,
        OB_Enemy7_5,
        OB_Enemy7_6,
        OB_Enemy7_7,
        OB_Enemy7_8,
        OB_Boss7,
        OB_Enemy8_1,
        OB_Enemy8_2,
        OB_Enemy8_3,
        OB_Enemy8_4,
        OB_Enemy8_5,
        OB_Enemy8_6,
        OB_Boss8
    }
    public NameMaster name;
    public Vector2 position;
}

[System.Serializable]
public class AreaCount
{
    public List<EnemyCount> GB_EnemyCount;
}

[System.Serializable]
public class StageCount
{
    public List<AreaCount> GB_AreaCount;
}

public class SC_StageManager : MonoBehaviour
{
    //ステージに敵を配置設定
    public  List<StageCount>  GB_StageCount;

    //オブジェクト変数
    [SerializeField] private GameObject OB_WarningPanel = null;             //ワーニング画面

    //スクリプト変数
    [SerializeField] private GameManager SC_GameManager = null;             //ゲームマネージャー
    [SerializeField] private SC_EnemyManager SC_EnemyManager = null;        //敵管理
    [SerializeField] private SC_GameSound SC_GameSound = null;              //ゲームサウンド
    [SerializeField] private SC_GameSE SC_GameSE = null;                    //ゲーム効果音
    [SerializeField] private SC_GimmickManager SC_GimmickManager = null;    //ギミック

    /// <summary> 
    /// 敵を召喚するエリア
    /// </summary> 
    /// <param name="pm_StageCount">ステージ番号</param> 
    /// <param name="pm_AreaCount">エリア番号</param>
    /// <param name="pm_PowerCount">プレイヤーの攻撃力</param>
    /// <returns>なし</returns>
    //public async void Area(int pm_StageCount, int pm_AreaCount, int pm_PowerCount)
    public void Area(int pm_StageCount, int pm_AreaCount, int pm_PowerCount)
    {
        StartCoroutine(AreaCoroutine(pm_StageCount, pm_AreaCount, pm_PowerCount));
    }

    private IEnumerator AreaCoroutine(int pm_StageCount, int pm_AreaCount, int pm_PowerCount)
    {
        //初期化
        List<SC_Enemy> w_Blocks = new List<SC_Enemy>();

        //エリア初期設定
        if ((GB_StageCount[pm_StageCount].GB_AreaCount.Count() - 1) == pm_AreaCount)
        {
            //await Warning(pm_StageCount);
            Warning(pm_StageCount);
            yield return new WaitForSeconds(10f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            NextAreaInit();
        }

        //ギミック初期化
        SC_GimmickManager.Option(pm_StageCount, pm_AreaCount);

        //敵配置
        for (int i = 0; i < GB_StageCount[pm_StageCount].GB_AreaCount[pm_AreaCount].GB_EnemyCount.Count(); i++)
        {


            SC_Enemy w_Block = SC_EnemyManager.Create(GB_StageCount[pm_StageCount].GB_AreaCount[pm_AreaCount].GB_EnemyCount[i].position,
                                                      GB_StageCount[pm_StageCount].GB_AreaCount[pm_AreaCount].GB_EnemyCount[i].name.ToString(),
                                                      pm_PowerCount,
                                                      i);

            w_Blocks.Add(w_Block);
        }

        //敵を全部壊した場合の処理
        var w_Stream = w_Blocks.Select(w_Block => w_Block.GB_OnBroken);

        //敵退治後のエリア遷移
        if ((GB_StageCount[pm_StageCount].GB_AreaCount.Count() - 1) == pm_AreaCount)
        {
            //ボスエリアの全敵を倒したクリア画面を表示
            Observable.WhenAll(w_Stream).Subscribe(_ => SC_GameManager.ClearStage());
        }
        else
        {
            //敵エリアの全敵を倒したら次の敵エリアを表示
            Observable.WhenAll(w_Stream).Subscribe(_ => Area(pm_StageCount, pm_AreaCount + 1, pm_PowerCount));
        }
    }

    /// <summary> 
    /// ワーニング画面表示
    /// </summary> 
    /// <param name="pm_StageCount">ステージ番号</param> 
    /// <returns>なし</returns>
    //public async Task Warning(int pm_StageCount)
    public void Warning(int pm_StageCount)
    {
        StartCoroutine("WarningCoroutine",pm_StageCount);
    }

    private IEnumerator WarningCoroutine(int pm_StageCount)
    {
        //await SC_GameSound.SoundFadeOutSyn();
        SC_GameSound.SoundFadeOutSyn();
        yield return new WaitForSeconds(5f);

        //画面に表示しているボールを削除
        SC_GameManager.BallDelete();

        //警告のSEを流す
        SC_GameSE.WarningSound();

        //ワーニングパネルを表示
        OB_WarningPanel.SetActive(false);
        OB_WarningPanel.SetActive(true);

        //Warningアニメーションのため5秒待ち
        //await Task.Delay(5000);
        yield return new WaitForSeconds(5f);

        //ステージのBGMを流す
        SC_GameSound.StageSound(pm_StageCount, true);

        //自分のターン画面表示
        SC_GameManager.AreaAfterMyTurnDisplay();
    }

    /// <summary> 
    /// 次のエリア表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void NextAreaInit()
    {
        //画面に表示しているボールを削除
        SC_GameManager.BallDelete();

        //自分のターン画面表示
        SC_GameManager.AreaAfterMyTurnDisplay();
    }
}
