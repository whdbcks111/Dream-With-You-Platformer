using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : Item
{
    public override void OnUseItem(Player player)
    {
        // player ���� ������ �ִ� �ڵ�
        player.SetInvincibility(5);
    }
}
