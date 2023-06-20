using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionCanvas : MonoBehaviour
{
    CanvasGroup _canvas;

    void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (_canvas.alpha == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
