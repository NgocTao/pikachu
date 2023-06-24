using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] Vector2Int _boardSize;
    [SerializeField] Cell _prefabCell;

    [Header("Grid")]
    [SerializeField] Vector2 _tileSize;
    [SerializeField] Vector2 _space;
    [SerializeField] Transform _prefabTile;

    [SerializeField] ModelData _modeData;
    PlayUI _playUI;
    List<Cell> _cells = new List<Cell>();
    List<Vector2Int> _allPositions = new List<Vector2Int>();

    public CellData[,] CellDatas => _dataCells;

    // New
    Dictionary<int, Transform> _mapTiles = new Dictionary<int, Transform>();
    CellData[,] _dataCells;
    ObjectPool<Transform> _poolTiles;
    ObjectPool<Cell> _poolCells;

    void Awake()
    {
        _poolTiles = new ObjectPool<Transform>(_prefabTile);
        _poolCells = new ObjectPool<Cell>(_prefabCell);
    }

    public void Setup()
    {
        // GenerateGrid();
        // LoadLevel();
        CreateTiles();
        GenerateGame(2);
    }

    public void CreateTiles()
    {
        var width = _tileSize.x * _boardSize.x + (_boardSize.x - 1) * _space.x;
        var height = _tileSize.y * _boardSize.y + (_boardSize.y - 1) * _space.y;

        _mapTiles.Clear();
        for (int r = 0; r < _boardSize.y; r++)
        {
            for (int c = 0; c < _boardSize.x; c++)
            {
                var tile = _poolTiles.Get();
                tile.SetParent(_prefabTile.parent);
                tile.transform.position = new Vector3(-width * 0.5f + _tileSize.x * 0.5f + c * (_tileSize.x + _space.x), -height * 0.5f + _tileSize.y * 0.5f + r * (_tileSize.y + _space.y), 0f);
                _mapTiles.Add(r * _boardSize.x + c, tile);
            }
        }

    }

    public void GenerateGame(int countMatch)
    {
        int totalPair = Mathf.RoundToInt((_boardSize.x - 2) * (_boardSize.y - 2) * 0.5f);

        List<Vector2Int> availableSlots = new List<Vector2Int>();
        for (int r = 1; r < _boardSize.y - 1; r++)
        {
            for (int c = 1; c < _boardSize.x - 1; c++)
            {
                availableSlots.Add(new Vector2Int(r, c));
            }
        }

        _dataCells = new CellData[_boardSize.y, _boardSize.x];
        for (int i = 0; i < totalPair; i++)
        {
            int randId = Random.Range(0, 20);
            for (int num = 0; num < countMatch; num++)
            {
                var newCell = new CellData { ID = randId };
                var indSlot = Random.Range(0, availableSlots.Count);
                newCell.Column = availableSlots[indSlot].y;
                newCell.Row = availableSlots[indSlot].x;
                availableSlots.RemoveAt(indSlot);
                _dataCells[newCell.Row, newCell.Column] = newCell;
            }
        }

        foreach (var cellData in _dataCells)
        {
            if (cellData == null) continue;
            var cell = _poolCells.Get();
            cell.transform.SetParent(_prefabCell.transform.parent);
            cell.Setup(ThemeManager.Instance.GetSpriteByID(cellData.ID), cellData.ID);
            cell.SetPos(cellData.Row, cellData.Column);
            cell.transform.position = _mapTiles[cellData.Row * _boardSize.x + cellData.Column].position;
        }
    }

    // public void GenerateGrid()
    // {
    //     for (int r = 0; r < C.GridConfig.Row; r++)
    //     {
    //         for (int c = 0; c < C.GridConfig.Column; c++)
    //         {
    //             GameObject gridCell = Instantiate(_prefabTile, transform);
    //             var cell = gridCell.GetComponentInChildren<Cell>();
    //             cell.SetPosition(r, c);
    //             _cells.Add(cell);
    //             _allPositions.Add(new Vector2Int(r, c));
    //             gridCell.transform.position = new Vector2(c + C.GridConfig.SpaceY * c, r + C.GridConfig.SpaceX * r);

    //         }
    //     }
    //     this.gameObject.transform.position = new Vector2(-(C.GridConfig.Column - 1) * 1.45f / 2, -(C.GridConfig.Row - 1) * 1.35f / 2);
    // }

    public void EnableCell(List<Cell> cellsMatching)
    {
        List<Cell> moveCells = new List<Cell>();
        foreach (var temp in cellsMatching)
        {
            foreach (var cell in _cells)
            {
                if (cell.Pos == temp.Pos)
                {
                    cell.EnableHighlight(false);
                    // cell.Enable(false);
                }
            }
        }
        foreach (var temp in cellsMatching)
        {
            foreach (var cell in _cells)
            {
                if (cell.Pos.y == temp.Pos.y && cell.Pos.x >= temp.Pos.x && cell.Active)
                {
                    moveCells.Add(cell);
                }
            }
            foreach (var cell in moveCells)
            {
                Debug.Log("moveCells: " + cell.Pos + " ID " + cell.ID);
            }
            ArrangeGrid(moveCells);
            moveCells.Clear();
        }



    }

    public void ArrangeGrid(List<Cell> moveCells)
    {
        for (int i = 0; i < moveCells.Count; i++)
        {
            if (i == moveCells.Count - 1)
            {
                moveCells[i].Enable(false);
                return;
            }
            moveCells[i].Setup(moveCells[i + 1]._spriteRederer.sprite, moveCells[i + 1].ID);
        }
        foreach (var cell in moveCells)
        {
            Debug.Log("Reload " + cell.Pos + " ID " + cell.ID);
        }
    }

    public void LoadLevel()
    {
        while (_allPositions.Count > 0)
        {
            var pos1 = RandomlySelectPositions();
            var pos2 = RandomlySelectPositions();
            var indexItem = Random.Range(0, 19);
            foreach (var cell in _cells)
            {
                if (cell.Pos == pos1 || cell.Pos == pos2)
                    cell.Setup(_modeData.Cells[indexItem].Sprite, _modeData.Cells[indexItem].ID);
            }
        }
    }

    public Vector2Int RandomlySelectPositions()
    {
        var index = Random.Range(0, _allPositions.Count);
        var pos = new Vector2Int(_allPositions[index].x, _allPositions[index].y);
        _allPositions.Remove(_allPositions[index]);
        return pos;
    }

    public CellData GetCellData(Cell cellSelect)
    {
        return _dataCells[cellSelect.Row, cellSelect.Column];
    }
}
