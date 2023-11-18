using UnityEngine;

public class SC_Warp: MonoBehaviour
{
    private Vector2[] GB_Position;  //宛先ワープの位置

    /// <summary> 
    /// ボールがぶつかったときにワープする(反射)
    /// </summary> 
    /// <param name="pm_Collision">衝突オブジェクト</param> 
    /// <returns>なし</returns>
    private void OnCollisionEnter2D(Collision2D pm_Collision)
    {
        Vector2 vector2 = new Vector2(0, 0);

        //アイテム取得音
        if (UnityEngine.Random.value > 0.8)
        {
            vector2 = GB_Position[0];
        }
        else if (UnityEngine.Random.value > 0.5)
        {
            vector2 = GB_Position[1];
        }
        else
        {
            vector2 = GB_Position[2];
        }

        pm_Collision.gameObject.GetComponent<RectTransform>().anchoredPosition = vector2;
    }

    /// <summary> 
    /// ボールがぶつかったときにワープする(貫通)
    /// </summary> 
    /// <param name="pm_Collision">衝突オブジェクト</param> 
    /// <returns>なし</returns>
    private void OnTriggerEnter2D(Collider2D pm_Collision)
    {
        Vector2 vector2 = new Vector2(0, 0);

        //アイテム取得音
        if (UnityEngine.Random.value > 0.8)
        {
            vector2 = GB_Position[0];
        }
        else if (UnityEngine.Random.value > 0.5)
        {
            vector2 = GB_Position[1];
        }
        else
        {
            vector2 = GB_Position[2];
        }
        
        pm_Collision.gameObject.GetComponent<RectTransform>().anchoredPosition = vector2;
    }

    /// <summary> 
    /// 宛先ワープの位置をセット
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void PositionSet(Vector2[] w_GimmickVector)
    {
        GB_Position = w_GimmickVector;
    }
}
