using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadioGroupUI : MonoBehaviour
{
    [SerializeField] private RadioButtonUI[] _radioBtns;

    public System.Action<int> OnChanged;
    int _currentIndex = 0;

    private void Awake()
    {
        _currentIndex = 0;
        for (int i = 0; i < _radioBtns.Length; i++)
        {
            _radioBtns[i].Setup(TouchedItem);
            _radioBtns[i].EnableButton(_radioBtns[i].Index == _currentIndex);
        }
    }

    public void Setup(int page)
    {
        _currentIndex = page;
        updateGraphics();
    }

    public void TouchedItem(int index)
    {
        if (_currentIndex == index)
            return;
        _currentIndex = index;
        OnChanged?.Invoke(_currentIndex);
        updateGraphics();
    }

    void updateGraphics()
    {
        for (int i = 0; i < _radioBtns.Length; i++)
        {
            _radioBtns[i].EnableButton(_radioBtns[i].Index == _currentIndex);
        }
    }
}
