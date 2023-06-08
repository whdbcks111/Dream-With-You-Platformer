using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public float invincibleDuration = 5f; // ���� ���� �ð�
    public float itemRespawnDelay = 20f; // ������ ����� ������

    private bool isInvincible = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if (!isInvincible)
            {
                StartCoroutine(ActivateInvincibility());
                StartCoroutine(RespawnItem(collision.gameObject));
            }
        }
    }

    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        // ���� ���·� ����Ǵ� ���� ����

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        // ���� ���°� �����Ǵ� ���� ����
    }

    private IEnumerator RespawnItem(GameObject item)
    {
        yield return new WaitForSeconds(itemRespawnDelay);

        item.SetActive(true);
    }
}