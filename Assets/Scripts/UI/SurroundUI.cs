using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SurroundUI : MonoBehaviour
{
    [SerializeField] private float _surroundTime, _surroundX, _surroundY;
    [SerializeField] private SurroundType _surroundTypeX, _surroundTypeY;

    private RectTransform _rectTransform;
    private Vector3 _initPos;
    private float _timer = 0f;


    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initPos = _rectTransform.localPosition;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _surroundTime) _timer = 0;

        var progress = (_timer / _surroundTime) * 2 - 1;

        _rectTransform.localPosition = _initPos;

        switch (_surroundTypeX)
        {
            case SurroundType.Linear:
                _rectTransform.localPosition += _surroundX * (Mathf.Abs(progress) - 0.5f) * 2 * Vector3.right;
                break;
            case SurroundType.Sin:
                _rectTransform.localPosition += _surroundX * Mathf.Sin(progress * Mathf.PI) * Vector3.right;
                break;
            case SurroundType.Cos:
                _rectTransform.localPosition += _surroundX * Mathf.Cos(progress * Mathf.PI) * Vector3.right;
                break;
        }

        switch (_surroundTypeY)
        {
            case SurroundType.Linear:
                _rectTransform.localPosition += _surroundY * (Mathf.Abs(progress) - 0.5f) * 2 * Vector3.up;
                break;
            case SurroundType.Sin:
                _rectTransform.localPosition += _surroundY * Mathf.Sin(progress * Mathf.PI) * Vector3.up;
                break;
            case SurroundType.Cos:
                _rectTransform.localPosition += _surroundY * Mathf.Cos(progress * Mathf.PI) * Vector3.up;
                break;
        }
    }
}

public enum SurroundType
{
    Linear, Sin, Cos
}