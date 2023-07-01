using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Tile-1", menuName = "Scriptables/Tile/OneTile", order = 4), Serializable]
public class ScriptableChosenTile : TileBase
{
    public SpriteFrequency SpriteFrequency;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = SpriteFrequency.Sprite;
    }

    public void SetSpriteFrequency(SpriteFrequency spriteFrequency)
    {
        SpriteFrequency = spriteFrequency;
    }
}