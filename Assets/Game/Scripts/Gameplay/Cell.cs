using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _spriteRederer;
    [SerializeField] public GameObject _goHighlight;

    public int ID { get { return _id; } }
    public Vector2Int Pos { get { return _pos; } }
    public bool Active { get { return _active; } }

    public int Row;
    public int Column;

    int _id;
    Vector2Int _pos = new Vector2Int();
    Color _spriteColor;
    bool _active = true;

    public void Setup(Sprite sprite, int id)
    {
        _spriteRederer.sprite = sprite;
        _id = id;
        EnableHighlight(false);
    }

    public void SetPos(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public void EnableHighlight(bool enabled)
    {
        _goHighlight.SetActive(enabled);
    }

    public void Enable(bool enabled)
    {
        _active = enabled;
        _spriteRederer.gameObject.SetActive(enabled);
    }

}

