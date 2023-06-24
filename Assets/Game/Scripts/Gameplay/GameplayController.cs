using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormStudio.Common.UI;
using DG.Tweening;

public interface IGameController
{
    Camera CameraGameplay { get; }
    bool IsOver { get; }
    bool IsPause { get; }

    void OnClickCell(Cell cellSelected);
}

public class GameplayController : MonoBehaviour, IGameController
{
    public Camera CameraGameplay { get { return _camera; } }
    public bool IsOver { get { return _isOver; } }
    public bool IsPause { get { return _isPause; } }
    Camera _camera;
    bool _isOver = false;
    bool _isPause = false;
    GridSystem _gridSystem;
    PlayerController _playerController;
    ShortestPathFinder _pathFinder;
    PlayUI _playUI;

    List<CellData> _cellSelected = new List<CellData>();

    public void Setup(Camera camera)
    {
        _camera = camera;

        //Create Grid
        var board = Instantiate(Resources.Load<GameObject>("Gameplay/Grid"), transform);
        _gridSystem = board.GetComponent<GridSystem>();
        _playerController = board.GetComponentInChildren<PlayerController>();
        _playerController.Setup(this);

        // UI Controlers
        // _playUI = UIManager.Instance.ShowUIOnTop<PlayUI>("PlayUI");
        // _playUI.Setup();

        //Setup Grid
        _gridSystem.Setup();
        _pathFinder = new ShortestPathFinder();
    }

    public void CheckMatching(List<Cell> cells)
    {
        _gridSystem.EnableCell(cells);
    }

    public void OnClickCell(Cell cellSelect)
    {
        cellSelect.EnableHighlight(true);
        _cellSelected.Add(_gridSystem.GetCellData(cellSelect));
        if (_cellSelected.Count >= 2)
        {
            StartCoroutine(ProcessCheckCell());
        }
    }

    IEnumerator ProcessCheckCell()
    {
        _pathFinder.GenerateGrid(_gridSystem.CellDatas);
        var paths = _pathFinder.FindShortestPath(_cellSelected[0].Row, _cellSelected[0].Column, _cellSelected[1].Row, _cellSelected[1].Column);
        yield return null;
    }
}
