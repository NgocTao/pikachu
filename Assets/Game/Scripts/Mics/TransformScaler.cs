using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformScaler : MonoBehaviour
{
    void Start()
    {
        var ratio = (float)Screen.height / Screen.width;
        if (ratio > 1.9f)
        {
            transform.localScale = transform.localScale * 0.93f;
        }
        else
        {
            transform.localScale = transform.localScale;
        }
    }
}
