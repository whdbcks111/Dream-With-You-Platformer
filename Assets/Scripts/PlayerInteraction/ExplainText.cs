using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(BoxCollider2D))]
public class ExplainText : MonoBehaviour
{
    Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Start()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(OnText());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(OffText());
        }
    }

    IEnumerator OnText()
    {
        yield return new WaitForFixedUpdate();

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a + Time.deltaTime * 3);
        if (_text.color.a >= 1)
        {
            yield return null;
        }
        else
        {
            StartCoroutine(OnText());
        }
    }

    IEnumerator OffText()
    {
        yield return new WaitForFixedUpdate();

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - Time.deltaTime* 3);
        if (_text.color.a <= 0)
        {
            yield return null;
        }
        else
        {
            StartCoroutine(OffText());
        }
    }
}
