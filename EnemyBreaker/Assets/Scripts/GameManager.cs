using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx.Triggers;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    //オブジェクト変数
    [SerializeField] private GameObject OB_PausePanel = null;          //ゲームポーズパネル
    [SerializeField] private GameObject OB_TitlePanel = null;          //ゲームタイトルパネル
    [SerializeField] private GameObject OB_WarningPanel = null;        //ゲームワーニングパネル
    [SerializeField] private GameObject OB_ClearPanel = null;          //クリアパネル
    [SerializeField] private GameObject OB_GameOverPanel = null;       //ゲームオーバーパネル
    [SerializeField] private GameObject OB_MyTurnPanel = null;         //自ターンパネル
    [SerializeField] private GameObject OB_EnemyTurnPanel = null;      //敵ターンパネル
    [SerializeField] private GameObject OB_BlocksClone = null;         //敵クローン
    [SerializeField] private GameObject OB_BlocksAttackClone = null;   //敵攻撃クローン
    [SerializeField] private GameObject OB_BallsClone = null;          //ボールクローン
    [SerializeField] private GameObject OB_Bar = null;                 //ボールバー
    [SerializeField] private GameObject OB_BarRad = null;              //ボールバー角度
    [SerializeField] private GameObject OB_GimmicksClone = null;       //ギミッククローン

    //コンポーネント変数
    [SerializeField] private Text OB_StageNumText = null;   //ステージ番号テキスト
    [SerializeField] private Text OB_StageTitText = null;   //ステージタイトルテキスト

    //スクリプト変数
    [SerializeField] private SC_GameSound    SC_GameSound = null;      //ゲームサウンド
    [SerializeField] private SC_BallManager  SC_BallManager  = null;   //ボール管理
    [SerializeField] private SC_ScoreManager SC_ScoreManager = null;   //スコア管理
    [SerializeField] private SC_BarManager   SC_BarManager   = null;   //バー管理
    [SerializeField] private SC_ClearManager SC_ClearManager = null;   //クリア画面管理
    [SerializeField] private SC_StageManager SC_StageManager = null;   //ステージ管理
    [SerializeField] private SC_EnemyManager SC_EnemyManager = null;   //敵管理

    //グローバル変数
    private int GB_Score = 0;              //スコア
    private int GB_ScoreHold;              //クリア画面で現在のスコアより減少しないようにする
    private int GB_BallDecrement = 0;      //球の発射数
    private int GB_BallCountMAX = 15;      //球の発射最大数
    private int GB_BallHoldCountMAX;       //クリア画面で現在の球の発射最大数より減少しないようにする
    private int GB_ExpCount  = 0;          //経験値の数
    private int GB_PowerCount = 1;         //パワーの数
    private int GB_PowerHoldCount;         //クリア画面で現在のパワーの数より減少しないようにする
    private int GB_StageCount = 0;         //ステージ番号
    private int GB_PlayerHP = 30;          //プレイヤー体力
    private int GB_BallCounter = 10;       //ボール体力
    private int GB_BallCounterHold;        //クリア画面で現在のボールカウンターの数より減少しないようにする

    private const int GB_ScoreCost = 1000;       //スコアのコスト
    private const int GB_BallMaxCost = 1000;    //ボール発射最大数のコスト
    private const int GB_BallCounterCost = 1000;//ボールカウンターのコスト
    private const int GB_PowerCost = 3000;      //パワーのコスト

    private const int GB_HpGet = 10;
    private const int GB_ExeGet = 500;

    private bool GB_BallShotFlag = false;   //ボール発射フラグ(true:発射無効 false:発射可能)
    private bool GB_PauseFlag = false;      //ポーズフラグ(true:ポーズ無 false:ポーズ有)
    private bool GB_TurnFlag = true;        //ターンフラグ(true:自ターン false:敵ターン)
    private bool GB_CoroutineFlag = true;   //コルーチン無効フラグ
    private bool GB_GameOverFlag = false;

    private List<GameObject> GB_Balls = new List<GameObject>(); //ボール生成変数

    /// <summary> 
    /// ゲームスタート
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void Start()
    {
        //初期化
        Initialize();

        //ステージのタイトル表示
        StageTitle(GB_StageCount);
    }

    /// <summary> 
    /// ゲーム初期化
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void Initialize()
    {
        //各画面非表示
        ScreenHide();

        //HPをセット
        SC_BarManager.OptionSet(GB_PlayerHP);

        //スコア初期化
        UpdateScoreText();
    }

    /// <summary> 
    /// パネル非表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void ScreenHide()
    {
        OB_MyTurnPanel.SetActive(false);
        OB_EnemyTurnPanel.SetActive(false);
        OB_PausePanel.SetActive(false);
        OB_WarningPanel.SetActive(false);
        OB_GameOverPanel.SetActive(false);
        OB_ClearPanel.SetActive(false);
    }

    /// <summary> 
    /// 能力テキスト更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void UpdateScoreText()
    {
        SC_ScoreManager.Hp(GB_PlayerHP);
        SC_ScoreManager.BallCount(GB_BallCountMAX, GB_BallDecrement);
        SC_ScoreManager.BallCounter(GB_BallCounter);
        SC_ScoreManager.Power(GB_PowerCount);
        SC_ScoreManager.Exp(GB_ExpCount);
    }


    /// <summary> 
    /// ステージタイトル表示
    /// </summary> 
    /// <param name="pm_StageNumber">ステージ番号</param> 
    /// <returns>なし</returns>
    //private async void StageTitle(int pm_StageNumber)
    private void StageTitle(int pm_StageNumber)
    {
        StartCoroutine("StageTitletCoroutine", pm_StageNumber);
    }

    private IEnumerator StageTitletCoroutine(int pm_StageNumber)
    {
        string[] W_StageTitleName = new string[8] { $"Forest", $"Desert", $"Ghost", $"Water", $"Flame", $"Sky", $"Space", $"Nightmare" };

        //ステージ番号とタイトルを表示
        OB_StageNumText.text = $"{pm_StageNumber + 1} Stage";
        OB_StageTitText.text = W_StageTitleName[pm_StageNumber];

        //ステージの画像をセット
        StageImageSet(pm_StageNumber + 1);

        //ステージタイトルパネルを再表示
        OB_TitlePanel.SetActive(false);
        OB_TitlePanel.SetActive(true);

        //ステージタイトル画面終了待ち
        //await Task.Delay(5000);
        yield return new WaitForSeconds(5f);

        StageSelect();
    }

    /// <summary> 
    /// ステージ画面表示
    /// </summary> 
    /// <param name="pm_ImageNumber">ステージ画像番号</param> 
    /// <returns>なし</returns> 
    private void StageImageSet(int pm_ImageNumber)
    {
        GameObject OB_Stage = GameObject.Find("StageImage");
        Image W_StageImage;
        string W_StageNameNumber = "image_stage" + pm_ImageNumber.ToString();

        //ステージの画像セット
        W_StageImage = OB_Stage.GetComponent<Image>();
        W_StageImage.sprite = Resources.Load<Sprite>(W_StageNameNumber);
    }

    /// <summary> 
    /// ステージ初期化
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    public void StageSelect()
    {
        GB_GameOverFlag = false;

        //ステージのBGMを流す(通常エリア)
        SC_GameSound.StageSound(GB_StageCount, false);

        SC_StageManager.Area(GB_StageCount, 0, GB_PowerCount);
    }

    /// <summary> 
    /// エリア表示後の処理
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    //public async void AreaAfterMyTurnDisplay()
    public void AreaAfterMyTurnDisplay()
    {
        StartCoroutine("AreaAfterMyTurnDisplayCoroutine");
    }

    /// <summary> 
    /// エリア表示後の処理(待ち処理)
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private IEnumerator AreaAfterMyTurnDisplayCoroutine()
    {
        //他の自攻撃オブジェクトが反映しないように防ぐ
        GB_TurnFlag = true;

        //バーを表示
        OB_Bar.SetActive(true);
        OB_BarRad.SetActive(true);

        //自分ターン画面表示
        OB_MyTurnPanel.SetActive(false);
        OB_MyTurnPanel.SetActive(true);

        //自分ターン画面表示
        //await Task.Delay(1000);
        yield return new WaitForSeconds(1f);

        //自攻撃オブジェクトがショットできるようにする
        GB_BallShotFlag = true;
    }

    /// <summary> 
    /// 自分のターン表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    //public async void MyTurnDisplay()
    public void MyTurnDisplay()
    {
        StartCoroutine("MyTurnDisplayCoroutine");
    }

    /// <summary> 
    /// 自分のターン表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private IEnumerator MyTurnDisplayCoroutine()
    {
        //自攻撃オブジェクト削除反映待ち
        //await Task.Delay(1);
        yield return new WaitForSeconds(0.001f);

        //敵の攻撃クローンがなくなった　且　敵のターンである場合
        if (OB_BlocksAttackClone.transform.childCount == 0 && GB_TurnFlag == false)
        {
            //他の自攻撃オブジェクトが反映しないように防ぐ
            GB_TurnFlag = true;

            //バーを表示
            OB_Bar.SetActive(true);
            OB_BarRad.SetActive(true);

            //自分ターン画面表示
            OB_MyTurnPanel.SetActive(false);
            OB_MyTurnPanel.SetActive(true);

            //自分ターン画面表示
            //await Task.Delay(1000);
            yield return new WaitForSeconds(1f);

            //自攻撃オブジェクトがショットできるようにする
            GB_BallShotFlag = true;
        }
    }

    /// <summary> 
    /// 敵のターン表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    //public async void EnemyTurnDisplay()
    public void EnemyTurnDisplay()
    {
        StartCoroutine("EnemyTurnDisplayCoroutine");
    }

    /// <summary> 
    /// 敵のターン表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private IEnumerator EnemyTurnDisplayCoroutine()
    {
        //敵攻撃オブジェクト削除反映待ち
        //await Task.Delay(1);
        yield return new WaitForSeconds(0.001f);

        //自分の攻撃クローンがなくなった　且　自分のターンである場合
        if (OB_BallsClone.transform.childCount == 0 && GB_TurnFlag == true && GB_CoroutineFlag == true && OB_BlocksClone.transform.childCount > 0)
        {
            //他の敵攻撃オブジェクトが反映しないように防ぐ
            GB_TurnFlag = false;

            //最大ボールの数のスコアを初期化
            GB_BallDecrement = 0;
            SC_ScoreManager.BallCount(GB_BallCountMAX, GB_BallDecrement);

            //バーを非表示
            OB_Bar.SetActive(false);
            OB_BarRad.SetActive(false);

            //敵ターン画面表示
            OB_EnemyTurnPanel.SetActive(false);
            OB_EnemyTurnPanel.SetActive(true);

            //敵ターン画面表示終了待ち
            //await Task.Delay(1000);
            yield return new WaitForSeconds(1f);

            SC_EnemyManager.EnemyTurn();
        }
    }

    /// <summary> 
    /// スコアマネージャにスコアをセット
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    public void ScoreSet()
    {
        //スコアをセット
        SC_ScoreManager.ScoreSet(GB_Score);
    }

    /// <summary> 
    /// 次のステージ表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    public void NextStage()
    {
        EnemyDelete();

        OB_ClearPanel.SetActive(false);

        //BGMをフェードアウト
        SC_GameSound.SoundFadeOutAsyn();

        //ステージ番号１つカウント
        GB_StageCount++;

        //ステージタイトル画面表示
        StageTitle(GB_StageCount);

        //ボールを削除
        BallDelete();
    }

    /// <summary> 
    /// クリア画面を表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    public void ClearStage()
    {
        //スコアを取得
        GB_Score = SC_ScoreManager.ScoreGet();

        //現在のステータスをクリア画面に更新
        if (GB_StageCount < 7) 
        {
            SC_ClearManager.Score(GB_Score);
            SC_ClearManager.Power(GB_PowerCount);
            SC_ClearManager.BallMax(GB_BallCountMAX);
            SC_ClearManager.BallCounter(GB_BallCounter);
            SC_ClearManager.Exp(GB_ExpCount);
            SC_ClearManager.Display(true);
        }
        else
        {
            SC_ClearManager.Score(GB_Score);
            SC_ClearManager.Display(false);
        }

        //クリア画面で最低ステータスを保持
        GB_PowerHoldCount = GB_PowerCount;
        GB_BallHoldCountMAX = GB_BallCountMAX;
        GB_BallCounterHold = GB_BallCounter;
        GB_ScoreHold = GB_Score;
    }

    /// <summary> 
    /// クリア画面で能力変更時の処理
    /// </summary> 
    /// <param name="pm_BonusName">ボタンの名前</param> 
    /// <returns>なし</returns> 
    public void ClearBonus(string pm_BonusName)
    {
        //能力変更ボタンを押下時に処理
        switch (pm_BonusName)
        {
            case "OB_BallMaxBackButton":

                if (GB_BallHoldCountMAX < GB_BallCountMAX)
                {
                    GB_ExpCount = GB_ExpCount + GB_BallMaxCost;
                    GB_BallCountMAX = GB_BallCountMAX - 5;
                    SC_ClearManager.BallMax(GB_BallCountMAX);
                }
                break;

            case "OB_BallMaxNextButton":

                if (GB_ExpCount - GB_BallMaxCost >= 0 && GB_BallCountMAX < 50)
                {
                    GB_ExpCount = GB_ExpCount - GB_BallMaxCost;
                    GB_BallCountMAX = GB_BallCountMAX + 5;
                    SC_ClearManager.BallMax(GB_BallCountMAX);
                }
                break;

            case "OB_PowerBackButton":

                if (GB_PowerHoldCount < GB_PowerCount)
                {
                    GB_ExpCount = GB_ExpCount + GB_PowerCost;
                    GB_PowerCount--;
                    SC_ClearManager.Power(GB_PowerCount);
                }
                break;

            case "OB_PowerNextButton":

                if (GB_ExpCount - GB_PowerCost >= 0)
                {
                    GB_ExpCount = GB_ExpCount - GB_PowerCost;
                    GB_PowerCount++;
                    SC_ClearManager.Power(GB_PowerCount);
                }
                break;

            case "OB_BallCounterBackButton":

                if (GB_BallCounterHold < GB_BallCounter)
                {
                    GB_ExpCount = GB_ExpCount + GB_BallCounterCost;
                    GB_BallCounter = GB_BallCounter - 5;
                    SC_ClearManager.BallCounter(GB_BallCounter);
                }
                break;

            case "OB_BallCounterNextButton":

                if (GB_ExpCount - GB_BallCounterCost >= 0 && GB_BallCounter < 50)
                {
                    GB_ExpCount = GB_ExpCount - GB_BallCounterCost;
                    GB_BallCounter = GB_BallCounter + 5;
                    SC_ClearManager.BallCounter(GB_BallCounter);
                }
                break;

            case "OB_ScoreBackButton":

                if (GB_ScoreHold < GB_Score)
                {
                    GB_ExpCount = GB_ExpCount + GB_ScoreCost;
                    GB_Score = GB_Score - 10000;
                    SC_ClearManager.Score(GB_Score);
                }
                break;

            case "OB_ScoreNextButton":

                if (GB_ExpCount - GB_ScoreCost >= 0)
                {
                    GB_ExpCount = GB_ExpCount - GB_ScoreCost;
                    GB_Score = GB_Score + 10000;
                    SC_ClearManager.Score(GB_Score);
                }
                break;

        }

        SC_ClearManager.Exp(GB_ExpCount);
    }

    /// <summary> 
    /// コンティニューボタン押下後の処理
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void Continue()
    {
        //ゲームオーバー画面を非表示
        OB_GameOverPanel.SetActive(false);

        //BGMをフェードアウト
        SC_GameSound.SoundFadeOutAsyn();

        //ステータス初期化
        GB_PlayerHP = 100;
        GB_ExpCount = 0;
        SC_BarManager.OptionSet(GB_PlayerHP);
        SC_ScoreManager.Hp(GB_PlayerHP);
        SC_ScoreManager.Exp(GB_ExpCount);
        SC_ScoreManager.ContinueScore();

        //ステージに表示しているオブジェクトをリセット
        BallDelete();
        EnemyDelete();
        GB_BallShotFlag = false;

        //ステージタイトル画面表示
        StageTitle(GB_StageCount);
    }

    /// <summary> 
    /// エリアに表示しているプレイヤーボールを削除
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    //public async void BallDelete()
    public void BallDelete()
    {
        StartCoroutine("BallDeleteCoroutine");
    }

    private IEnumerator BallDeleteCoroutine()
    {
        //自動的にボールショットを無効
        GB_CoroutineFlag = false;

        //表示しているボールを削除
        for (int i = 0; i < GB_Balls.Count; i++)
        {
            Destroy(GB_Balls[i]);
        }
        GB_Balls.Clear();

        //自動的にボールショットを無効解除
        //await Task.Delay(100);
        yield return new WaitForSeconds(0.1f);
        
        GB_CoroutineFlag = true;

        //ボール発射カウントを初期化
        GB_BallDecrement = 0;
        UpdateScoreText();
    }

    /// <summary> 
    /// エリアに表示している敵を削除
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void EnemyDelete()
    {
        //エリアに存在する敵を削除
        foreach (Transform childTransform in OB_BlocksClone.transform)
        {
            Destroy(childTransform.gameObject);
        }
        foreach (Transform childTransform in OB_BlocksAttackClone.transform)
        {
            Destroy(childTransform.gameObject);
        }
        foreach (Transform childTransform in OB_GimmicksClone.transform)
        {
            Destroy(childTransform.gameObject);
        }
    }

    /// <summary> 
    /// プレイヤーダメージ時にHPを更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void UpdateHpText(int pm_PlayerHP)
    {
        GB_PlayerHP = pm_PlayerHP;
        SC_ScoreManager.Hp(pm_PlayerHP);
    }

    /// <summary> 
    /// アイテム取得時にスコア変更
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void ItemGet(string ItemName)
    {
        //アイテム取得時ステータス変更
        switch (ItemName)
        {
            case "OB_ItemHp(Clone)":
                GB_PlayerHP = GB_PlayerHP + GB_HpGet;
                SC_ScoreManager.Hp(GB_PlayerHP);
                SC_BarManager.OptionSet(GB_PlayerHP);
                break;
            case "OB_ItemExp(Clone)":
                GB_ExpCount = GB_ExpCount + GB_ExeGet;
                SC_ScoreManager.Exp(GB_ExpCount);
                SC_ClearManager.Exp(GB_ExpCount);
                break;
        }
    }

    /// <summary> 
    /// ゲームクリアボタン押下後の処理
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void GameClear()
    {
        SceneManager.LoadScene("GameClearScene");
        Debug.Log("ゲームクリア...");
    }

    /// <summary> 
    /// ゲームオーバー画面を表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void GameOver()
    {
        OB_GameOverPanel.SetActive(true);
        GB_GameOverFlag = true;
        Debug.Log("ゲームオーバー...");
    }

    /// <summary> 
    /// バーとボールショットの制御
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void Update()
    {

        //Q押下でポーズ画面
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Pause();
        }

        //ポーズの有無
        if (GB_PauseFlag == false)
        {

            //バーの位置と角度を操作
            SC_BarManager.Move();
            SC_BarManager.Angle();

            //スペースキーでボール発射
            if (Input.GetKeyDown(KeyCode.Space) && (GB_BallShotFlag == true) && (GB_GameOverFlag == false))
            {
                GB_BallShotFlag = false;
                StartCoroutine("BallShotCoroutine");
            }
        }
    }


    /// <summary> 
    /// ポーズ画面表示と制御無効
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void Pause()
    {
        OB_PausePanel.SetActive(!GB_PauseFlag);
        GB_PauseFlag = !GB_PauseFlag;

        if (GB_PauseFlag == true)
        {
            //ゲーム画面を停止する
            Time.timeScale = 0;
        }
        else
        {
            //ゲーム画面を動かす
            Time.timeScale = 1;
        }
    }

    /// <summary> 
    /// ボールショットコルーチン(100ミリ秒後に処理実行)
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    IEnumerator BallShotCoroutine()
    {
        //ボールを一定間隔で発射
        while ((GB_BallDecrement < GB_BallCountMAX) && (GB_CoroutineFlag == true))
        {
            if (GB_PauseFlag == false)
            {
                BallShot();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary> 
    /// ボールのショット
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void BallShot()
    {
        //ボールの発射カウントを１つ加算
        GB_BallDecrement++;

        //スコア更新
        UpdateScoreText();

        //ボールの生成
        GameObject W_Ball = SC_BallManager.Create(GB_BallCounter);
        GB_Balls.Add(W_Ball);
    }
}
