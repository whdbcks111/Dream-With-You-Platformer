using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;

    [SerializeField] private float _jumpForce, _moveSpeed;
    private int _maxJumpCount = 1, _jumpCount = 1;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetButton("Jump") && _jumpCount > 0)
        {
            --_jumpCount;
            _rigid.velocity = Vector2.up * _jumpForce;
        }

        transform.position += Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime * _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && transform.position.y > collision.GetContact(0).point.y)
            _jumpCount = _maxJumpCount;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            _jumpCount = 0;
    }
}
