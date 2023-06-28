using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Tile-1", menuName = "Scriptables/Tile/OneObstacle", order = 1), Serializable]
public class ScriptableChosenObstacleTile : TileBase
{
    public Vector3 CellSize;
    public Transform Host;

    private GameObjectFrequency GameObjects;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Instantiate absolutely nothing
        //Debug.Log(position);
        //Vector3 obstaclePos = (Vector3)position * CellSize.x;
        //GameObject point = new GameObject("Take position");
        //point.transform.position = obstaclePos;
        //point.transform.parent = Host;
    }

    public void SetHost(Transform host) => Host = host;
}