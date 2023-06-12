using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] private float _allowXDistance, _allowYDistance;
    [SerializeField] private float _followSpeed;
    private Vector3 _offset, _velocity = Vector3.zero;

    private void Start()
    {
        _target = FindObjectOfType<Player>().GetComponent<Transform>();
        _offset = transform.position - _target.position;
    }

    private void FixedUpdate()
    {
        var idealPos = _target.position + _offset;
        var targetPos = transform.position;

        if (transform.position.x < idealPos.x - _allowXDistance)
            targetPos.x = idealPos.x - _allowXDistance;
        else if (transform.position.x > idealPos.x + _allowXDistance)
            targetPos.x = idealPos.x + _allowXDistance;

        if (transform.position.y < idealPos.y - _allowYDistance)
            targetPos.y = idealPos.y - _allowYDistance;
        else if (transform.position.y > idealPos.y + _allowYDistance)
            targetPos.y = idealPos.y + _allowYDistance;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, 1 / _followSpeed, Mathf.Infinity, Time.fixedDeltaTime);
    }
}
