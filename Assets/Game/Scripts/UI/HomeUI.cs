using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StormStudio.Common.UI;
using TMPro;
using System;
using StormStudio.Common.Native;
using DG.Tweening;

public class HomeUI : UIController
{

    System.Action _onStartGame;
    void Awake()
    {
    }

    public void Setup(System.Action onStartGame)
    {
        _onStartGame =onStartGame;
    }

    public void TouchedStartGame()
    {
        _onStartGame?.Invoke();
    }

    protected override void OnUIRemoved()
    {

    }
}
