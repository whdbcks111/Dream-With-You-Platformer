using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private bool _isUsed = false;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    protected float regenTime = 10;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isUsed  && other.gameObject.TryGetComponent(out Player player))
        {
            _isUsed = true;
            OnUseItem(player);
            StartCoroutine(RegenRoutine());
        }
    }
    private IEnumerator RegenRoutine()
    {
        yield return new WaitForSeconds(regenTime);
        _isUsed= false;
    }
    private void Update()
    {
        _spriteRenderer.color = _originalColor * (_isUsed ? new Color(.5f, .5f, .5f, .3f) : Color.white);
    }
    abstract public void OnUseItem(Player player);
}