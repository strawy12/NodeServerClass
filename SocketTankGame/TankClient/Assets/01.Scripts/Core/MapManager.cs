using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MapCategory
{
    PATH = 0,
    BLOCK = 1,
    SAFEZONE = 2,
    NONE = 99
}

public class MapManager
{
    public static MapManager Instance;

    private Tilemap _mainMap;
    private Tilemap _collisionMap;
    private Tilemap _safeZoneMap;

    public MapManager(Transform mapObject)
    {
        _mainMap = mapObject.Find("Background").GetComponent<Tilemap>();
        _collisionMap = mapObject.Find("Collision").GetComponent<Tilemap>();
        _safeZoneMap = mapObject.Find("SafeZone").GetComponent<Tilemap>();
    }

    public Vector3Int GetTilePos(Vector3 worldPos)
    {
        return _mainMap.WorldToCell(worldPos);
    }

    public MapCategory GetTileCategory(Vector3Int tilePos)
    {
        TileBase tile = _collisionMap.GetTile(tilePos);
        if(tile != null) return MapCategory.BLOCK;

         tile = _safeZoneMap.GetTile(tilePos);
        if (tile != null) return MapCategory.SAFEZONE;

         tile = _mainMap.GetTile(tilePos);
        if (tile != null) return MapCategory.PATH;

        return MapCategory.NONE;

    }

}
