using UnityEngine;
using UnityEngine.UI;
using NCMB;

public class SC_RankingRegister : MonoBehaviour
{
    //グローバル変数
    private int GB_Score;

    //オブジェクト
    [SerializeField] private GameObject OB_NoteText = null;     //警告テキスト(表示)

    //コンポーネント
    [SerializeField] private Button CP_ScoreRegister = null;    //スコア登録ボタン(押下後に非活性化のため)
    [SerializeField] private Text CP_InputText = null;          //テキスト入力
    [SerializeField] private Text CP_NoteText = null;           //警告テキスト(テキストの文字長さ、テキストの変更)

    /// <summary> 
    /// サーバに名前とスコアを登録
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void Register()
    {
        OB_NoteText.SetActive(true);

        //文字数を1文字以上10文字以下で入力
        if (CP_InputText.text.Length > 0 && CP_InputText.text.Length <= 10)
        {
            //警告メッセージに表示
            CP_NoteText.text = "登録しました。";

            //ボタンを非活性
            CP_ScoreRegister.interactable = false;

            //DBに登録
            NCMBObject w_ScoreClass = new NCMBObject("ScoreClass");
            w_ScoreClass["score"] = GB_Score;
            w_ScoreClass["userName"] = CP_InputText.text;
            w_ScoreClass.SaveAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    Debug.Log("Error: " + e.ErrorMessage);
                }
                else
                {
                    Debug.Log("success");
                }
            });
        }
        else
        {
            //警告メッセージに表示
            CP_NoteText.text = "1文字以上10字以下で入力ください";
        }


    }

    /// <summary> 
    /// スコアを保持
    /// </summary> 
    /// <param name="pm_Score">スコア</param> 
    /// <returns>なし</returns>
    public void ScoreSet(int pm_Score)
    {
        GB_Score = pm_Score;
    }
}