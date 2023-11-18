using System.Collections;
using UnityEngine;

//ゲームサウンドスクリプト
public class SC_GameSound : MonoBehaviour
{
    [SerializeField] private AudioSource GameSound = null;    //サウンド
    [SerializeField] private AudioClip[] GB_AreaBGM = null;   //敵BGM
    [SerializeField] private AudioClip[] GB_BossBGM = null;   //ボスBGM

    private float GB_SoundVolume;    //サウンド音量

    /// <summary> 
    /// ステージの曲選択
    /// </summary> 
    /// <param name="pm_StageNumber">ステージ番号</param> 
    /// <param name="pm_BossStage">ボスの有無(true:ボス false:通常)</param> 
    /// <returns>なし</returns>
    public void StageSound(int pm_StageNumber, bool pm_BossStage)
    {
        if (pm_BossStage == false)
        {
            GameSound.clip = GB_AreaBGM[pm_StageNumber];
        }
        else
        {
            GameSound.clip = GB_BossBGM[pm_StageNumber];
        }

        GameSound.Play();
    }

    /// <summary> 
    /// BGMフェードアウト(同期)
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    //public async Task SoundFadeOutSyn()
    public void SoundFadeOutSyn()
    {
        StartCoroutine("SoundFadeOutSynCoroutine");
    }

    private IEnumerator SoundFadeOutSynCoroutine()
    {
        GB_SoundVolume = 1.0f;

        while(GB_SoundVolume > 0.0f)
        {
            GameSound.volume = GB_SoundVolume;
            GB_SoundVolume = GB_SoundVolume - 0.01f;
            //await Task.Delay(50);
            yield return new WaitForSeconds(0.05f);
        }
        GameSound.Stop();
        GameSound.volume = 1.0f;
    }

    /// <summary> 
    /// BGMフェードアウト(非同期)
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    //public async void SoundFadeOutAsyn()
    public void SoundFadeOutAsyn()
    {
        StartCoroutine("SoundFadeOutAsynCoroutine");
    }

    private IEnumerator SoundFadeOutAsynCoroutine()
    {
        GB_SoundVolume = 1.0f;

        while (GB_SoundVolume > 0.0f)
        {
            GameSound.volume = GB_SoundVolume;
            GB_SoundVolume = GB_SoundVolume - 0.01f;
            //await Task.Delay(1);
            yield return new WaitForSeconds(0.001f);
        }
        GameSound.Stop();
        GameSound.volume = 1.0f;
    }
}
