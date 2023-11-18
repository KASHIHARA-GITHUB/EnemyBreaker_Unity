using UnityEngine;
using UniRx;
using UniRx.Triggers;

//ドロップアイテムスクリプト
public class SC_Item : MonoBehaviour
{

    //スクリプト
    [SerializeField] private SC_GameSE SC_GameSE = null;            //アイテム取得音
    [SerializeField] private GameManager SC_GameManager = null;     //ゲームマネージャー

    //オブジェクト
    [SerializeField] private GameObject OB_Ground = null;           //アイテムドロップ

    /// <summary> 
    /// バーがアイテムにふれたときに処理する
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void OnCollisionEnter2D()
    {
        //アイテム取得音
        SC_GameSE.ItemGetSound();
        SC_GameManager.ItemGet(gameObject.name);

        //アイテムを削除
        Destroy(gameObject);
    }

    /// <summary> 
    /// アイテムがアイテムドロップバーに当たったときに処理する
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void BallDrop()
    {
        //アイテムが落ちた時、処理を行う
        OB_Ground.OnTriggerEnter2DAsObservable()
            .Subscribe(
                collider =>
                {
                    //ボールオブジェクト要素数チェック
                    if (collider.gameObject.name == "OB_ItemHp(Clone)" || collider.gameObject.name == "OB_ItemExp(Clone)")
                    {
                        //オブジェクト削除
                        Destroy(collider.gameObject);
                    }
                }
            );
    }
}
