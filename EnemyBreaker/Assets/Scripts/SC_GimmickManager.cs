using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GimmickObj
{
    public enum GimmickMaster
    {
        OB_Block,
        OB_Warp,
        OB_WarpCounter,
        OB_WarpDist,
        OB_DeathBlock
    }
    public GimmickMaster name;
    public GameObject GimmickObject;
}

[System.Serializable]
public class GimmickCount
{
    public enum GimmickMaster
    {
        OB_Block,
        OB_Warp,
        OB_WarpCounter,
        OB_WarpDist,
        OB_DeathBlock
    }
    public GimmickMaster name;
    public Vector2 position;
}

[System.Serializable]
public class GimmickArea
{
    public List<GimmickCount> GB_GimmickCount;
}

[System.Serializable]
public class GimmickStage
{
    public List<GimmickArea> GB_GimmickArea;
}

[System.Serializable]
public class WarpDist
{
    public int stageNumber;
    public int AreaNumber;
    public Vector2[] position = new Vector2[3];
}

//ドロップアイテムスクリプト
public class SC_GimmickManager: MonoBehaviour
{
    public List<GimmickObj>   GB_GimmickObject;     //ギミックオブジェクト
    public List<GimmickStage> GB_GimmickStage;      //ステージに設置するギミック
    public List<WarpDist>     GB_WarpDist;          //宛先ワープ

    //オブジェクト
    [SerializeField] private GameObject OB_GimmicksClone = null;           //ギミッククローン

    //コンポーネント変数
    [SerializeField] private Transform CP_GimmicksClone = null;            //ボールエリア

    //グローバル変数
    private bool GB_WarpDistSet;                    //宛先ワープセットフラグ(true:オブジェクトセット可能 false:オブジェクトセット不可)
    private const int GB_WarpDistSetCount = 3;      //宛先ワープセット可能数

    /// <summary> 
    /// ギミックの設定
    /// </summary> 
    /// <param name="pm_StageCount">ステージ番号</param> 
    /// <param name="pm_AreaCount">エリア番号</param>
    /// <returns>なし</returns>
    public int Option(int pm_StageCount, int pm_AreaCount)
    {
        GB_WarpDistSet = true;  //宛先ワープフラグ

        //エリアのギミックを削除
        foreach (Transform childTransform in OB_GimmicksClone.transform)
        {
            Destroy(childTransform.gameObject);
        }

        //ギミックを設置しない場合呼び出し先に戻る
        if (GB_GimmickStage[pm_StageCount].GB_GimmickArea.Count() <= pm_AreaCount) { return 0; }
        if (GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount.Count() == 0) { return 0; }

        //エリアにギミックをセット
        for (int x = 0; x < GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount.Count(); x++)
        {
            for (int y = 0; y < GB_GimmickObject.Count(); y++)
            {
                //対象のオブジェクトを検索する
                if (GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount[x].name.ToString() == GB_GimmickObject[y].name.ToString())
                {
                    GameObject w_Gimmick = null;

                    w_Gimmick = Instantiate(GB_GimmickObject[y].GimmickObject, CP_GimmicksClone) as GameObject;
                    w_Gimmick.GetComponent<RectTransform>().anchoredPosition = GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount[x].position;

                    //ワープギミックの場合は下記の処理を行う
                    if (GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount[x].name.ToString() == "OB_Warp" ||
                        GB_GimmickStage[pm_StageCount].GB_GimmickArea[pm_AreaCount].GB_GimmickCount[x].name.ToString() == "OB_WarpCounter")
                    {
                        Vector2[] w_GimmickVector = WarpDistSet(pm_StageCount, pm_AreaCount);

                        SC_Warp w_GimmickWarp = w_Gimmick.GetComponent<SC_Warp>();
                        w_GimmickWarp.PositionSet(w_GimmickVector);

                    }

                    break;
                }
            }
        }

        return 0;
    }

    /// <summary> 
    /// 宛先ワープの位置をセット
    /// </summary> 
    /// <param name="message">なし</param> 
    /// <returns>なし</returns>
    private Vector2[] WarpDistSet(int pm_StageCount, int pm_AreaCount)
    {
        Vector2[] w_GimmickVector;              // 宛先ワープの位置
        w_GimmickVector = new Vector2[3];       // 宛先ワープの位置格納配列数
        GameObject w_GimmickWarpDist = null;    // 宛先ワープのオブジェクト

        //宛先ワープの位置を配列に格納
        for(int x = 0; x < GB_WarpDist.Count(); x++)
        {
            if (pm_StageCount == GB_WarpDist[x].stageNumber && pm_AreaCount == GB_WarpDist[x].AreaNumber)
            {
                for (int y = 0; y < GB_WarpDistSetCount; y++)
                {
                    w_GimmickVector[y] = GB_WarpDist[x].position[y];  
                }

                break;
            }
        }

        //宛先ワープオブジェクトの位置をセット
        if (GB_WarpDistSet == true)
        {
            GB_WarpDistSet = false;

            for (int y = 0; y < GB_WarpDistSetCount; y++)
            {
                w_GimmickWarpDist = Instantiate(GB_GimmickObject[3].GimmickObject, CP_GimmicksClone) as GameObject;
                w_GimmickWarpDist.GetComponent<RectTransform>().anchoredPosition = w_GimmickVector[y];
            }
        }

        return w_GimmickVector;
    }
}
