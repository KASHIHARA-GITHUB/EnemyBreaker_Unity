using UnityEngine;
using UnityEngine.UI;

//バースクリプト
public class SC_BarManager : MonoBehaviour
{
    //オブジェクト変数
    [SerializeField] private GameObject OB_Player = null;       //ゲームプレイヤー

    //スクリプト変数
    [SerializeField] private GameManager SC_GameManager = null; //ゲームマネージャ
    [SerializeField] private SC_GameSE SC_GameSE = null;        //ゲーム効果音

    //コンポーネント変数
    [SerializeField] private RectTransform CP_BarRectTransform = null;      //ボールバー位置
    [SerializeField] private RectTransform CP_BarRadRectTransform = null;   //ボールバー角度
    [SerializeField] private RectTransform CP_Player = null;                //プレイヤー位置

    //グローバル変数
    private int GB_Hp;          //プレイヤーHP
    private float GB_NewX;      //バーの位置更新

    /// <summary> 
    /// バーの位置更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void Move()
    {
        //変数宣言
        float w_CurrentX = CP_BarRadRectTransform.localPosition.x;
        
        Vector3  w_Pos;

        //移動最大距離範囲
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GB_NewX = Mathf.Min(w_CurrentX + 2, 150);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            GB_NewX = Mathf.Max(w_CurrentX - 2, -150);
        }

        //バーの現在位置
        w_Pos = new Vector3(GB_NewX, CP_BarRectTransform.localPosition.y, 0);
        CP_BarRectTransform.localPosition    = w_Pos;
        w_Pos = new Vector3(GB_NewX, CP_BarRadRectTransform.localPosition.y, 0);
        CP_BarRadRectTransform.localPosition = w_Pos;
        w_Pos = new Vector3(GB_NewX, CP_Player.localPosition.y, 0);
        CP_Player.localPosition = w_Pos;
    }

    /// <summary> 
    /// バーの角度更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void Angle()
    {

        //角度最大範囲
        if (Input.GetKey(KeyCode.X))
        {
            if (CP_BarRadRectTransform.rotation.z < 0.4)
            {
                CP_BarRadRectTransform.Rotate(new Vector3(0, 0, 0.3f));
            }
        }
        else if (Input.GetKey(KeyCode.C))
        {
            if (CP_BarRadRectTransform.rotation.z > -0.4)
            {
                CP_BarRadRectTransform.Rotate(new Vector3(0, 0, -0.3f));
            }
        }
    }

    /// <summary> 
    /// プレイヤーのHP更新
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void OptionSet(int pm_Hp)
    {
        GB_Hp = pm_Hp;
    }

    /// <summary> 
    /// プレイヤーがダメージ受けた時に処理
    /// </summary> 
    /// <param name="collision">プレイヤーの衝突物</param> 
    /// <returns>なし</returns>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "OB_Ball(Clone)" &&
            collision.gameObject.name != "OB_ItemHp(Clone)" &&
            collision.gameObject.name != "OB_ItemExp(Clone)")
        {

            //オブジェクト削除
            Destroy(collision.gameObject);

            //ぶつかった音
            SC_GameSE.BlockHitSound();

            //ダメージ後、瞬時、赤色を表示
            OB_Player.GetComponent<Image>().color = Color.red;
            Invoke("StatusInit", 0.1f);

            //プレイヤーのHP判定
            if(GB_Hp > 0){
                switch (collision.gameObject.name)
                {
                    case "OB_AttackBall(Clone)":
                        GB_Hp = GB_Hp - 1;
                        break;
                    case "OB_AttackBallTwo(Clone)":
                        GB_Hp = GB_Hp - 2;
                        break;
                    case "OB_AttackBallThree(Clone)":
                        GB_Hp = GB_Hp - 3;
                        break;
                    default:
                        break;
                }
                //プレイヤーのHPを更新    
                SC_GameManager.UpdateHpText(GB_Hp);

                //プレイヤーのターン画面を表示
                SC_GameManager.MyTurnDisplay();
            } else {
                SC_GameManager.GameOver();
            }
        }
    }

    /// <summary> 
    /// ダメージ時に赤くする
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void StatusInit()
    {
        OB_Player.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }
}
