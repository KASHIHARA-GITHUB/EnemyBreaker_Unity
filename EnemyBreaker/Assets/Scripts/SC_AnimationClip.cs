using UnityEngine;

public class SC_AnimationClip : MonoBehaviour
{

    public void PrintEvent(string pm_AnimationClip)
    {
        //各ボタンの処理
        switch (pm_AnimationClip)
        {
            case "WarningFlag":
                //SC_GameManager.StageSelect(true);
                break;
            case "TitleFlag":
                //SC_GameManager.StageSelect(false);
                break;
            case "MyTurn":
                //SC_GameManager.MyTurn();
                break;
            case "EnemyTurn":
                //SC_GameManager.EnemyTurn();
                break;
            default:
                break;
        }
    }
}
