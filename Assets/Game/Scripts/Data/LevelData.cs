using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[System.Serializable]
public class GroupData
{
    public string Group;
    public float Percent;
}

[System.Serializable]
public class LevelConfigData
{
    public int Level;
    public List<GroupData> Groups;
    public float PercentNew;
    public float PercentDelta;
}

[System.Serializable]
public class LevelGenerateData
{
    public int Level;
    public float PercentNew;
    public float PercentDelta;
    public int[] Range;
    public int[] Duration;
}

[System.Serializable]
public class LevelData
{
    public int Index;
    public List<string> Board;
    public int Duration;

    public LevelData()
    {
        Index = 0;
        Board = new List<string>();
        Duration = 60;
    }
}