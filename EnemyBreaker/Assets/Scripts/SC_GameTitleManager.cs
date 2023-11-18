using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_GameTitleManager: MonoBehaviour
{
    //オブジェクト
    [SerializeField] private GameObject OB_ProgressPanel = null;    //プログレスバー
    [SerializeField] private GameObject OB_GameSound = null;        //ゲームサウンド
    [SerializeField] private GameObject OB_GameButtonSE = null;     //ボタンSE

    //コンポーネント
    [SerializeField] private Rigidbody2D[] CP_BallRigid = null;         //ボールの角度
    [SerializeField] private Canvas CP_Canvas = null;               //キャンバス
    [SerializeField] private Image CP_Progress = null;              //プログレスバー
    [SerializeField] private Text CP_ProgressText = null;           //プログレスのパーセンテージテキスト

    //グローバル
    private float GB_SoundVolume;               //サウンド音量
    private static bool GB_AudioStart = true;   //スタート時だけ音楽を流す

    /// <summary> 
    /// ゲームタイトル画面初期
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void Start()
    {
        //ゲーム画面に表示するボールを動かす
        BallCreate();

        //プログレスバー非表示
        ProgressInit();

        //ゲームサウンドセット
        GameSoundOption();
    }

    /// <summary> 
    /// プログレスバー非表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void ProgressInit()
    {
        //プログレス画面非表示
        OB_ProgressPanel.SetActive(false);
    }

    /// <summary> 
    /// プログレスバー表示
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    public void Loading()
    {
        //プログレス画面非表示
        OB_ProgressPanel.SetActive(true);
        StartCoroutine("LoadData");
    }

    /// <summary> 
    /// プログレスバー進捗処理と音楽フェードアウト
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private IEnumerator LoadData()
    {
        AsyncOperation w_Async;   //非同期処理
        float w_ProgressVal;      //プログレスバー値

        GB_AudioStart = true;

        OB_GameSound = GameObject.Find("GameTitleSound");
        OB_GameButtonSE = GameObject.Find("GameButtonSE");

        //音楽をフェードアウト
        GB_SoundVolume = 1.0f;
        while (GB_SoundVolume > 0.0f)
        {
            OB_GameSound.GetComponent<AudioSource>().volume = GB_SoundVolume;
            GB_SoundVolume = GB_SoundVolume - 0.05f;
            //await Task.Delay(50);
            yield return new WaitForSeconds(0.05f);
        }
        
        SceneManager.MoveGameObjectToScene(OB_GameSound, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(OB_GameButtonSE, SceneManager.GetActiveScene());

        w_Async = SceneManager.LoadSceneAsync("GameScene");

        //　読み込みが終わるまで進捗状況をプログレスバーに反映させる
        while (!w_Async.isDone)
        {
            w_ProgressVal = Mathf.Clamp01(w_Async.progress / 0.9f);
            CP_ProgressText.text = $"{w_ProgressVal * 100} %";
            CP_Progress.fillAmount = w_ProgressVal;
            yield return null;
        }
    }

    /// <summary> 
    /// 画面上にボールを動かす
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void BallCreate()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 w_Direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            CP_BallRigid[i].velocity = w_Direction * 600 * CP_Canvas.transform.localScale.x;
        }
    }

    /// <summary> 
    /// ゲーム開始曲を流す
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void GameSoundOption()
    {
        //一度だけ処理を行う
        if (GB_AudioStart)
        {
            // Sceneを遷移してもオブジェクトが消えないようにする
            OB_GameSound.GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(OB_GameButtonSE);
            DontDestroyOnLoad(OB_GameSound);

            GB_AudioStart = false;
        }
    }
}
