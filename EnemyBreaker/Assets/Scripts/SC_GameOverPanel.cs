using UnityEngine;
public class SC_GameOverPanel : MonoBehaviour
{
    //関数名
    //Initialize:初期化
    //引数名
    //なし
    public void Init()
    {
        //パネルを非表示
        gameObject.SetActive(false);
    }

    //関数名
    //Display:初期化関数
    //引数名
    //なし
    public void Display(bool pm_PanelDisplay)
    {
        //パネルを表示
        gameObject.SetActive(pm_PanelDisplay);
    }
}
