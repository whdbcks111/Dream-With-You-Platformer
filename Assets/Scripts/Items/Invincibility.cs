using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public float invincibleDuration = 5f; // 무적 지속 시간
    public float itemRespawnDelay = 20f; // 아이템 재등장 딜레이

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
        // 무적 상태로 변경되는 동작 수행

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        // 무적 상태가 해제되는 동작 수행
    }

    private IEnumerator RespawnItem(GameObject item)
    {
        yield return new WaitForSeconds(itemRespawnDelay);

        item.SetActive(true);
    }
}