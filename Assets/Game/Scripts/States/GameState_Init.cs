using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using static StormStudio.Common.GSMachine;
using UnityEngine;
using StormStudio.GameOps;
using UnityEngine.Networking;

public partial class GameFlow : MonoBehaviour
{
    System.DateTime _currentDateTime = System.DateTime.UtcNow;
    public double DeltaTotalSeconds = 0;
    public bool LoadedCurrentDateTime = false;

    [System.Serializable]
    public class UnixTimestampData
    {
        public string UnixTimeStamp;
    }

    void GameState_Init(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            StartCoroutine(coroutineInit());
        }
        else if (stateEvent == StateEvent.Exit)
        {
        }
    }

    IEnumerator coroutineInit()
    {
        yield return null;
        SceneTransition(() => _gsMachine.ChangeState(GameState.Home));
    }
}
