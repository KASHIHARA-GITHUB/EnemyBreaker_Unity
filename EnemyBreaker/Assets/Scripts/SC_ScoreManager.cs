using UnityEngine;
using UnityEngine.UI;

public class SC_ScoreManager : MonoBehaviour
{
    //コンポーネント変数
    [SerializeField] private Text CP_ScoreText = null;              //スコア
    [SerializeField] private Text CP_BallCountMaxText = null;       //ボール最大数
    [SerializeField] private Text CP_BallCounterText = null;        //ボールの体力
    [SerializeField] private Text CP_HpCountText = null;            //プレイヤー体力
    [SerializeField] private Text CP_PowerText = null;              //プレイヤー攻撃力
    [SerializeField] private Text CP_ExpText = null;                //プレイヤー経験値

    //スクリプト変数
    [SerializeField] private SC_RankingRegister SC_RankingRegister = null;  //

    private int GB_Score;   //スコア

    /// <summary> 
    /// 経験値取得時のスコア更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void UpdateScore(int pm_Score)
    {
        //現在の得点を表示
        GB_Score += pm_Score;
        CP_ScoreText.text = $"Score: {GB_Score}";
        SC_RankingRegister.ScoreSet(GB_Score);
    }

    /// <summary> 
    /// コンティニュー時のスコア初期化
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void ContinueScore()
    {
        //スコアを初期化
        GB_Score = 0;
        CP_ScoreText.text = $"Score: {GB_Score}";
        SC_RankingRegister.ScoreSet(GB_Score);
    }

    /// <summary> 
    /// スコアをゲームマネージャにセット
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>スコア</returns>
    public int ScoreGet()
    {
        return GB_Score;
    }

    /// <summary> 
    /// スコアをスコアマネージャにセット
    /// </summary> 
    /// <param name="pm_Score">なし</param> 
    /// <returns>なし</returns>
    public void ScoreSet(int pm_Score)
    {
        GB_Score = pm_Score;
        CP_ScoreText.text = $"Score: {GB_Score}";
        SC_RankingRegister.ScoreSet(GB_Score);
    }

    /// <summary> 
    /// ボールが発射できる数更新
    /// </summary> 
    /// <param name="pm_BallMAX">ボール最大数</param> 
    /// <param name="pm_BallDecrement">ボール発射回数</param> 
    /// <returns>なし</returns>
    public void BallCount(int pm_BallMAX, int pm_BallDecrement)
    {
        //ボールを発射できる数を表示
        CP_BallCountMaxText.text = $"BallMax: {pm_BallMAX - pm_BallDecrement}";
    }

    /// <summary> 
    /// ボールが壁にあたる回数
    /// </summary> 
    /// <param name="pm_BallCounter">ボール最大数</param> 
    /// <returns>なし</returns>
    public void BallCounter(int pm_BallCounter)
    {
        //ボールを発射できる数を表示
        CP_BallCounterText.text = $"BallCounter: {pm_BallCounter}";
    }

    /// <summary> 
    /// 体力更新
    /// </summary> 
    /// <param name="pm_Hp">プレイヤー体力</param> 
    /// <returns>なし</returns>
    public void Hp(int pm_Hp)
    {
        //ボールを発射した数を表示
        CP_HpCountText.text = $"Hp: {pm_Hp}";
    }

    /// <summary> 
    /// 攻撃力更新
    /// </summary> 
    /// <param name="pm_Power">プレイヤー体力></param> 
    /// <returns>なし</returns>
    public void Power(int pm_Power)
    {
        //現在のボールの攻撃力を表示
        CP_PowerText.text = $"Power: {pm_Power}";
    }

    /// <summary> 
    /// 経験値更新
    /// </summary> 
    /// <param name="pm_Exp">経験値></param> 
    /// <returns>なし</returns>
    public void Exp(int pm_Exp)
    {
        //現在の経験値を表示
        CP_ExpText.text = $"Exp: {pm_Exp}";
    }
}
