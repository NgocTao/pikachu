using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StormStudio.Common.UI;
using TMPro;
using System;
using StormStudio.Common.Native;
using DG.Tweening;

public class PlayUI : UIController
{

    System.Action _onStartGame;
    void Awake()
    {
    }

    public void Setup()
    {
    }

    public void TouchedStartGame()
    {
        _onStartGame?.Invoke();
    }

    protected override void OnUIRemoved()
    {

    }
}
