using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/ObstacleFilledTile", menuName = "Scriptables/Tile/ObstacleFilledTile", order = 2), Serializable]
public class ObstacleFilledTile : TileBase
{
    private Sprite TileSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = TileSprite;
        tileData.colliderType = Tile.ColliderType.Sprite;
    }

    public void SetSprite(Sprite tileSprite)
    {
        TileSprite = tileSprite;
    }
}