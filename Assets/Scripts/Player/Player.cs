using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private int platformLayer;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _jumpForce, _moveSpeed, _dashForce, _glidDrag, _dashTime, _swiftForce;
    [SerializeField] private int _dashSpectrumCount;
    [SerializeField] private SpriteRenderer _playerSpectrum;

    private int _maxJumpCount = 2, _jumpCount = 2;
    private bool _isOnGround = false, _canDash = false;
    private float _dashTimer = 0f, _dashDir;
    private float _dashSpectrumTimer = 0f;
    private int _spectrumCounter = 0;
    private float _originalDrag;
    private float _swiftTimer = 0f, _invincibilityTimer = 0f;

    private Color _originalColor;
    private Dictionary<string, Color> _colorMultipliers = new();

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        platformLayer = LayerMask.NameToLayer("Platform");
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalDrag = _rigid.drag;
        _originalColor = _spriteRenderer.color;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isOnGround) _canDash = true;

        if (Input.GetButton("Jump") && _jumpCount == _maxJumpCount && _isOnGround)
        {
            Jump();
        }
        else if (Input.GetButtonDown("Jump") && _jumpCount > 0 && _jumpCount < _maxJumpCount)
        {
            Jump();
        }

        _rigid.drag = Input.GetKey(KeyCode.LeftShift) ? _glidDrag : _originalDrag;

        var hor = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(hor) > Mathf.Epsilon)
        {
            if (_canDash && _dashTimer <= 0f && Input.GetKeyDown(KeyCode.Return))
            {
                Dash(hor);
            }
            _spriteRenderer.flipX = hor < 0f;
        }


        _rigid.velocity = _rigid.velocity * Vector2.up + 
            _moveSpeed * (_swiftTimer > 0f ? _swiftForce : 1f) * hor * Vector2.right;
        if ((_dashTimer -= Time.deltaTime) > 0f)
        {
            if (_dashDir * hor < 0) _dashTimer = 0f;
            _rigid.velocity = Vector2.right * _dashDir;

            if ((_dashSpectrumTimer -= Time.deltaTime) < 0f)
            {
                _dashSpectrumTimer += _dashTime / _dashSpectrumCount;
                var spectrum = Instantiate(_playerSpectrum, transform.position, Quaternion.identity);
                spectrum.sortingOrder -= _spectrumCounter;
                spectrum.flipX = _spriteRenderer.flipX;
                var col = spectrum.color;
                col.a = 0.2f + ((_spectrumCounter + 1f) / _dashSpectrumCount) * 0.8f;
                spectrum.color = col;
                StartCoroutine(SpectrumRoutine(spectrum));
                _spectrumCounter++;
            }
        }
        else if (_spectrumCounter > 0)
        {
            _spectrumCounter = 0;
            _dashSpectrumTimer = 0f;
        }

        if((_invincibilityTimer -= Time.deltaTime) > 0f)
        {
            // 무적 이펙트, 파티클
            ApplyColor("inv", Color.yellow, 0.1f);
        }

        if((_swiftTimer -= Time.deltaTime) > 0f)
        {
            // 신속 이펙트, 파티클
            ApplyColor("swift", Color.cyan, 0.1f);
        }

        _spriteRenderer.color = _originalColor;
        foreach(var color in _colorMultipliers.Values)
        {
            _spriteRenderer.color *= color;
        }
    }

    public void ApplyColor(string key, Color color, float time)
    {
        StartCoroutine(ColorRoutine(key, color, time));
    }

    private IEnumerator ColorRoutine(string key, Color color, float time)
    {
        _colorMultipliers[key] = color;
        yield return new WaitForSeconds(time);
        _colorMultipliers.Remove(key);
    }

    public void Dash(float dir)
    {
        _canDash = false;
        _dashTimer = _dashTime;
        _dashDir = dir * _dashForce;
    }

    private IEnumerator SpectrumRoutine(SpriteRenderer spriteRenderer)
    {
        var col = spriteRenderer.color;
        var originalAlpha = col.a;
        for (var i = 1f; i > 0f; i -= Time.deltaTime)
        {
            col.a = originalAlpha * i;
            spriteRenderer.color = col;
            yield return null;
        }
        Destroy(spriteRenderer.gameObject);
    }

    public void Jump(float force)
    {
        --_jumpCount;
        _rigid.velocity = Vector2.up * force;
    }

    public void Jump()
    {
        Jump(_jumpForce);
    }

    public void Sleep()
    {
        if (_invincibilityTimer > 0f) return;
        StartCoroutine(SleepRoutine());
    }

    public void SetInvincibility(float time)
    {
        _invincibilityTimer = time;
    }

    public void SetSwift(float time)
    {
        _swiftTimer = time;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Sleep();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0 &&
            _boxCollider.bounds.min.y >= collision.GetContact(0).point.y &&
            _rigid.velocity.y <= 0.1f)
        {
            _isOnGround = true;
            _jumpCount = _maxJumpCount;
            TilesManager.Instance.OnGroundCollision(this, collision);
        }
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & platformLayer) != 0)
            _isOnGround = false;
    }
}
