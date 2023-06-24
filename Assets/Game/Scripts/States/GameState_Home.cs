using System.Collections;
using System.Collections.Generic;
using StormStudio.Common.UI;
using static StormStudio.Common.GSMachine;
using UnityEngine;
using StormStudio.GameOps;

public partial class GameFlow : MonoBehaviour
{
    HomeUI _homeUI;

    void GameState_Home(StateEvent stateEvent)
    {
        if (stateEvent == StateEvent.Enter)
        {
            SoundManager.Instance.PlayBgmHome();
            _homeUI = UIManager.Instance.ShowUIOnTop<HomeUI>("HomeUI");
            _homeUI.Setup(onStartGame);
        }
        else if (stateEvent == StateEvent.Exit)
        {
            UIManager.Instance.ReleaseUI(_homeUI, true);
        }
    }
    private void onStartGame()
    {
        SceneTransition(() => _gsMachine.ChangeState(GameState.Gameplay));
    }

    private void onSoundChanged(bool changed)
    {
        SoundManager.Instance.ToggleSfx();
    }

    private void onMusicChanged(bool changed)
    {
        SoundManager.Instance.ToggleBGM();
    }

    private void onVibrateChanged(bool changed)
    {
        SoundManager.Instance.ToggleVibrate();
    }
}
