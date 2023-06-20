using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _leftMax, _rightMax;
    [SerializeField] private GhostData _data;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalPosition;
    private int _direction = 1;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position.x > _originalPosition.x + _rightMax)
        {
            var pos = transform.position;
            pos.x = _originalPosition.x + _rightMax;
            transform.position = pos;
            _direction *= -1;
        }

        else if(transform.position.x < _originalPosition.x - _leftMax)
        {
            var pos = transform.position;
            pos.x = _originalPosition.x - _leftMax;
            transform.position = pos;
            _direction *= -1;
        }

        _spriteRenderer.flipX = _direction > 0;

        transform.position += _direction * _data.MoveSpeed * Time.deltaTime * Vector3.right; //최적화를 위한 코드
    }
}
