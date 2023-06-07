using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int platformLayer;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _jumpForce, _moveSpeed, _dashForce, _glidDrag, _dashTime;
    [SerializeField] private SpriteRenderer _playerSpectrum;
    private int _maxJumpCount = 2, _jumpCount = 2;
    private bool _isOnGround = false;
    private float _dashTimer = 0f, _dashDir;
    private float _dashSpectrumTimer = 0f;
    private int _spectrumCount = 0;
    private float _originalDrag;

    private void Awake()
    {
        platformLayer = LayerMask.NameToLayer("Platform");
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalDrag = _rigid.drag;
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

        _rigid.drag = Input.GetKey(KeyCode.LeftShift) ? _glidDrag : _originalDrag;

        var hor = Input.GetAxisRaw("Horizontal");
        
        if (Mathf.Abs(hor) > Mathf.Epsilon)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _dashTimer = _dashTime;
                _dashDir = hor * _dashForce;
            }
            _spriteRenderer.flipX = hor < 0f;
        }
        

        _rigid.velocity = _rigid.velocity * Vector2.up + _moveSpeed * hor * Vector2.right;
        if((_dashTimer -= Time.deltaTime) > 0f)
        {
            if (_dashDir * hor < 0) _dashTimer = 0f;
            _rigid.velocity = Vector2.right * _dashDir;

            if((_dashSpectrumTimer -= Time.deltaTime) < 0f)
            {
                _dashSpectrumTimer += _dashTime / 4f;
                var spectrum = Instantiate(_playerSpectrum, transform.position, Quaternion.identity);
                spectrum.sortingOrder -= _spectrumCount;
                var col = spectrum.color;
                col.a = 0.2f + _spectrumCount * 0.2f;
                spectrum.color = col;
                StartCoroutine(SpectrumRoutine(spectrum));
                _spectrumCount++;
            }
        }
        else if(_spectrumCount > 0)
        {
            _spectrumCount = 0;
            _dashSpectrumTimer = 0f;
        }
    }

    private IEnumerator SpectrumRoutine(SpriteRenderer spriteRenderer)
    {
        var col = spriteRenderer.color;
        var originalAlpha = col.a;
        for(var i = 1f; i > 0f; i -= Time.deltaTime)
        {
            col.a = originalAlpha * i;
            spriteRenderer.color = col;
            yield return null;
        }
        Destroy(spriteRenderer.gameObject);
    }

    public void Jump(float force)
    {
        print(--_jumpCount);
        _rigid.velocity = Vector2.up * force;
    }

    public void Jump()
    {
        Jump(_jumpForce);
    }

    public void Sleep()
    {
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        yield return null;
        print("Sleep...");
        //Not Implemented
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Sleep();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ((collision.gameObject.layer & platformLayer) != 0 && transform.position.y > collision.GetContact(0).point.y)
        {
            _isOnGround = true;
            _jumpCount = _maxJumpCount;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Sleep();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0 && transform.position.y > collision.GetContact(0).point.y)
            TilesManager.Instance.OnGroundCollision(this, collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0)
            _isOnGround = false;
    }
}
