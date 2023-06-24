using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellConfig
{
    public int ID;
    public Sprite Sprite;
}

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    [SerializeField] List<CellConfig> _themeA;

    Dictionary<int, Sprite> _cellSprites = new Dictionary<int, Sprite>();

    void Awake()
    {
        Instance = this;
        // Load Theme
        LoadTheme(true);
    }

    void LoadTheme(bool isA)
    {
        _cellSprites.Clear();
        foreach (var cell in _themeA)
        {
            _cellSprites.Add(cell.ID, cell.Sprite);
        }
    }

    public Sprite GetSpriteByID(int id)
    {
        return _cellSprites[id];
    }
}
