using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelData", menuName = "Pikachu/ModelData", order = 1)]

[System.Serializable]
public class ModelData : ScriptableObject
{
    public List<CellData> Cells = new List<CellData>();
}

[System.Serializable]
public class CellData
{
    public int ID;
    public int Row;
    public int Column;
    public Sprite Sprite;
}
