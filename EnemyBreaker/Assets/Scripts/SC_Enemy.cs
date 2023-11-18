using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

//敵のステータススクリプト
public class SC_Enemy : MonoBehaviour
{
    //オブジェクト変数
    [SerializeField] private GameObject OB_HpPrefab = null;     //体力アイテム
    [SerializeField] private GameObject OB_ExpPrefab = null;    //経験値アイテム

    //スクリプト変数
    [SerializeField] private SC_GameSE SC_GameSE = null;            //ゲーム効果音
    [SerializeField] private SC_EnemyAttack SC_EnemyAttack = null;  //敵の攻撃

    //コンポーネント変数
    [SerializeField] private Image CP_GreenGage = null;             //敵の体力ゲージ

    //グローバル変数
    public Subject<int> GB_OnBroken = new Subject<int>();           //敵破壊後のスコア監視
    private Vector2 GB_Position;                                    //敵の位置
    private string GB_SkillName;                                    //敵のスキル名

    /// <summary> 
    /// 敵のステータス設定
    /// </summary> 
    /// <param name="pm_Position">敵の位置</param> 
    /// <param name="pm_PowerCount">敵に受けるダメージ</param>
    /// <param name="pm_EnemyHP">敵の体力</param>
    /// <param name="pm_Score">敵のスコア</param>
    /// <param name="pm_SkillName">敵のスキル名</param>
    /// <returns>なし</returns>
    public void OptionSet(Vector2 pm_Position, int pm_PowerCount, int pm_EnemyHP, int pm_Score, string pm_SkillName)
    {
        //敵オブジェクトに変数を保存
        GB_Position = pm_Position;
        GB_SkillName = pm_SkillName;

        //敵の体力
        ReactiveProperty<int> w_HitPoint = new ReactiveProperty<int>(pm_EnemyHP);

        //敵のステータス設定
        EnemyDamage(w_HitPoint, pm_PowerCount, pm_EnemyHP, pm_Score);

    }

    /// <summary> 
    /// 敵のステータス設定
    /// </summary> 
    /// <param name="pm_HitPoint">敵の体力</param> 
    /// <param name="pm_PowerCount">敵に受けるダメージ</param>
    /// <param name="pm_EnemyGreenGame">敵の体力</param>
    /// <param name="pm_Score">敵のスコア</param>
    /// <returns>なし</returns>
    private void EnemyDamage(ReactiveProperty<int> pm_HitPoint, int pm_PowerCount, int pm_EnemyGreenGame, int pm_Score)
    {
        //ブロックがぶつかった
        this.OnCollisionEnter2DAsObservable()
            .Subscribe(
                collider =>
                {
                    //ぶつかった音
                    SC_GameSE.BlockHitSound();

                    //体力ゲージ現象
                    pm_HitPoint.Value = pm_HitPoint.Value - pm_PowerCount;
                    this.CP_GreenGage.fillAmount = CP_GreenGage.fillAmount - (pm_PowerCount / (float)pm_EnemyGreenGame);

                    //ダメージ後、瞬時、赤色を表示
                    gameObject.GetComponent<Image>().color = Color.red;
                    Invoke("StatusInit", 0.1f);
                }
            ).AddTo(this);
        //ブロックの体力が0になった
        pm_HitPoint
        .Where(hp => hp <= 0)
        .Subscribe(
            _ =>
            {
                //壊れた音
                SC_GameSE.BlockBreakSound();

                //スコア取得
                GB_OnBroken.OnNext(pm_Score);
                GB_OnBroken.OnCompleted();

                //アイテムドロップ生成関数
                ItemDrop();

                //オブジェクトを削除
                Destroy(this.gameObject);
            }
        ).AddTo(this);
    }

    /// <summary> 
    /// 敵の色を赤くする
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void StatusInit()
    {
        this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    /// <summary> 
    /// アイテムドロップ
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private void ItemDrop()
    {
        GameObject OB_Items = GameObject.Find("Items");

        if (UnityEngine.Random.value > 0.9)
        {
            GameObject w_Item = Instantiate(OB_HpPrefab, OB_Items.GetComponent<RectTransform>()) as GameObject;
            ItemOptionSet(w_Item);
        }
        else if (UnityEngine.Random.value > 0.5)
        {
            GameObject w_Item = Instantiate(OB_ExpPrefab, OB_Items.GetComponent<RectTransform>()) as GameObject;
            ItemOptionSet(w_Item);
        }
    }

    /// <summary> 
    /// アイテム設定
    /// </summary> 
    /// <param name="pm_Item">アイテムクローン</param> 
    /// <returns>なし</returns>
    private void ItemOptionSet(GameObject pm_Item)
    {
        pm_Item.GetComponent<RectTransform>().anchoredPosition = GB_Position;
        pm_Item.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
        SC_Item w_Item = pm_Item.GetComponent<SC_Item>();
        w_Item.BallDrop();
    }

    /// <summary> 
    /// 敵の攻撃
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void EnemyAttack()
    {
        //引数1:敵の位置
        //引数2:弾の発射数
        //引数3:弾のレベル 0:Lv1 1:Lv2 2:Lv3
        //引数4:弾の分散数(100度～260度の範囲)
        //   例:12を設定した場合、正12角形の頂点方向へ分散する
        switch (GB_SkillName)
        {
            case "FiveShotBallLv1":
                SC_EnemyAttack.WholeBall(GB_Position, 1, 0, 15);
                break;
            case "FiveShotBallLv2":
                SC_EnemyAttack.WholeBall(GB_Position, 3, 0, 15);
                break;
            case "FiveShotBallLv3":
                SC_EnemyAttack.WholeBall(GB_Position, 3, 1, 15);
                break;
            case "FiveShotBallLv4":
                SC_EnemyAttack.WholeBall(GB_Position, 1, 1, 18);
                break;
            case "FiveShotBallLv5":
                SC_EnemyAttack.WholeBall(GB_Position, 1, 2, 18);
                break;
            case "FiveShotBallLv6":
                SC_EnemyAttack.WholeBall(GB_Position, 1, 2, 24);
                break;
            case "RainBallLv1":
                SC_EnemyAttack.RainBall(GB_Position, 1, 1, 12);
                break;
            case "RainBallLv2":
                SC_EnemyAttack.RainBall(GB_Position, 1, 1, 18);
                break;
            case "RainBallLv3":
                SC_EnemyAttack.RainBall(GB_Position, 2, 1, 12);
                break;
            case "RainBallLv4":
                SC_EnemyAttack.RainBall(GB_Position, 2, 1, 18);
                break;
            case "RainBallLv5":
                SC_EnemyAttack.RainBall(GB_Position, 2, 2, 18);
                break;
            case "RainBallLv6":
                SC_EnemyAttack.RainBall(GB_Position, 3, 2, 24);
                break;
            case "HomingBallLv1":
                SC_EnemyAttack.HomingBall(GB_Position, 3, 0);
                break;
            case "HomingBallLv2":
                SC_EnemyAttack.HomingBall(GB_Position, 5, 0);
                break;
            case "HomingBallLv3":
                SC_EnemyAttack.HomingBall(GB_Position, 5, 1);
                break;
            case "HomingBallLv4":
                SC_EnemyAttack.HomingBall(GB_Position, 7, 1);
                break;
            case "HomingBallLv5":
                SC_EnemyAttack.HomingBall(GB_Position, 10, 2);
                break;
            case "HomingBallLv6":
                SC_EnemyAttack.HomingBall(GB_Position, 15, 2);
                break;
            default:
                break;
        }

    }
}