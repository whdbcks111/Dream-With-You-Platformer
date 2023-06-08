using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : Item
{
    public override void OnUseItem(Player player)
    {
        // player 에게 무적을 주는 코드
        player.SetInvincibility(5);
    }
}
