using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using static UnityEditor.PlayerSettings;

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

    public void OnGroundCollision(Collision2D collision)
    {
        if(collision.gameObject == _collapseTilemap.gameObject)
        {

            foreach(var contact in collision.contacts)
            {
                if (contact.point.y >= collision.otherCollider.transform.position.y) continue;
                for(var xOffset = -1; xOffset <= 1; xOffset ++)
                {
                    var pos = contact.point + xOffset * 0.1f * Vector2.right - collision.GetContact(0).normal * 0.3f;
                    Vector3Int cellPos = _collapseTilemap.WorldToCell(pos);
                    if (!_collapseWaitSet.Contains(cellPos))
                    {
                        StartCoroutine(CollapseRoutine(cellPos));
                    }

                } 
            }
            
        }
        else if(collision.gameObject == _forceJumpTilemap.gameObject)
        {
            collision.otherRigidbody.velocity = _forceJumpForce * Vector2.up;
        }
    }

    private IEnumerator CollapseRoutine(Vector3Int pos)
    {
        var tile = _collapseTilemap.GetTile(pos);
        print(pos);
        _collapseWaitSet.Add(pos);
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
