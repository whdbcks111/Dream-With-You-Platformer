using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int platformLayer;
    private Rigidbody2D _rigid;

    [SerializeField] private float _jumpForce, _moveSpeed, _dashForce, _minGlidingVelY;
    private int _maxJumpCount = 2, _jumpCount = 2;
    private bool _isOnGround = false;

    private void Awake()
    {
        platformLayer = LayerMask.NameToLayer("Platform");
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

        if(Input.GetKey(KeyCode.E))
        {
            var vel = _rigid.velocity;
            if(vel.y < _minGlidingVelY)
                vel.y = Mathf.Lerp(vel.y, _minGlidingVelY, 6f * Time.deltaTime);
            _rigid.velocity = vel;

            print("Gliding...");
        }

        var hor = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.R) && Mathf.Abs(hor) > Mathf.Epsilon)
        {
            print("Dash!!");
            _rigid.AddForce(_dashForce * hor * Vector2.right, ForceMode2D.Impulse);
        }

        _rigid.velocity = _rigid.velocity * Vector2.up + _moveSpeed * hor * Vector2.right;
    }

    private void Jump()
    {
        --_jumpCount;
        _rigid.velocity = Vector2.up * _jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ((collision.gameObject.layer & platformLayer) != 0 && transform.position.y > collision.GetContact(0).point.y)
        {
            _isOnGround = true;
            _jumpCount = _maxJumpCount;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0 && transform.position.y > collision.GetContact(0).point.y)
            TilesManager.Instance.OnGroundCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0)
            _isOnGround = false;
    }
}
