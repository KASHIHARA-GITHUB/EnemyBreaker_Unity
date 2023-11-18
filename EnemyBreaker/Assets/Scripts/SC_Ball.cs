using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class SC_Ball : MonoBehaviour
{
    //オブジェクト
    [SerializeField] private GameObject OB_Ground = null;         //ゲームドロップバー

    //スクリプト
    [SerializeField] private GameManager SC_GameManager = null;   //ゲームマネージャ

    //コンポーネント
    [SerializeField] private Canvas CP_Canvas = null;             //キャンバス
    [SerializeField] private Rigidbody2D CP_Rigid = null;         //ボールの角度
    [SerializeField] private RectTransform CP_Trans = null;           //ボールの位置

    /// <summary> 
    /// ボールの設定
    /// </summary> 
    /// <param name="pm_BallAngle">ボールの発射角度</param> 
    /// <param name="pm_Hp">ボールの体力</param> 
    /// <returns>なし</returns>
    public void OptionSet(float pm_BallAngle, int pm_Hp)
    {
    
        //ボールの発射角度設定
        Vector2 w_Direction = new Vector2(-2.6f * pm_BallAngle, 1).normalized;
        CP_Rigid.velocity = w_Direction * 600 * CP_Canvas.transform.localScale.x;
        
        BallBreak(pm_Hp);
        BallDrop();

    }

    /// <summary> 
    /// ボールの衝突時に処理
    /// </summary> 
    /// <param name="pm_Collision">衝突オブジェクト</param> 
    /// <returns>なし</returns>
    private void OnCollisionEnter2D(Collision2D pm_Collision)
    {
        float w_Speed = 1.002f;

        if (pm_Collision.gameObject.name == "OB_Bar")
        {
            BarBallPoint(w_Speed, pm_Collision);
        }
        else
        {
            //速度増加
            //if (CP_Rigid.velocity.magnitude < 20.0f)
            //{
                CP_Rigid.velocity = CP_Rigid.velocity * w_Speed;
            //}
        }
    }

    /// <summary> 
    /// ボールとバーの位置を計算し、ボールの発射角度を決める
    /// </summary> 
    /// <param name="pm_Speed">ボールの速度</param>
    /// <param name="pm_Collision">衝突時のバーの位置</param> 
    /// <returns>なし</returns>
    private void BarBallPoint(float pm_Speed, Collision2D pm_Collision)
    {
        //プレイヤーとボールの位置取得
        Vector2 w_PlayerPos = pm_Collision.transform.position;
        Vector2 w_BallPos = CP_Trans.position;

        //角度計算
        Vector2 w_Direction = (w_BallPos - w_PlayerPos).normalized;

        //速度設定
        pm_Speed = CP_Rigid.velocity.magnitude;

        // 速度増加
        //if (CP_Rigid.velocity.magnitude < 20.0f)
        //{
            CP_Rigid.velocity = w_Direction * pm_Speed;
        //}
    }


    /// <summary> 
    /// ボールの体力設定と破壊時の処理
    /// </summary> 
    /// <param name="pm_Hp">ボールの体力</param> 
    /// <returns>なし</returns>
    private void BallBreak(int pm_Hp)
    {
        int w_Gage = 255 / pm_Hp;
        int w_Color=255;
        ReactiveProperty<int> w_HitPoint = new ReactiveProperty<int>(pm_Hp);
        
        //ボールがぶつかった
        this.OnCollisionEnter2DAsObservable()
            .Subscribe(
                collision =>
                {
                    //ボールが物にぶつかると色が変化する
                    w_HitPoint.Value = w_HitPoint.Value - 1;
                    w_Color = w_Color - w_Gage;
                    this.gameObject.GetComponent<Image>().color = new Color32(255, (byte)w_Color, (byte)w_Color, 255);
                }
            ).AddTo(this);
        //ボールの体力が0になった
        w_HitPoint
        .Where(hp => hp <= 0)
        .Subscribe(
            _ =>
            {
                //オブジェクトを削除
                Destroy(this.gameObject);

                //敵のターン画面を表示
                SC_GameManager.EnemyTurnDisplay();
            }
        ).AddTo(this);
    }

    /// <summary> 
    /// ボールが落ちた時の処理
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void BallDrop()
    {
        //ボールが落ちた時、処理を行う
        OB_Ground.OnTriggerEnter2DAsObservable()
            .Subscribe(
                collider =>
                {
                    //ボールオブジェクト要素数チェック
                    if (collider.gameObject.name == "OB_Ball(Clone)")
                    {
                        //オブジェクト削除
                        Destroy(collider.gameObject);

                        //敵のターン画面を表示
                        SC_GameManager.EnemyTurnDisplay();
                    }
                }
            );
    }
}
