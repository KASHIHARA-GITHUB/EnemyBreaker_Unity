using UnityEngine;

//ドロップアイテムスクリプト
public class SC_DeathBlock: MonoBehaviour
{
    //スクリプト
    [SerializeField] private GameManager SC_GameManager = null;   //ゲームマネージャ

    /// <summary> 
    /// ぶつかったボールを削除する
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void OnCollisionEnter2D(Collision2D pm_Collision)
    {
        //ボールを削除
        Destroy(pm_Collision.gameObject);

        //敵のターン画面を表示
        SC_GameManager.EnemyTurnDisplay();
    }

}
