using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/ObstacleTile", menuName = "Scriptables/Tile/ObstacleTile", order = 1), Serializable]
public class ObstacleTile : TileBase
{
    public Sprite ObstacleSprite;

    private Tilemap _tilemap;
    private ScriptableChosenObstacleTile _scot;
    private ObstacleFilledTile _otf;

    public void OnEnable()
    {
        _scot = GameAssets.ScriptableChosenObstacleTile;
        _otf = ScriptableObject.CreateInstance<ObstacleFilledTile>();
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        int tiledWidth = Mathf.CeilToInt(ObstacleSprite.rect.width / ObstacleSprite.pixelsPerUnit / 0.5f);
        int tiledHeight = Mathf.CeilToInt(ObstacleSprite.rect.height / ObstacleSprite.pixelsPerUnit / 0.5f);
        int tiledOffsetX = Mathf.FloorToInt(tiledWidth / 2f);
        int tiledOffsetY = Mathf.FloorToInt(tiledHeight / 2f);

        for (int j = 0; j < tiledHeight; j++)
        {
            for (int k = 0; k < tiledWidth; k++)
            {
                if (j == 0 && k == 0)
                {
                    continue;
                }

                if (_tilemap.HasTile(position + new Vector3Int(k, j)))
                {
                    return;
                }
            }
        }

        _scot.SetHost(_tilemap.transform);
        _otf.SetSprite(ObstacleSprite);

        for (int j = 0; j < tiledHeight; j++)
        {
            for (int k = 0; k < tiledWidth; k++)
            {
                if (j == tiledOffsetY && k == tiledOffsetX)
                {
                    _tilemap.SetTile(position + new Vector3Int(tiledOffsetX, tiledOffsetY), _otf);
                }
                else
                {
                    _tilemap.SetTile(position + new Vector3Int(k, j), _scot);
                }
            }
        }

    }

    public void SetTilemap(Tilemap tilemap) => _tilemap = tilemap;
}
