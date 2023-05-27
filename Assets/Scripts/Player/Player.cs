using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;

    [SerializeField] private float _jumpForce, _moveSpeed;
    private int _maxJumpCount = 2, _jumpCount = 2;
    private bool _isOnGround = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetButton("Jump") && _jumpCount == _maxJumpCount && _isOnGround)
        {
            Jump();
        }
        else if(Input.GetButtonDown("Jump") && _jumpCount > 0 && _jumpCount < _maxJumpCount)
        {
            Jump();
        }

        transform.position += Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * _moveSpeed;
    }

    private void Jump()
    {
        --_jumpCount;
        _rigid.velocity = Vector2.up * _jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && transform.position.y > collision.GetContact(0).point.y)
        {
            _isOnGround = true;
            _jumpCount = _maxJumpCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            _isOnGround = false;
    }
}
