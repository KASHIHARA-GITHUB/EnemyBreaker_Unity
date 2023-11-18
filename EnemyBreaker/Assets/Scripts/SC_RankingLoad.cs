using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;

public class SC_RankingLoad : MonoBehaviour
{
    [SerializeField] private Text CP_NameText  = null;  //名前テキスト
    [SerializeField] private Text CP_ScoreText = null;  //スコアテキスト

    /// <summary> 
    /// ランキング画面初期
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void Start()
    {
        string w_Name = "";
        string w_Score = "";

        //二フクラサイトのデータストア(ScoreClass)に登録
        NCMBQuery<NCMBObject> w_Query = new NCMBQuery<NCMBObject>("ScoreClass");
        w_Query.OrderByDescending("score");
        w_Query.Limit = 10;
        w_Query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            //つながらなかった場合エラーを表示
            if (e != null)
            {
                Debug.LogWarning("error: " + e.ErrorMessage);
            }
            else
            {
                //データストアから1位の順番から取得
                for (int i = 0; i < objList.Count; i++)
                {
                    //Debug.Log("ScoreRanking " + objList[i]["score"] + objList[i]["userName"]);
                    w_Name = w_Name + objList[i]["userName"] + "\n";
                    w_Score = w_Score + objList[i]["score"] + "\n";
                }
            }

            //テキストに反映
            CP_NameText.text = w_Name;
            CP_ScoreText.text = w_Score;

        });
    }
}