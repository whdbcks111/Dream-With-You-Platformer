using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesManager : MonoBehaviour
{
    [SerializeField] Tilemap _normalTilemap, _collapseTilemap, _collapsedTilemap, 
        _flashOnTilemap, flashOffTilemap, _forceJumpTilemap;

    [Space]
    [SerializeField] float _flashInterval, _collapseTime, _forceJumpForce;


}
