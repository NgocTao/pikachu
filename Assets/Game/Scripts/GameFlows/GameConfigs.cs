using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ResourceConfig
{
    public ResourceID ID;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public int LevelUnlock;
    public int DefaultValue;
    public int BasePrice;
}

[System.Serializable]
public class IAPSpriteConfig
{
    public string ID;
    public Sprite Panel;
    public Sprite Icon;
    public Sprite Tag;
}

public partial class GameFlow : MonoBehaviour
{
    const string KEY_BOOSTER_PRICE = "PRICE_{0}";
    const string KEY_REVIVE_PRICE = "PRICE_REVIVE";
    const string KEY_COUNT_BOOSTER_REWARD = "COUNT_BOOSTER_REWARD_{0}";
    const string KEY_REWARD_WATCH_AD = "REWARD_WATCH_AD";
    const string KEY_REWARD_FREE = "REWARD_FREE";
    const string KEY_REWARD_LEVEL_COMPLETE = "REWARD_LEVEL_COMPLETE";

    [Header("Resource Configs")]
    [SerializeField] List<ResourceConfig> _resourceConfigs;
    [SerializeField] public List<IAPSpriteConfig> _iapSpriteConfigs;

    [Header("Level Configs")]
    [SerializeField] private TextAsset[] _assetLevels;

    public Dictionary<ResourceID, ResourceConfig> ResourceConfigs = new Dictionary<ResourceID, ResourceConfig>();

    public void LoadConfigs()
    {
        foreach (var config in _resourceConfigs)
        {
            if (!ResourceConfigs.ContainsKey(config.ID))
                ResourceConfigs.Add(config.ID, config);
        }
    }

    // Set/Get Coin Buy Booster
    public void SetBoosterPrice(ResourceID type, int price)
    {
        var key = string.Format(KEY_BOOSTER_PRICE, type.ToString().ToUpper());
        PlayerPrefs.SetInt(key, price);
        PlayerPrefs.Save();
    }

    public int GetBoosterPrice(ResourceID type)
    {
        var key = string.Format(KEY_BOOSTER_PRICE, type.ToString().ToUpper());
        return PlayerPrefs.GetInt(key, ResourceConfigs[type].BasePrice);
    }

    // Set/Get Amount Reward Ad
    public void SetRevivePrice(int price)
    {
        PlayerPrefs.SetInt(KEY_REVIVE_PRICE, price);
        PlayerPrefs.Save();
    }

    public int GetRevivePrice()
    {
        return PlayerPrefs.GetInt(KEY_REVIVE_PRICE, C.PriceConfigs.PricePerRevive);
    }


    // Set/Get Amount Reward Ad
    public void SetRewardWatchAd(int amount)
    {
        PlayerPrefs.SetInt(KEY_REWARD_WATCH_AD, amount);
        PlayerPrefs.Save();
    }

    public int GetRewardWatchAd()
    {
        return PlayerPrefs.GetInt(KEY_REWARD_WATCH_AD, C.RewardConfigs.CoinPerWatchAd);
    }

    // Set/Get Amount Reward Ad
    public void SetRewardFree(int amount)
    {
        PlayerPrefs.SetInt(KEY_REWARD_FREE, amount);
        PlayerPrefs.Save();
    }

    public int GetRewardFree()
    {
        return PlayerPrefs.GetInt(KEY_REWARD_FREE, C.RewardConfigs.CoinPerWatchAd);
    }

    // Set/Get Amount Buy Booster
    public void SetRewardBuyBooster(ResourceID type, int amount)
    {
        var key = string.Format(KEY_COUNT_BOOSTER_REWARD, type.ToString().ToUpper());
        PlayerPrefs.SetInt(key, amount);
        PlayerPrefs.Save();
    }

    public int GetRewardBuyBooster(ResourceID type)
    {
        var key = string.Format(KEY_COUNT_BOOSTER_REWARD, type.ToString().ToUpper());
        return PlayerPrefs.GetInt(key, ResourceConfigs[type].DefaultValue);
    }

    // Set/Get Amount Reward Level
    public void SetRewardLevelComplete(int amount)
    {
        PlayerPrefs.SetInt(KEY_REWARD_LEVEL_COMPLETE, amount);
        PlayerPrefs.Save();
    }

    public int GetRewardLevelComplete()
    {
        return PlayerPrefs.GetInt(KEY_REWARD_LEVEL_COMPLETE, C.RewardConfigs.CoinCompleteLevel);
    }

    public IAPSpriteConfig GetIAPSpriteConfig(string bundleID)
    {
        var config = _iapSpriteConfigs.Find(val => val.ID == bundleID);
        return config;
    }
}