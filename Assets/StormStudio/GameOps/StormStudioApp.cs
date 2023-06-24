using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormStudio.GameOps
{
    public interface IFullscreenAdDelegate
    {
        bool ShouldMuteVideoAd();
        void FullscreenAdSetAppInputActive(bool active);
    }

    public class StormStudioApp : MonoBehaviour, IMainAppService
    {

        public event AppPaused OnAppPaused;
        public event System.Action OnAppQuit;
        public event AppPerfChanged OnAppPerfChanged;

        public static StormStudioApp Instance
        {
            get;
            private set;
        }

        // FPS
        bool _enableFPSCounter;
        float _FPSUpdateInterval;
        float _FPSAccumulate;
        int _totalFrames;
        float _refreshFPSTimeleft;
        bool _appInLowPerformance;

        
        public AppConfigs AppConfigs { get; private set; }
        public bool IsAppPaused { get; private set; }

        public void SubscribeAppPause(AppPaused listener)
        {
            OnAppPaused += listener;
        }

        public void UnSubscribeAppPause(AppPaused listener)
        {
            OnAppPaused -= listener;
        }

        public void SubscribeAppQuit(System.Action listener)
        {
            OnAppQuit += listener;
        }

        public void UnSubscribeAppQuit(System.Action listener)
        {
            OnAppQuit -= listener;
        }

        public void SubscribeAppPerfChanged(AppPerfChanged listener)
        {
            OnAppPerfChanged += listener;
        }

        public void UnSubscribeAppPerfChanged(AppPerfChanged listener)
        {
            OnAppPerfChanged -= listener;
        }

        public void SubscribeAppDateChanged(System.Action onDateChanged)
        {

        }

        public void UnSubscribeAppDateChanged(System.Action onDateChanged)
        {

        }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("StormStudio instance is already created!");
                return;
            }
            Instance = this;
            Initialize();
        }

        void Update()
        {
        }

        void OnDestroy()
        {
            Instance = null;
        }

        void ProcessAppPerfChanged(bool lowPerf)
        {
            OnAppPerfChanged?.Invoke(lowPerf);
        }

        public void RunOnMainThread(System.Action action)
        {
#if UNITY_ANDROID
            // System.Threading.Tasks.Task.Run(() => { }).ContinueWithOnMainThread(task =>
            // {
            //     action();
            // });
#else
            action();
#endif
        }

        void Initialize()
        {
            // Load app configs
            var textAssets = Resources.Load<TextAsset>("app_configs");
            AppConfigs = new AppConfigs();
            AppConfigs.Load(textAssets.bytes);

            // FPS
            _enableFPSCounter = AppConfigs.GetValue("enable_fps_counter", "General").BooleanValue;
            if (_enableFPSCounter)
            {
                _FPSUpdateInterval = (float)AppConfigs.GetValue("fps_update_interval", "General").DoubleValue;
                _refreshFPSTimeleft = _FPSUpdateInterval;
            }
        }

        
        #region Interrupt
        void ProcessAppPaused(bool pauseStatus)
        {
            OnAppPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus != IsAppPaused)
                ProcessAppPaused(pauseStatus);

            IsAppPaused = pauseStatus;
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus == IsAppPaused)
                ProcessAppPaused(!focusStatus);

            IsAppPaused = !focusStatus;
        }

        private void OnApplicationQuit()
        {
            OnAppQuit?.Invoke();
        }
        #endregion
    }
}
