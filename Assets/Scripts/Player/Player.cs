using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float _jumpForce, _moveSpeed, _dashForce, _glidDrag, _dashTime, _swiftForce;
    [SerializeField] private int _dashSpectrumCount;
    [SerializeField] private SpriteRenderer _playerSpectrum;
    [SerializeField] private Image _sleepPanel;
    [SerializeField] private VolumeProfile _volumeProfile;
    [SerializeField] private Image _dashCooldownImage;
    [SerializeField] private float _dashCooldown = 10f;
    [SerializeField] private AudioSource _glidSource; 

    [Space]
    [Header("SpeechBubble")]
    [SerializeField] private TextMeshProUGUI _speechNameUI;
    [SerializeField] private TextMeshProUGUI _speechMessageUI;
    [SerializeField] private Button _speechNextButton;

    private int _maxJumpCount = 2, _jumpCount = 2;
    private bool _isOnGround = false, _canDash = false;
    private float _dashTimer = 0f, _dashDir;
    private float _dashSpectrumTimer = 0f;
    private float _dashRemainCooldown = 0f;
    private int _spectrumCounter = 0;
    private float _originalDrag;
    private float _swiftTimer = 0f, _invincibilityTimer = 0f;
    private int _platformLayer;
    private bool _isSleeping = false;

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private Color _originalColor;
    private Dictionary<string, Color> _colorMultipliers = new();

    private Vector3 _spawnPoint;

    private readonly Queue<MessageAction> _messageActions = new();

    public bool IsDashUnlocked
    {
        get { return !SceneManager.GetActiveScene().name.Equals("InGameScene") || PlayerPrefs.GetInt("EnterStage") >= 5; }
    }

    public bool IsGlidUnlocked
    {
        get { return !SceneManager.GetActiveScene().name.Equals("InGameScene") || PlayerPrefs.GetInt("EnterStage") >= 3; }
    }

    public bool IsGliding
    {
        get { return Input.GetKey(KeyCode.LeftShift) && _rigid.velocity.y < 0f && IsGlidUnlocked; }
    }

    private void Awake()
    {
        Instance = this;

        _platformLayer = LayerMask.NameToLayer("Platform");
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalDrag = _rigid.drag;
        _originalColor = _spriteRenderer.color;
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _spawnPoint = transform.position;

        if (_volumeProfile.TryGet(out ColorAdjustments ca))
        {
            ca.saturation.value = 0f;
            ca.hueShift.value = 0f;
            ca.contrast.value = 0f;
        }

    }

    private void OnApplicationQuit()
    {
        if (_volumeProfile.TryGet(out ColorAdjustments ca))
        {
            ca.saturation.value = 0f;
            ca.hueShift.value = 0f;
            ca.contrast.value = 0f;
        }
    }

    private void Update()
    {
        if (_isOnGround) _canDash = true;


        //테스트코드
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowSpeech(new MessageAction[] {
                new MessageAction("나", "안녕하세요", () => { }),
                new MessageAction("나", "안녕하세요2", () => { }),
                new MessageAction("나", "안녕하세요3", () => { })
            });
        }

        var speechBubble = _speechNextButton.transform.parent.gameObject;

        if (_messageActions.Count > 0 && !speechBubble.activeSelf)
        {
            speechBubble.SetActive(true);
            var message = _messageActions.Peek();
            _speechNameUI.SetText(message.Name);
            _speechMessageUI.SetText(message.MessageText);
            _speechNextButton.onClick.RemoveAllListeners();
            _speechNextButton.onClick.AddListener(() =>
            {
                message.EventAction();
                speechBubble.SetActive(false);
                _messageActions.Dequeue();
            });
        }

        if (!_isSleeping)
        {
            if(!speechBubble.activeSelf)
            {
                if (Input.GetButton("Jump") && _jumpCount == _maxJumpCount && _isOnGround)
                {
                    Jump();
                }
                else if (Input.GetButtonDown("Jump") && _jumpCount > 0 && _jumpCount < _maxJumpCount)
                {
                    Jump();
                }

                GlidUpdate();
            }

            var hor = speechBubble.activeSelf ? 0 : Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(hor) > Mathf.Epsilon)
            {
                if (_canDash && _dashRemainCooldown <= 0f && _dashTimer <= 0f && Input.GetKeyDown(KeyCode.Return))
                {
                    Dash(hor);
                }
                _spriteRenderer.flipX = hor < 0f;
                _animator.SetBool("IsRunning", true);
            }
            else
            {
                _animator.SetBool("IsRunning", false);
            }

            _rigid.velocity = _rigid.velocity * Vector2.up +
                _moveSpeed * (_swiftTimer > 0f ? _swiftForce : 1f) * hor * Vector2.right;

            DashUpdate(hor);
        }
        else // 잠들고 있는 상태라면
        {
            _dashTimer = 0f;
            _spectrumCounter = 0;
            _dashSpectrumTimer = 0f;
            _rigid.velocity *= Vector3.up;

            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsGliding", false);
        }

        if (speechBubble.activeSelf) // 대화중이라면
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _speechNextButton.onClick.Invoke();
            }
        }

        _animator.SetBool("IsSleeping", _isSleeping);
        _animator.SetFloat("JumpVelocity", _rigid.velocity.y);


        if ((_invincibilityTimer -= Time.deltaTime) > 0f)
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

        if (transform.position.y < -30f) Sleep();
    }

    public void ShowSpeech(MessageAction[] actions)
    {
        foreach(var action in actions)
        {
            _messageActions.Enqueue(action);
        }
    }

    private void DashUpdate(float hor)
    {

        _animator.SetBool("IsDashing", _dashTimer > 0f);

        if(_dashRemainCooldown > 0f) _dashRemainCooldown -= Time.deltaTime;

        _dashCooldownImage.fillAmount = Mathf.Clamp01(_dashRemainCooldown / _dashCooldown);
        _dashCooldownImage.transform.parent.gameObject.SetActive(IsDashUnlocked);
        
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
                spectrum.sprite = _spriteRenderer.sprite;
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
    }

    private void GlidUpdate()
    {
        var isGliding = IsGliding;
        _glidSource.volume = Mathf.Clamp(_glidSource.volume + 0.1f * Time.deltaTime * (isGliding ? 1 : -1), 0f, 0.1f);
        _animator.SetBool("IsGliding", isGliding);
        _rigid.drag = isGliding ? _glidDrag : _originalDrag;
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
        if (!IsDashUnlocked) return;
        SoundManager.Instance.Play("Dash", 0.1f, 2.5f);
        _dashRemainCooldown = _dashCooldown;
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
        SoundManager.Instance.Play("Jump", 0.3f, 0.6f);
        --_jumpCount;
        _rigid.velocity = Vector2.up * force;
    }

    public void Jump()
    {
        Jump(_jumpForce);
    }

    public void SetInvincibility(float time)
    {
        _invincibilityTimer = time;
    }

    public void SetSwift(float time)
    {
        _swiftTimer = time;
    }

    public void Sleep()
    {
        if (_invincibilityTimer > 0f) return;
        if (_isSleeping) return;
        SoundManager.Instance.Play("Sleep", 0.1f, 0.8f);
        _isSleeping = true;
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {

        yield return new WaitForSeconds(0.8f);
        _volumeProfile.TryGet(out ColorAdjustments ca);

        _sleepPanel.gameObject.SetActive(true);
        var col = _sleepPanel.color;
        for (var i = 0f; i < 1f; i += Time.deltaTime / 1.5f)
        {
            col.a = i;
            _sleepPanel.color = col;

            ca.saturation.value = i * 100f;
            ca.hueShift.value = i * -100f;
            ca.contrast.value = i * 70f;

            yield return null;
        }
        col.a = 1;
        _sleepPanel.color = col;

        transform.position = _spawnPoint;
        yield return new WaitForSeconds(0.8f);

        for (var i = 1f; i >= 0f; i -= Time.deltaTime / 1.3f)
        {
            col.a = i;
            _sleepPanel.color = col;

            ca.saturation.value = i * 100f;
            ca.hueShift.value = i * -100f;
            ca.contrast.value = i * 70f;

            yield return null;
        }
        col.a = 0;
        _sleepPanel.color = col;

        ca.saturation.value = 0f;
        ca.hueShift.value = 0f;
        ca.contrast.value = 0f;

        _sleepPanel.gameObject.SetActive(false);

        _isSleeping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_isSleeping)
        {
            Sleep();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_isSleeping)
        {
            Sleep();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & _platformLayer) != 0 &&
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
        if ((collision.gameObject.layer & _platformLayer) != 0)
            _isOnGround = false;
    }
}

public struct MessageAction
{
    public readonly string Name, MessageText;
    public readonly Action EventAction;

    public MessageAction(string name, string text, Action action)
    {
        this.Name = name;
        this.MessageText = text;
        this.EventAction = action;
    }
}