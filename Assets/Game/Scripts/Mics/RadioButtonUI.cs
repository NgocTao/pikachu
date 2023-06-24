using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class RadioButtonUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _text;

    [Header("Frames")]
    [SerializeField] private Sprite _iconFrameOn;
    [SerializeField] private Sprite _iconFrameOff;

    [Header("Colors")]
    [SerializeField] private Sprite _textOn;
    [SerializeField] private Sprite _textOff;

    public System.Action<int> OnTouched;

    public int Index;
    
    public void Setup(System.Action<int> onTouched)
    {
        OnTouched = onTouched;
    }

    public void EnableButton(bool value)
    {
        _icon.sprite = value ? _iconFrameOn : _iconFrameOff;
        _text.sprite = value ? _textOn : _textOff;

        _icon.SetNativeSize();
        _text.SetNativeSize();
    }

    public void Touched()
    {
        SoundManager.Instance.PlaySfxTapButton();
        OnTouched?.Invoke(Index);
    }
}
