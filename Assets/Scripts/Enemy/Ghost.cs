using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _leftMax, _rightMax;
    [SerializeField] private GhostData _data;
    private Vector3 _originalPosition;
    private int _direction = 1;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if(transform.position.x>= _originalPosition.x + _rightMax ||
            transform.position.x <= _originalPosition.x - _leftMax)
        {
            _direction *= -1;
        }

        transform.position += _direction * _data.MoveSpeed * Time.deltaTime * Vector3.right; //최적화를 위한 코드
    }
}
