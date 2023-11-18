using UnityEngine;

public class SC_GameSE : MonoBehaviour
{
    [SerializeField] private AudioSource GameSound = null;            //効果音
    [SerializeField] private AudioClip GB_BlockHitSound = null;       //敵当たる音
    [SerializeField] private AudioClip GB_BlockBreakSound = null;     //敵壊れる音
    [SerializeField] private AudioClip GB_ItemGetSound = null;        //アイテム取得音
    [SerializeField] private AudioClip GB_WarningSound = null;        //警告音

    /// <summary> 
    /// 敵にあたる音
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void BlockHitSound()
    {
        GameSound.PlayOneShot(GB_BlockHitSound);
    }

    /// <summary> 
    /// 敵が壊れる音
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void BlockBreakSound()
    {
        GameSound.PlayOneShot(GB_BlockBreakSound);
    }

    /// <summary> 
    /// アイテム取得音
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void ItemGetSound()
    {
        GameSound.PlayOneShot(GB_ItemGetSound);
    }

    /// <summary> 
    /// 警告音
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    public void WarningSound()
    {
        GameSound.PlayOneShot(GB_WarningSound);
    }
}
