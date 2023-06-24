using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class C
{
    public enum BtnType
    {
        None = 0,
        Resume = 1,
        Home = 2,
        Howtoplay = 3,
        Restart = 4,
        Play = 5
    }

    public static class DragConfig
    {
        public static float MultiDragObject = 8f;
        public static float RadiusTap = 25f;
        public static float DeltaTimeTap = 0.3f;
        public static float SpeedTouch = 0.5f;
        public static float DragHeight = 3.0f;
        public static float HeightInHolder = 1.5f;
    }

    public static class AnimationConfig
    {
        public static float TimeAddObject = 0.3f;
        public static float TimeMatchObject = 0.3f;
        public static float SpeedMoving = 1f;
        public static float ScaleTouched = 1.15f;
    }

    public static class GameLayer
    {
        public static string LayerIdleObject = "IdleObject";
        public static string LayerDragObject = "DragObject";
        public static string LayerHolder = "Holder";
        public static string LayerGround = "Ground";
    }

    public static class GameConfig
    {
        public static int MaximumMission = 2;
        public static float BonusTime = 60f;
        public static int CountMatch = 3;
        public static float DurationCombo = 5f;
        public static float StarPerSecond = 10f;
        public static int DefaultTimePerObject = 3;
    }

    public static class PriceConfigs
    {
        public static int PricePerRevive = 800;
    }

    public static class RewardConfigs
    {
        public static int FreeCoin = 100;
        public static int CoinPerWatchAd = 100;
        public static int PiggyCoinPerMatch = 500;
        public static int MaximumCoinInBank = 6000;
        public static int CoinCompleteTutorial = 500;
        public static int CoinCompleteLevel = 50;
    }

    public static class AdsConfig
    {
        public static string PLACEMENT_INTERSTITIAL_PAUSE = "intersitial_pause";
        public static string PLACEMENT_INTERSTITIAL_LEVEL_COMPLETE = "intersitial_level_complete";
        public static string PLACEMENT_INTERSTITIAL_GAME_OVER = "intersitial_game_over";
        public static string PLACEMENT_REWARDED_SHOP = "rewarded_shop";
        public static string PLACEMENT_REWARDED_BOOSTER = "rewarded_booster";
        public static string PLACEMENT_REWARDED_LEVEL_COMPLETE = "rewarded_level_complete";
        public static string PLACEMENT_REWARDED_GAME_OVER = "rewarded_game_over";

        public static int LevelShowAd = 2;
        public static int AdsGameCompleteCondition = 2;
        public static int AdsGameOverCondition = 5;
    }

    public static class TimeConfig
    {
        public static int HourPerRewardFree = 24;
        public static int SecondForHint = 5;
        public static int TimeCheckAfterComplete = 5;
    }

    public static class GridConfig
    {
        public static int Column = 12;
        public static int Row = 7;
        public static float SpaceX = 0.35f;
        public static float SpaceY = 0.45f;
    }

}
