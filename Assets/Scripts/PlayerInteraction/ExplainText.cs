using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(BoxCollider2D))]
public class ExplainText : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 10f, _minDistance = 5f;
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        var dist = (Player.Instance.transform.position - transform.position).magnitude;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 
            Mathf.Clamp01(1 - Mathf.Max(0, dist - _minDistance) / (_maxDistance - _minDistance)));
    }
}
