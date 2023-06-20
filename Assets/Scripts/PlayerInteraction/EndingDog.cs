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
                        new("나", "헉.. 헉.. 겨우 쫒아왔네..", () => { }),
                        new("나", "바람아, 여기서 뭐해! 이제 집으로 돌아가자.", () => { }),
                        new("바람이", "날 찾아서 여기까지 와준거야?", () => {}),
                        new("나", "어? 바람아 너 말도 할 수 있었어?", () => { }),
                        new("바람이", "응. 왜냐면 여긴 꿈이니까. 넌 집으로 돌아가.", () => { }),
                        new("나", "왜? 싫어! 나, 너랑 같이 갈 거야!", () => { }),
                        new("바람이", "안 돼. 여긴... 아주 오래 살아서 집에 돌아갈 떄가 된 생명들만 올 수 있거든.", () => { }),
                        new("나", "그럼... 나도 아주 오래 살면 여기 올 수 있어?", () => { }),
                        new("바람이", "응. 그러니까 너는 천천히 와.\n나는 여기서 너를 기다릴게. 푹 쉬면서.", () => { }),
                        new("나", "...응! 천천히 올게, 그때까지 기다려 줘.\r\n", () => { }),
                        new("바람이", "그럼 다음에 또 보자, 안녕.", () => { }),
                        new("나", "... (안녕...)", () => {
                            _state = 2;
                            Player.Instance.End();
                        }),
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
