using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StormStudio.Common;
using StormStudio.Common.UI;
using StormStudio.Common.Utils;
using StormStudio.GameOps;

public partial class GameFlow : MonoBehaviour
{
    public enum GameState
    {
        Init,
        Home,
        Gameplay,
        Tutorial
    }

    public enum LeaderboardID
    {
        None = -1,
        Classic = 0,
        TimeChallenge = 1
    }

    public static GameFlow Instance { get; private set; }

    public bool IsTimeChallengeModeActive { get; private set; }

    //[SerializeField] RawImage _overlayImage;

    private GSMachine _gsMachine = new GSMachine();
    private StormStudioApp _stormApp;
    // private NativeLeaderboardManager _leaderboardManager;
    
    string[] _leaderboardIDs;

    public string[] LeaderboardIDs { get { return _leaderboardIDs; } }

    bool _remoteConfigsAreUpdated;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupStormApp();

#if !BUILD_DEV || DISABLE_LOGS
        Debug.unityLogger.logEnabled = false;
#endif
        Input.multiTouchEnabled = false;

#if UNITY_STANDALONE && !UNITY_EDITOR
        Screen.SetResolution(720, 1280, false);
#endif

        if (Application.isEditor)
            Application.runInBackground = true;

        Application.targetFrameRate = 60;
    }

    void SetupStormApp()
    {
        _stormApp = new GameObject("StormApp", typeof(StormStudioApp)).GetComponent<StormStudioApp>();
        Object.DontDestroyOnLoad(_stormApp.gameObject);
    }

    public void ApplyFetchedConfigs()
    {
        _remoteConfigsAreUpdated = true;
    }

    public int[] ParseStringToIntArray(string stringToParse, char charSplit)
    {
        int[] array;
        var arrayString = stringToParse.Split(charSplit);
        array = new int[arrayString.Length];
        for (int i = 0; i < arrayString.Length; i++)
            array[i] = int.Parse(arrayString[i]);
        return array;
    }

    IEnumerator Start()
    {
        HideLoading();
        LoadConfigs();
        
        // LoadRatingData();
        SoundManager.Instance.LoadSoundSettings();
        // LoadProfile();
        // LoadNotification();

        // Start game state machine
        _gsMachine.Init(OnStateChanged, GameState.Gameplay);
        SoundManager.Instance.OnEnableMusic += onEnableMusic;
        while (true)
        {
            _gsMachine.StateUpdate();
            yield return null;
        }
    }

    private void onEnableMusic(bool enabled)
    {
        if (enabled)
        {
            switch((GameState) _gsMachine.CurrentState)
            {
                case GameState.Home:
                    SoundManager.Instance.PlayBgmHome();
                    break;
                case GameState.Gameplay:
                    SoundManager.Instance.PlayBgmGameplay();
                    break;
            }
        }
    }

    public void SubscribeAppPause(AppPaused listener)
    {
        _stormApp.SubscribeAppPause(listener);
    }

    public void UnSubscribeAppPause(AppPaused listener)
    {
        _stormApp.UnSubscribeAppPause(listener);
    }

    // void OnDestroy()
    // {
    //     Instance = null;
    // }

    void ShowLoading()
    {
        
    }

    void HideLoading()
    {
        
    }

    public T ShowUI<T>(string uiPath, bool overlay = false) where T : UIController
    {
        return UIManager.Instance.ShowUIOnTop<T>(uiPath, overlay);
    }

    public void SceneTransition(System.Action onSceneOutFinished)
    {
        UIManager.Instance.SetUIInteractable(false);
        SceneDirector.Instance.Transition(new TransitionFade()
        {
            duration = 0.667f,
            tweenIn = TweenFunc.TweenType.Sine_EaseInOut,
            tweenOut = TweenFunc.TweenType.Sine_EaseOut,
            onStepOutDidFinish = () =>
            {
                onSceneOutFinished.Invoke();
            },
            onStepInDidFinish = () =>
            {
                UIManager.Instance.SetUIInteractable(true);
            }
        });
    }

    #region GSMachine
    GSMachine.UpdateStateDelegate OnStateChanged(System.Enum state)
    {
        switch (state)
        {
            case GameState.Init:
                return GameState_Init;
            case GameState.Home:
                return GameState_Home;
            case GameState.Gameplay:
                return GameState_Gameplay;
        }

        return null;
    }
    #endregion
}