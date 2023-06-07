using UnityEngine;

[CreateAssetMenu(fileName = "GhostData", menuName = "ScripableObject/Ghost", order = 1)]
public class GhostData : ScriptableObject
{
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed {
        get { return _moveSpeed; }
        private set { }
    }
}
