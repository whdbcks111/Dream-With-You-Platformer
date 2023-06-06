using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance { get; private set; }

    [SerializeField] Tilemap _collapseTilemap, _collapsedTilemap,
        _flashOnTilemap, _flashOffTilemap, _forceJumpTilemap;

    [Space]
    [SerializeField] float _flashInterval, _collapseTime, _collapseReplaceTime, _forceJumpForce;

    private bool _isFlashOn = true;
    private float _flashTimer = 0f;
    private HashSet<Vector3Int> _collapseWaitSet = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Collapse(Vector3Int cellPos)
    {
        if (_collapseWaitSet.Contains(cellPos) || _collapseTilemap.GetTile(cellPos) is null) return;
        Stack<Vector3Int> stack = new();
        stack.Push(cellPos);

        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        
        while(stack.Count > 0)
        {
            var p = stack.Pop();

            _collapseWaitSet.Add(p);
            StartCoroutine(CollapseRoutine(p));

            foreach(var dir in directions)
            {
                var child = p + dir;
                if (!_collapseWaitSet.Contains(child) && _collapseTilemap.GetTile(child) is not null)
                {
                    stack.Push(child);
                }
            }
            
        }
        
    }

    public void OnGroundCollision(Collision2D collision)
    {
        if(collision.gameObject == _collapseTilemap.gameObject)
        {
            foreach(var contact in collision.contacts)
            {
                if (contact.point.y >= collision.otherCollider.transform.position.y) continue;
                var pos = contact.point - collision.GetContact(0).normal * 0.4f;
                Vector3Int cellPos = _collapseTilemap.WorldToCell(pos);

                Collapse(cellPos);
            }
        }
        else if (collision.gameObject == _forceJumpTilemap.gameObject)
        {
            collision.otherRigidbody.velocity = _forceJumpForce * Vector2.up;
        }
    }

    private IEnumerator CollapseRoutine(Vector3Int pos)
    {
        var tile = _collapseTilemap.GetTile(pos);

        for (float timer = 0f; timer < _collapseTime; timer += Time.deltaTime)
        {
            yield return null;
        }
        _collapseTilemap.SetTile(pos, null);
        _collapsedTilemap.SetTile(pos, tile);

        for (float timer = 0f; timer < _collapseReplaceTime; timer += Time.deltaTime)
        {
            yield return null;
        }
        _collapseTilemap.SetTile(pos, tile);
        _collapsedTilemap.SetTile(pos, null);

        _collapseWaitSet.Remove(pos);
    }

    private void Update()
    {
        // 점멸 타일 온오프
        if((_flashTimer -= Time.deltaTime) <= 0)
        {
            _flashTimer += _flashInterval;

            Tilemap fromTimemap, toTilemap;
            if(_isFlashOn)
            {
                fromTimemap = _flashOnTilemap;
                toTilemap = _flashOffTilemap;
            }
            else
            {
                fromTimemap = _flashOffTilemap;
                toTilemap = _flashOnTilemap;
            }
            _isFlashOn = !_isFlashOn;

            var enumerator = fromTimemap.cellBounds.allPositionsWithin.GetEnumerator();
            do
            {
                var pos = enumerator.Current;
                var tile = fromTimemap.GetTile(pos);
                if (tile is null) continue;
                toTilemap.SetTile(pos, tile);
                fromTimemap.SetTile(pos, null);
            }
            while(enumerator.MoveNext());
        }
    }
}
