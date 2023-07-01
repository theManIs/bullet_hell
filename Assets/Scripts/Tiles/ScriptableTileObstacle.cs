using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Obstacle", menuName = "Scriptables/Tile/Obstacle", order = 3), Serializable]
public class ScriptableTileObstacle : TileBase
{
    public Transform Host;
    public Vector3 CellSize;
    public GameObjectFrequency[] GameObjects;
    public List<GameObject> TileObstacles = new List<GameObject>();
    public float Scale = 0.1f;
    public int StartingNoiseBlockSize = 10;
    public int RandomSeed = 100;

    private Tilemap _tilemap;
    private ScriptableChosenObstacleTile _scot;

    public void OnEnable()
    {
        StartingNoiseBlockSize = Convert.ToInt32(Random.value * RandomSeed);
        _scot = GameAssets.ScriptableChosenObstacleTile;
    }

    //public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    //{
    //    //TODO убрать обработчик весов в родительский класс
    //    int weight = new List<GameObjectFrequency>(GameObjects).Aggregate(0, (carrier, gmf) => carrier += gmf.Frequency);
    //    int pickValue = Random.Range(1, weight + 1);
    //    int accumulator = 0;
    //    // Debug.Log(weight + " " + pickValue);
    //    foreach (GameObjectFrequency gof in GameObjects)
    //    {
    //        accumulator += gof.Frequency;

    //        if (accumulator >= pickValue)
    //        {
    //            if (gof.GameObject)
    //            {
    //                for (int j = 0; j < gof.rect.height; j++)
    //                {
    //                    for (int k = 0; k < gof.rect.width; k++)
    //                    {
    //                        if (j == 0 && k == 0)
    //                        {
    //                            continue;
    //                        }

    //                        if (_tilemap.HasTile(position + new Vector3Int(k, j)))
    //                        {
    //                            return;
    //                        }
    //                    }
    //                }

    //                Vector3 obstaclePos = ((Vector3)position + new Vector3(gof.rect.x, gof.rect.y)) * CellSize.x;
    //                GameObject point = new GameObject("Center");
    //                point.transform.position = obstaclePos - new Vector3(gof.rect.x, gof.rect.y) * CellSize.x;
    //                GameObject obstacleInstance = Instantiate(gof.GameObject, obstaclePos, Quaternion.identity);
    //                point.transform.parent = obstacleInstance.transform;
    //                TileObstacles.Add(obstacleInstance);

    //                if (obstacleInstance.GetComponent<NetworkObject>() is { } netOnj)
    //                {
    //                    netOnj.Spawn(true);
    //                }

    //                if (Host is { })
    //                {
    //                    obstacleInstance.transform.parent = Host;
    //                }

    //                _scot.SetHost(obstacleInstance.transform);
    //                for (int j = 0; j < gof.rect.height; j++)
    //                {
    //                    for (int k = 0; k < gof.rect.width; k++)
    //                    {
    //                        if (j == 0 && k == 0)
    //                        {
    //                            continue;
    //                        }

    //                        _tilemap.SetTile(position + new Vector3Int(k, j), _scot);
    //                    }
    //                }
    //            }

    //            break;
    //        }
    //    }

    //    // // bool evenCell = Mathf.Abs(position.y + position.x) % 2 > 0;
    //    // // Debug.Log(position);
    //    // GameObjectFrequency gof = GameObjects[Random.Range(0, GameObjects.Length)];
    //    // // Debug.Log(gof.Possibility + " " +( Random.value * 100));
    //    // if (gof.Possibility > Random.value * 100)
    //    // {
    //    //     Instantiate(gof.GameObject, (Vector3)position * CellSize.x, Quaternion.identity);
    //    // }
    //    //
    //    // // tileData.sprite = GameObjects[Random.Range(0, GameObjects.Length)].Sprite;
    //}

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        List<GameObjectFrequency> spriteList = new List<GameObjectFrequency>(GameObjects);
        int[] domain = spriteList.ConvertAll(s => s.Frequency).ToArray();
        PerlinNoiseGenerator png = new PerlinNoiseGenerator(Scale, domain, StartingNoiseBlockSize, StartingNoiseBlockSize);
        int gameObjectIndex = png.GetIndexWeighted(position.x, position.y);
        GameObjectFrequency gof = GameObjects[gameObjectIndex];

        if (gof.GameObject)
        {
            for (int j = 0; j < gof.rect.height; j++)
            {
                for (int k = 0; k < gof.rect.width; k++)
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

            Vector3 obstaclePos = ((Vector3)position + new Vector3(gof.rect.x, gof.rect.y)) * CellSize.x;
            //GameObject point = new GameObject("Center");
            //point.transform.position = obstaclePos - new Vector3(gof.rect.x, gof.rect.y) * CellSize.x;
            GameObject obstacleInstance = Instantiate(gof.GameObject, obstaclePos, Quaternion.identity);
            //point.transform.parent = obstacleInstance.transform;
            TileObstacles.Add(obstacleInstance);

            if (obstacleInstance.GetComponent<NetworkObject>() is { } netOnj)
            {
                netOnj.Spawn(true);
            }

            if (Host is { })
            {
                obstacleInstance.transform.parent = Host;
            }

            _scot.SetHost(obstacleInstance.transform);
            for (int j = 0; j < gof.rect.height; j++)
            {
                for (int k = 0; k < gof.rect.width; k++)
                {
                    if (j == 0 && k == 0)
                    {
                        continue;
                    }

                    _tilemap.SetTile(position + new Vector3Int(k, j), _scot);
                }
            }
        }
    }

    public void SetHost(Transform host) => Host = host;
    public void SetTilemap(Tilemap tilemap) => _tilemap = tilemap;
}

//TODO Вынести в отдельный файл
[Serializable]
public struct GameObjectFrequency
{
    public GameObject GameObject;
    //TODO Сделать веса для генерации тайлов
    public int Frequency;
    public Sprite Sprite;
    public Rect rect;
}