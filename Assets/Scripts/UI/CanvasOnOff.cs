using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOnOff : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvas;


    protected Button _button;

    protected virtual private void Awake()
    {
        _button = GetComponent<Button>();
    }

    protected virtual void Start()
    {
        OffCanvas();
        _button.onClick.AddListener(CanvasControl);
    }

    public void CanvasControl()
    {
        if (_canvas.alpha == 1)
        {
            OffCanvas();
        }
        else
        {
            OnCanvas();
        }
    }
    
    void OnCanvas()
    {
        _canvas.alpha = 1;
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;
    }

    void OffCanvas()
    {
        _canvas.alpha = 0;
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }
}
