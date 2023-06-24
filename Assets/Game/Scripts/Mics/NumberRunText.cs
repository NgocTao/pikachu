using System.Collections;
using UnityEngine;
using TMPro;
using StormStudio.Common.Utils;

public class NumberRunText : MonoBehaviour
{
    [SerializeField] protected float _duration;
    TextMeshProUGUI _numberText;
    int _currentValue;
    int _fromValue;
    int _toValue;
    ActionEaseInterval _actionRun;
    Coroutine _textRunRoutine;

    void Awake()
    {
        _numberText = GetComponent<TextMeshProUGUI>();
    }

    public void SetValueWithoutRefresh(int value)
    {
        _currentValue = value;
    }

    public void SetCurrentValue(int value)
    {
        _currentValue = value;
        RefreshNumberText(_currentValue);
    }

    public void SetText(string text)
    {
        _numberText.text = text;
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

        if (!_actionRun.IsFinished && _textRunRoutine != null)
        {
            StopCoroutine(_textRunRoutine);
        }

        _actionRun.Reset();
        _textRunRoutine = StartCoroutine(PlayRun(_duration));
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
        _textRunRoutine = null;
        RefreshNumberText(_currentValue);
    }

    void RefreshNumberText(int value)
    {
        if (value == 0)
            _numberText.text = "0";
        else
            _numberText.text = value.ToString("#,##");
    }
}
