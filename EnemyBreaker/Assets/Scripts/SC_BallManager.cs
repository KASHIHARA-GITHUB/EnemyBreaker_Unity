using UnityEngine;

//ボール生成スクリプト
public class SC_BallManager : MonoBehaviour
{

    //オブジェクト変数
    [SerializeField] private GameObject OB_BallPrefab = null;         //ボール生成オブジェクト
    [SerializeField] private GameObject OB_BallClones = null;         //ボールクローンのグループ

    //コンポーネント変数
    [SerializeField] private Transform CP_Balls = null;               //ボールエリア
    [SerializeField] private RectTransform CP_BarRectTrans = null;   //バーの位置
    [SerializeField] private RectTransform CP_BarRadRectTrans = null; //バーの角度

    /// <summary> 
    /// ボール生成
    /// </summary> 
    /// <param name="pm_Hp">ボールの体力</param> 
    /// <returns>ボールオブジェクト</returns>
    public GameObject Create(int pm_Hp)
    {
        //ボール生成
        GameObject w_BallObject = Instantiate(OB_BallPrefab, CP_Balls) as GameObject;

        //バーの現在位置取得
        Vector2 w_BarPos = new Vector2(CP_BarRectTrans.localPosition.x, CP_BarRectTrans.localPosition.y);
        Vector2 w_2DPosition = new Vector2((int)w_BarPos.x, -470);

        //現在角度取得
        float w_AngleValue = (float)CP_BarRadRectTrans.rotation.z;

        //生成したボールの設定
        w_BallObject.GetComponent<RectTransform>().anchoredPosition = w_2DPosition;
        w_BallObject.transform.SetParent(OB_BallClones.transform);
        SC_Ball w_Ball = w_BallObject.GetComponent<SC_Ball>();
        w_Ball.OptionSet(w_AngleValue, pm_Hp);

        return w_BallObject;
    }
}