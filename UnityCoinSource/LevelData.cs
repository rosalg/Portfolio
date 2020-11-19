using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelData
{
    public IndivLevelData[] LD;

    public LevelData(GameManager gm)
    {
        LD = GameManager.instance.IndividualLevelData;
    }

}
