using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class EnemyObject
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
    public NameMaster EnemyName;
    [SerializeField] public int hp;
    [SerializeField] public int score;
    public enum SkillMaster
    {
        FiveShotBallLv1,
        FiveShotBallLv2,
        FiveShotBallLv3,
        FiveShotBallLv4,
        FiveShotBallLv5,
        FiveShotBallLv6,
        RainBallLv1,
        RainBallLv2,
        RainBallLv3,
        RainBallLv4,
        RainBallLv5,
        RainBallLv6,
        HomingBallLv1,
        HomingBallLv2,
        HomingBallLv3,
        HomingBallLv4,
        HomingBallLv5,
        HomingBallLv6,
    }
    public SkillMaster SkillName;
    [SerializeField] public GameObject Enemyobject;
}

public class SC_EnemyManager : MonoBehaviour
{

    [SerializeField] private List<EnemyObject> GB_EnemyObject = null;   //敵のステータス
    [SerializeField] private GameObject OB_EnemyClones = null;          //敵のクローン
    [SerializeField] private GameObject OB_EnemyBreakClones = null;     //敵の破壊アニメーションのクローン
    [SerializeField] private GameObject OB_EnemyBreak = null;
    [SerializeField] private SC_ScoreManager SC_ScoreManager = null;    //スコア

    /// <summary> 
    /// 敵生成
    /// </summary> 
    /// <param name="pm_Position">敵の位置</param> 
    /// <param name="pm_EnemyName">敵の名前</param> 
    /// <param name="pm_PowerCount">敵のダメージ攻撃量</param> 
    /// <param name="pm_Number">敵番号</param> 
    /// <returns>敵オブジェクト</returns>
    public SC_Enemy Create(Vector2 pm_Position,string pm_EnemyName,int pm_PowerCount, int pm_Number)
    {

        GameObject w_Enemy = null;
        SC_Enemy w_EnemyStatus = null;
        int w_Hp = 0;
        int w_Score = 0;
        string w_Skill = "";

        //敵オブジェクト選択
        for (int i = 0; i < GB_EnemyObject.Count(); i++)
        {
            if (pm_EnemyName == GB_EnemyObject[i].EnemyName.ToString())
            {
                w_Enemy = Instantiate(this.GB_EnemyObject[i].Enemyobject, OB_EnemyClones.GetComponent<RectTransform>()) as GameObject;
                w_Hp = GB_EnemyObject[i].hp;
                w_Score = GB_EnemyObject[i].score;
                w_Skill = GB_EnemyObject[i].SkillName.ToString();
                break;
            }
        }

        //敵オブジェクトセット
        w_Enemy.GetComponent<RectTransform>().anchoredPosition = pm_Position;
        w_Enemy.name = pm_Number.ToString("Enemy00");
        w_Enemy.transform.SetParent(OB_EnemyClones.transform);
        w_EnemyStatus = w_Enemy.GetComponent<SC_Enemy>();
        w_EnemyStatus.OptionSet(pm_Position, pm_PowerCount, w_Hp, w_Score,w_Skill);

        //敵がやられたときにスコアを取得
        w_EnemyStatus.GB_OnBroken.Subscribe(
            score => EnemyBreak(w_EnemyStatus,score)
        ).AddTo(w_EnemyStatus);

        return w_EnemyStatus;

    }

    /// <summary> 
    /// 敵の破壊アニメーション生成
    /// </summary> 
    /// <param name="pm_Enemy">敵の位置</param> 
    /// <param name="pm_score">スコア</param> 
    /// <returns>なし</returns>
    private void EnemyBreak(SC_Enemy pm_Enemy,int pm_score)
    {
        GameObject w_EnemyBreak;

        //敵の位置を取得し、敵がやられた時のアニメーションの位置に設定
        Vector2 w_EnemyBreakPosition = pm_Enemy.GetComponent<RectTransform>().anchoredPosition;
        w_EnemyBreak = Instantiate(OB_EnemyBreak, OB_EnemyBreakClones.GetComponent<RectTransform>()) as GameObject;
        w_EnemyBreak.GetComponent<RectTransform>().anchoredPosition = w_EnemyBreakPosition;

        //アニメーション終了後にオブジェクト削除
        StartCoroutine(DelayMethod(0.8f, () =>
        {
            Destroy(w_EnemyBreak);
        }));

        //スコア反映
        SC_ScoreManager.UpdateScore(pm_score);
    }
        

    /// <summary> 
    /// 敵攻撃
    /// </summary> 
    /// <param name="pm_message">なし</param> 
    /// <returns>なし</returns>
    public void EnemyTurn()
    {
        //敵の数分攻撃
        foreach (Transform childTransform in OB_EnemyClones.transform)
        {
            StartCoroutine(DelayMethod(UnityEngine.Random.value, () =>
            {
                GameObject OB_EnemyAttack = GameObject.Find(childTransform.gameObject.name);
                SC_Enemy w_EnemyAttack = OB_EnemyAttack.GetComponent<SC_Enemy>();
                w_EnemyAttack.EnemyAttack();
            }));
        }
    }

    /// <summary> 
    /// 処理待ち時間
    /// </summary> 
    /// <param name="pm_WaitTime">待ち時間</param> 
    /// <param name="pm_Action">処理</param> 
    /// <returns>なし</returns>
    private IEnumerator DelayMethod(float pm_WaitTime, Action pm_Action)
    {
        yield return new WaitForSeconds(pm_WaitTime);
        pm_Action();
    }
}