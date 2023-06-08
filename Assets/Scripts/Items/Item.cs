using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // �÷��̾�� ������ ��Ȱ��ȭ �Ǿ��ٰ� �����Ǵ� �ڵ�
    //               ^^^^^^^ bool ���� �ϳ� �������� true�϶� ���� �� �ְ� �ϰ� false�϶� �� �԰� �ϰ�
    // ������Ʈ���� bool ������ ���� �� �������ϰ� �ٲٴ°� �߰��ϸ� �ɵ� _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    // �÷��̾�� ����� �� ȿ���� �ִ� �Լ� (����)

    abstract public void OnUseItem(Player player);
}
