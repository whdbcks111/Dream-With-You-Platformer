using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // 플레이어랑 닿으면 비활성화 되었다가 리젠되는 코드
    //               ^^^^^^^ bool 변수 하나 만들어가지고 true일땐 먹을 수 있게 하고 false일땐 못 먹게 하고
    // 업데이트에서 bool 변수에 따라서 색 반투명하게 바꾸는거 추가하면 될듯 _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    // 플레이어랑 닿았을 때 효과를 주는 함수 (만듦)

    abstract public void OnUseItem(Player player);
}
