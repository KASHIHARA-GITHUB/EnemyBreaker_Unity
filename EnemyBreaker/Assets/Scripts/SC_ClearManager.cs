using UnityEngine;
using UnityEngine.UI;

public class SC_ClearManager : MonoBehaviour
{
    //オブジェクト変数
    [SerializeField] private GameObject OB_ClearPanel = null;       //クリア画面
    [SerializeField] private GameObject OB_ClearButton = null;      //クリアボタン(メインメニューに戻る)
    [SerializeField] private GameObject OB_NextButton = null;       //次へボタン(次のステージ)
    [SerializeField] private GameObject OB_RegistButton = null;     //登録ボタン(ランキングに登録)
    [SerializeField] private GameObject OB_StatusPanel = null;      //能力パネル
    [SerializeField] private GameObject OB_NoteText = null;         //警告テキスト

    //コンポーネント変数
    [SerializeField] private Text CP_ScoreText = null;              //スコアテキスト
    [SerializeField] private Text CP_ClearScoreText = null;         //クリアスコアテキスト
    [SerializeField] private Text CP_PowerText = null;              //パワーテキスト
    [SerializeField] private Text CP_BallMaxText = null;            //ボール最大テキスト
    [SerializeField] private Text CP_BallCounterText = null;        //ボールカウンターテキスト
    [SerializeField] private Text CP_ExpText = null;                //経験値テキスト

    /// <summary> 
    /// クリア画面の表示
    /// </summary> 
    /// <param name="pm_PanelDisplay">ボタンの表示(true:次のステージ画面 false:全ステージクリア画面)</param> 
    /// <returns>なし</returns>
    public void Display(bool pm_PanelDisplay)
    {
        //パネルを表示
        OB_ClearPanel.SetActive(true);
        OB_NoteText.SetActive(false);
        if (pm_PanelDisplay == true)
        {
            OB_ClearButton.SetActive(false);
            OB_StatusPanel.SetActive(true);
            OB_NextButton.SetActive(true);
            OB_RegistButton.SetActive(false);

        }
        else
        {
            OB_ClearButton.SetActive(true);
            OB_StatusPanel.SetActive(false);
            OB_NextButton.SetActive(false);
            OB_RegistButton.SetActive(true);
        }
    }

    /// <summary> 
    /// スコアをテキスト表示
    /// </summary> 
    /// <param name="pm_Score">スコア</param> 
    /// <returns>なし</returns>
    public void Score(int pm_Score)
    {
        CP_ScoreText.text = $"{pm_Score}";
        CP_ClearScoreText.text = $"{pm_Score}";
    }

    /// <summary> 
    /// 攻撃力をテキスト表示
    /// </summary> 
    /// <param name="pm_Power">攻撃力</param> 
    /// <returns>なし</returns>
    public void Power(int pm_Power)
    {
        CP_PowerText.text = $"{pm_Power}";
    }

    /// <summary> 
    /// ボール最大発射数をテキスト表示
    /// </summary> 
    /// <param name="pm_CountMAX">ボール最大発射数</param> 
    /// <returns>なし</returns>
    public void BallMax(int pm_CountMAX)
    {
        CP_BallMaxText.text = $"{pm_CountMAX}";
    }

    /// <summary> 
    /// ボールのカウンター数をテキスト表示
    /// </summary> 
    /// <param name="pm_BallCounter">ボール最大発射数</param> 
    /// <returns>なし</returns>
    public void BallCounter(int pm_BallCounter)
    {
        CP_BallCounterText.text = $"{pm_BallCounter}";
    }

    /// <summary> 
    /// 経験値をテキスト表示
    /// </summary> 
    /// <param name="pm_Exp">経験値</param> 
    /// <returns>なし</returns>
    public void Exp(int pm_Exp)
    {
        CP_ExpText.text = $"{pm_Exp}";
    }
}
