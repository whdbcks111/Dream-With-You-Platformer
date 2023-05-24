using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;

    [SerializeField] private float _jumpForce, _moveSpeed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        transform.position += Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * _moveSpeed;
    }
}
