using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[System.Serializable]
public enum PurchaseType
{
    Free = 0,
    Coin = 1,
    Ads = 2,
    USD = 3
}

public enum BunbleType
{
    Normal = 0,
    Bundle = 1,
    Offer = 3
}

[System.Serializable]
public enum ResourceID
{
    None = -1,
    Coin = 0,
    Hint = 1,
    Time = 2,
    Undo = 3
}

[System.Serializable]
public class IAPData
{
    public int Order;
    public string Name;
    public PurchaseType TypePurchase;
    public BunbleType TypeBundle;
    public string BundleID;
    public ResourceID[] ResourceIDs;
    public int[] Values;
    public string Price;
    public string Tag;
    public string ExpireDate; 
}