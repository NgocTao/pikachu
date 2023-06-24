using System.Collections;
using UnityEngine;
using TMPro;
using StormStudio.Common.Utils;

public class NumberRunBitmapText : MonoBehaviour
{
    [SerializeField] protected float _duration;
    BitmapNumberUI _numberText;
    int _currentValue;
    int _fromValue;
    int _toValue;
    ActionEaseInterval _actionRun;

    void Awake()
    {
        _numberText = GetComponent<BitmapNumberUI>();
    }

    public void SetCurrentValue(int value)
    {
        _currentValue = value;
        RefreshNumberText(_currentValue);
    }

    public void Run(int number)
    {
        _toValue = number;

        bool firstRun = _actionRun == null;
        if (firstRun)
        {
            _actionRun = new ActionEaseInterval
            {
                duration = _duration,
                tweenType = TweenFunc.TweenType.Sine_EaseOut
            };
        }

        if (firstRun || _actionRun.IsFinished)
        {
            _actionRun.Reset();
            StartCoroutine(PlayRun(_duration));
        }
        else
        {
            _actionRun.Reset();
        }
    }

    IEnumerator PlayRun(float duration)
    {
        _fromValue = _currentValue;
        while (!_actionRun.IsFinished)
        {
            var f = _actionRun.Step(Time.deltaTime);
            var value = Mathf.Lerp(_fromValue, _toValue, f);
            var newValue = Mathf.CeilToInt(value);
            if (_currentValue != newValue)
            {
                _currentValue = newValue;
                RefreshNumberText(_currentValue);
            }

            yield return null;
        }

        _currentValue = _toValue;
        RefreshNumberText(_currentValue);
    }

    void RefreshNumberText(int value)
    {
        _numberText.ShowNumber(value);
    }
}
