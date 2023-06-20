using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDog : MonoBehaviour
{
    [SerializeField] private float _triggerDistance = 6f;
    [SerializeField] private float _speed = 4f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    
    private int _state = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var dist = (Player.Instance.transform.position - transform.position).magnitude;
        var moveDir = 0f;

        switch(_state)
        {
            case 0:

                if(dist < _triggerDistance)
                {
                    _state = 1;
                    Player.Instance.ShowSpeech(new MessageAction[]
                    {
                        new("��", "��? �ٶ��̴�.", () => { }),
                        new("��", "�� ���⿡ ����?", () => { }),
                        new("��", "�ٶ���! �� ���� ���°ž�?", () => {
                            _state = 2;
                        }),
                        new("��", "�ٶ���!", () => { }),
                        new("��", "��� �ٶ���!", () => { }),
                        new("��", "�츮���� �� ���� �ƴѵ�...", () => { }),
                        new("��", "���󰡺��߰ھ�!", () => { })
                    });
                }

                break;

            case 2:
                moveDir = 1f;

                if (dist > _triggerDistance * 10 + 10f) Destroy(gameObject);

                break;
        }

        if (Mathf.Abs(moveDir) > Mathf.Epsilon) _spriteRenderer.flipX = moveDir > 0;

        transform.position += Vector3.right * moveDir * Time.deltaTime * _speed;
    }
}
