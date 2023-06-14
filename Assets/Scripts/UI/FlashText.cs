using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlashText : MonoBehaviour
{
    [SerializeField] private float _flashInterval;
    private TextMeshProUGUI _ui;
    private float _flashTimer = 0f;
    private int _dir = 1;

    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(_dir > 0)
        {
            _flashTimer += Time.deltaTime;
            if (_flashTimer > _flashInterval) _dir *= -1;
        }
        else
        {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer < 0) _dir *= -1;
        }

        _ui.alpha = Mathf.Clamp01(_flashTimer / _flashInterval);
    }
}
