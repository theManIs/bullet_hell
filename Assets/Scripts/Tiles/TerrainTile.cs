using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/TerrainTile", menuName = "Scriptables/Tile/TerrainTile", order = 1), Serializable]
public class TerrainTile : TileBase
{
    public Sprite TileSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = TileSprite;
    }
}