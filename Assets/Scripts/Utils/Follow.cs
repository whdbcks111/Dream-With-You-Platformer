using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] private float _allowXDistance, _allowYDistance;
    [SerializeField] private float _followSpeed;
    private Vector3 _offset;

    private void Awake()
    {
        _offset = transform.position - _target.position;
    }

    private void Update()
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

        transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed * Time.deltaTime);
    }
}
