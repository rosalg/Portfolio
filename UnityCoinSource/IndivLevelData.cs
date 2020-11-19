using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IndivLevelData 
{
    [HideInInspector]
    public string LevelName { get; set; }
    [HideInInspector]
    public bool IsUnlocked { get; set; }

    public IndivLevelData(bool p_IsUnlocked, string p_LevelName)
    {
        LevelName = p_LevelName;
        IsUnlocked = p_IsUnlocked;
    }

}
