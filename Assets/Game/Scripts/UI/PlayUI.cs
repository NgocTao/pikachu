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

    System.Action _onHint;
    System.Action _onArrangeBoard;
    System.Action _onChangeTheme;

    public void Setup(System.Action onHint, System.Action onArrangeBoard, System.Action onChangeTheme)
    {
        _onHint = onHint;
        _onArrangeBoard = onArrangeBoard;
        _onChangeTheme = onChangeTheme;
    }

    public void TouchedHint()
    {
        _onHint?.Invoke();
    }
    public void TouchedArragneBoard()
    {
        _onArrangeBoard?.Invoke();
    }
    public void TouchedChangeTheme()
    {
        _onChangeTheme?.Invoke();
    }

    protected override void OnUIRemoved()
    {

    }
}
