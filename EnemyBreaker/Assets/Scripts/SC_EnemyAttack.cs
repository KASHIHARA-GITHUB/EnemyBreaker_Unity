using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class SC_EnemyAttack : MonoBehaviour
{
    //オブジェクト
    [SerializeField] private GameObject OB_EnemiesAttackArea = null;   //攻撃クローン
    [SerializeField] private GameObject[] OB_SkillObject = null;         //スキルオブジェクト
    [SerializeField] private GameObject OB_Ground = null;              //ゲームドロップバー

    //スクリプト
    [SerializeField] private GameManager SC_GameManager = null;        //ゲームマネージャ

    //コンポーネント
    [SerializeField] private RectTransform CP_PlayerTrans = null;      //プレイヤーの位置
    [SerializeField] private Canvas CP_Canvas = null;                  //キャンバスの大きさ

    /// <summary> 
    /// 一発ずつ球を発射する
    /// </summary> 
    /// <param name="pm_Position">敵の位置</param> 
    /// <returns>なし</returns>
    public void RainBall(Vector2 pm_Position, int pm_BallShotCount, int pm_ShotLevel, int pm_BallCircleDivision)
    {
        int w_BallCircleAngle = 0;
        int w_BallAngleUnit = 360 / pm_BallCircleDivision;

        float w_BallAngle_X = 0;
        float w_BallAngle_Y = 0;

        GameObject w_SkillObject;

        //球の数を決める
        List<int> w_BallList = new List<int>();
        for (int y = 0; y < pm_BallShotCount; y++)
        {
            for (int x = 0; x < pm_BallCircleDivision; x++)
            {
                if ((x * w_BallAngleUnit) > 100 && (x * w_BallAngleUnit) < 260)
                {
                    w_BallList.Add(x * w_BallAngleUnit);
                }
            }
            for (int x = 0; x < pm_BallCircleDivision; x++)
            {
                if ((-1 * x * w_BallAngleUnit) < -100 && (-1 * x * w_BallAngleUnit) > -260)
                {
                    w_BallList.Add(-1 * x * w_BallAngleUnit);
                }
            }
        }

        //攻撃オブジェクト生成
        for (int x = 0; x < w_BallList.Count; x++)
        {
            //攻撃オブジェクト生成
            w_SkillObject = Instantiate(OB_SkillObject[pm_ShotLevel], OB_EnemiesAttackArea.GetComponent<RectTransform>()) as GameObject;
            w_SkillObject.GetComponent<RectTransform>().anchoredPosition = pm_Position;
            w_SkillObject.transform.SetParent(OB_EnemiesAttackArea.transform);

            //ボールショットの角度計算
            w_BallAngle_X = Mathf.Sin(w_BallList[x] * Mathf.Deg2Rad + w_BallCircleAngle * Mathf.Deg2Rad);
            w_BallAngle_Y = Mathf.Cos(w_BallList[x] * Mathf.Deg2Rad + w_BallCircleAngle * Mathf.Deg2Rad);
            AttackBallShot(w_BallAngle_X, w_BallAngle_Y, x, w_SkillObject);
        }
    }

    /// <summary> 
    /// まとめて球を発射する
    /// </summary> 
    /// <param name="pm_Position">敵の位置</param> 
    /// <returns>なし</returns>
    public void WholeBall(Vector2 pm_Position, int pm_BallShotCount, int pm_ShotLevel, int pm_BallCircleDivision)
    {   
        int w_BallCircleAngle = 0;
        int w_BallAngleUnit = 360 / pm_BallCircleDivision;
        int w_BallWithShot = 0;

        float w_BallAngle_X = 0;
        float w_BallAngle_Y = 0;

        GameObject w_SkillObject;

        //一括で発射できる発射範囲を計算(100度～260度の範囲)
        for (int x = 0; x < pm_BallCircleDivision; x++)
        {
            if ((x * w_BallAngleUnit) > 100 && (x * w_BallAngleUnit) < 260)
            {
                w_BallWithShot++;
            }
        }

        //球の数を決める
        List<int> w_BallList = new List<int>();
        for (int y = 0; y < pm_BallShotCount; y++)
        {
            for (int x = 0; x < pm_BallCircleDivision; x++)
            {
                if ((x * w_BallAngleUnit) > 100 && (x * w_BallAngleUnit) < 260)
                {
                    w_BallList.Add(x * w_BallAngleUnit);
                }
            }
        }

        //５方向ボール
        for (int x = 0; x < w_BallList.Count; x++)
        {
            //攻撃オブジェクト生成
            w_SkillObject = Instantiate(OB_SkillObject[pm_ShotLevel], OB_EnemiesAttackArea.GetComponent<RectTransform>()) as GameObject;
            w_SkillObject.GetComponent<RectTransform>().anchoredPosition = pm_Position;
            w_SkillObject.transform.SetParent(OB_EnemiesAttackArea.transform);

            //ボールショットの角度計算
            w_BallAngle_X = Mathf.Sin(w_BallList[x] * Mathf.Deg2Rad + w_BallCircleAngle * Mathf.Deg2Rad);
            w_BallAngle_Y = Mathf.Cos(w_BallList[x] * Mathf.Deg2Rad + w_BallCircleAngle * Mathf.Deg2Rad);
            AttackBallShot(w_BallAngle_X, w_BallAngle_Y, x/w_BallWithShot, w_SkillObject);
        }
    }

    /// <summary> 
    /// 球の発射
    /// </summary> 
    /// <param name="pm_BallAngle_X">攻撃弾の角度X</param> 
    /// <param name="pm_BallAngle_Y">攻撃弾の角度Y</param> 
    /// <param name="pm_TimeCount">弾の発射時間</param> 
    /// <param name="pm_SkillObject">弾クローン</param> 
    /// <returns>なし</returns>
    public void AttackBallShot(float pm_BallAngle_X, float pm_BallAngle_Y, int pm_TimeCount, GameObject pm_SkillObject)
    {
        BallDrop(pm_SkillObject);

        //コンポーネント変数の宣言
        Rigidbody2D w_SkillObject = pm_SkillObject.GetComponent<Rigidbody2D>();

        //時間をずらしてボールを発射
        StartCoroutine(DelayMethod(0.1f * (float)pm_TimeCount, () =>
        {
            Vector2 w_Direction = new Vector2(1f * pm_BallAngle_X, 1f * pm_BallAngle_Y).normalized;
            w_SkillObject.velocity = w_Direction * 300 * CP_Canvas.transform.localScale.x;
        }));
    }

    /// <summary> 
    /// プレイヤーの位置に球を発射する
    /// </summary> 
    /// <param name="pm_Position">敵の位置</param> 
    /// <returns>なし</returns>
    public void HomingBall(Vector2 pm_Position, int pm_BallShotCount, int pm_ShotLevel)
    {
        GameObject w_SkillObject;

        for (int i = 0; i < pm_BallShotCount; i++)
        {
            w_SkillObject = Instantiate(OB_SkillObject[pm_ShotLevel], OB_EnemiesAttackArea.GetComponent<RectTransform>()) as GameObject;
            w_SkillObject.GetComponent<RectTransform>().anchoredPosition = pm_Position;
            w_SkillObject.transform.SetParent(OB_EnemiesAttackArea.transform);

            BallDrop(w_SkillObject);

            HomingBallShot(pm_Position, i,w_SkillObject);
        }
            
    }

    /// <summary> 
    /// 球の発射
    /// </summary> 
    /// <param name="pm_BallAngle_X">攻撃弾の角度X</param> 
    /// <param name="pm_BallAngle_Y">攻撃弾の角度Y</param> 
    /// <param name="pm_TimeCount">弾の発射時間</param> 
    /// <param name="pm_SkillObject">弾クローン</param> 
    /// <returns>なし</returns>
    public void HomingBallShot(Vector2 pm_Position, int pm_TimeCount, GameObject pm_SkillObject)
    {

        //コンポーネント変数の宣言
        Rigidbody2D w_HomingObject = pm_SkillObject.GetComponent<Rigidbody2D>();

        //時間をずらしてボールを発射
        StartCoroutine(DelayMethod(0.1f * (float)pm_TimeCount, () =>
        {
            Vector2 w_PlayerPos = CP_PlayerTrans.anchoredPosition;
            Vector2 w_Direction = (w_PlayerPos - pm_Position).normalized;
            w_HomingObject.velocity = w_Direction * 450 * CP_Canvas.transform.localScale;
        }));
    }

    /// <summary> 
    /// 処理待ち時間
    /// </summary> 
    /// <param name="pm_WaitTime">待ち時間</param> 
    /// <param name="pm_Action">処理</param> 
    /// <returns>なし</returns>
    private IEnumerator DelayMethod(float pm_WaitTime, Action pm_Action)
    {
        yield return new WaitForSeconds(pm_WaitTime);
        pm_Action();
    }

    /// <summary> 
    /// 攻撃弾削除
    /// </summary> 
    /// <param name="pm_SkillObject">攻撃弾</param> 
    /// <returns>なし</returns>
    private void BallDrop(GameObject pm_SkillObject)
    {
        //ボールが落ちた時、処理を行う
        OB_Ground.OnTriggerEnter2DAsObservable()
            .Subscribe(
                collider =>
                {
                    //敵の攻撃オブジェクト要素数チェック
                    if (collider.gameObject.name == "OB_AttackBall(Clone)"    ||
                        collider.gameObject.name == "OB_AttackBallTwo(Clone)" ||
                        collider.gameObject.name == "OB_AttackBallThree(Clone)")
                    {
                        //オブジェクト削除
                        Destroy(collider.gameObject);

                        //プレイヤーのターン画面を表示
                        SC_GameManager.MyTurnDisplay();
                    }
                }
            );
        
    }
}