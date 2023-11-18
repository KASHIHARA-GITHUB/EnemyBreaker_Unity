using UnityEngine.SceneManagement;
using UnityEngine;

//ボタン処理スクリプト
public class SC_Button : MonoBehaviour
{
    //スクリプト
    [SerializeField] private GameManager SC_GameManager = null;             //ゲームマネージャ
    [SerializeField] private SC_GameTitleManager SC_GameTitleManager = null;  //プログレスマネージャ
    [SerializeField] private SC_RankingRegister SC_RankingRegister = null;  //ランキングボタン(押下後に非活性化のため)
    [SerializeField] private SC_Manual SC_Manual = null;  //ランキングボタン(押下後に非活性化のため)

    /// <summary> 
    /// ボタンクリック時の処理
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void OnClick()
    {
        //ボタンの音を取得
        AudioSource CP_GameSE = GameObject.Find("GameButtonSE").GetComponent<AudioSource>();
        CP_GameSE.Play();

        //ボタン名確認
        Debug.Log(transform.name);

        //各ボタンの処理
        switch (transform.name)
        {
            case "OB_GameStartButton":
                //ゲーム画面へ遷移する
                SC_GameTitleManager.Loading();
                break;
            case "OB_ManualButton":
                //マニュアル画面に遷移する
                SceneManager.LoadScene("GameManual");
                break;
            case "OB_ManualNextButton":
                SC_Manual.PageUpdate(false);
                break;
            case "OB_ManualBackButton":
                SC_Manual.PageUpdate(true);
                break;
            case "OB_RankingButton":
                //ランキング画面に遷移する
                SceneManager.LoadScene("GameRanking");
                break;
            case "OB_ScoreRegistButton":
                //ランキングに登録する
                SC_GameManager.ScoreSet();
                SC_RankingRegister.Register();
                break;
            case "OB_BallMaxBackButton":
            case "OB_BallMaxNextButton":
            case "OB_PowerBackButton":
            case "OB_PowerNextButton":
            case "OB_BallCounterBackButton":
            case "OB_BallCounterNextButton":
            case "OB_ScoreBackButton":
            case "OB_ScoreNextButton":
                //クリア画面で能力変更
                SC_GameManager.ClearBonus(transform.name);
                break;
            case "OB_NextStageButton":
                //次のステージ画面を表示
                SC_GameManager.ScoreSet();
                SC_GameManager.NextStage();
                break;
            case "OB_ContinueButton":
                //最初のエリアに遷移する(現在のステージを表示)
                SC_GameManager.Continue();
                break;
            case "OB_GameOverButton":
            case "OB_MainMenuButton":
                //タイトル画面に遷移する
                SceneManager.LoadScene("GameTitle");
                break;
            default:
                break;
        }
    }
}
