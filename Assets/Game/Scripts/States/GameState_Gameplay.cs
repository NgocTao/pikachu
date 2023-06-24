using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using static StormStudio.Common.GSMachine;
using UnityEngine;
using StormStudio.GameOps;

public enum GameMode
{
    Classic,
    SevenSlot,
}

public partial class GameFlow : MonoBehaviour
{
    [SerializeField] private Camera CameraGameplay;

    GameplayController _gameController;
    GameMode _mode;
    LevelData _currentLevelData;

    void GameState_Gameplay(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            _gameController = new GameObject("GameplayController", typeof(GameplayController)).GetComponent<GameplayController>();
            _gameController.Setup(CameraGameplay);
        }
        else if (stateEvent == StateEvent.Exit)
        {
            Destroy(_gameController.gameObject);
            _gameController = null;
        }
    }
}
