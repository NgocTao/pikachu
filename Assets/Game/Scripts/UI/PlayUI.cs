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
    [SerializeField] TextMeshProUGUI _countdownText;
    [SerializeField] TextMeshProUGUI _socre;
    float _countdownSeconds = 180f;
    bool _isCountingDown = false;

    System.Action _onHint;
    System.Action _onArrangeBoard;
    System.Action _onChangeTheme;
    System.Action<bool> _overTime;

    public void Setup(System.Action onHint, System.Action onArrangeBoard, System.Action onChangeTheme, System.Action<bool> overTime)
    {
        _onHint = onHint;
        _onArrangeBoard = onArrangeBoard;
        _onChangeTheme = onChangeTheme;
        _overTime = overTime;
        _countdownText.text = "3:00";

        StartCoroutine(CountdownCoroutine());
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

    IEnumerator CountdownCoroutine()
    {
        while (_countdownSeconds > 0f && !_isCountingDown)
        {
            int minutes = Mathf.FloorToInt(_countdownSeconds / 60);
            int seconds = Mathf.FloorToInt(_countdownSeconds % 60);
            _countdownText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);
            _countdownSeconds--;
        }
        _countdownText.text = "0:00";
        _overTime?.Invoke(_isCountingDown);
    }

    public void StopCountTime(bool isCountingDown)
    {
        _isCountingDown = isCountingDown;
        _socre.text =_countdownSeconds.ToString();
    }


}
