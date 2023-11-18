using UnityEngine;

public class SC_Manual: MonoBehaviour
{
    //オブジェクト
    [SerializeField] private GameObject OB_Manual1 = null;         //マニュアル画面1
    [SerializeField] private GameObject OB_Manual2 = null;         //マニュアル画面2
    [SerializeField] private GameObject OB_BackButton = null;      //戻るボタン
    [SerializeField] private GameObject OB_NextButton = null;      //次へボタン 

    /// <summary> 
    /// マニュアル画面初期
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns> 
    private void Start()
    {
        OB_Manual1.SetActive(true);
        OB_Manual2.SetActive(false);
        OB_BackButton.SetActive(false);
        OB_NextButton.SetActive(true);
    }

    /// <summary> 
    /// マニュアル画面の更新
    /// </summary> 
    /// <param name="pm_Page">ページ表示</param> 
    /// <returns>なし</returns>
    public void PageUpdate(bool pm_Page)
    {
        OB_Manual1.SetActive(pm_Page);
        OB_Manual2.SetActive(!pm_Page);
        OB_BackButton.SetActive(!pm_Page);
        OB_NextButton.SetActive(pm_Page);
    }
}
