using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Obstacle", menuName = "Scriptables/Tile/Obstacle", order = 1), Serializable]
public class ScriptableTileObstacle : TileBase
{
    public Vector3 CellSize;
    public GameObjectFrequency[] GameObjects;
    public List<GameObject> TileObstacles = new List<GameObject>();

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //TODO убрать обработчик весов в родительский класс
        int weight = new List<GameObjectFrequency>(GameObjects).Aggregate(0, (carrier, gmf) => carrier += gmf.Frequency);
        int pickValue = Random.Range(1, weight + 1);
        int accumulator = 0;
        // Debug.Log(weight + " " + pickValue);
        foreach (GameObjectFrequency gof in GameObjects)
        {
            accumulator += gof.Frequency;

            if (accumulator >= pickValue)
            {
                if (gof.GameObject)
                {
                    TileObstacles.Add(Instantiate(gof.GameObject, (Vector3)position * CellSize.x, Quaternion.identity));
                }

                break;
            }
        }

        // // bool evenCell = Mathf.Abs(position.y + position.x) % 2 > 0;
        // // Debug.Log(position);
        // GameObjectFrequency gof = GameObjects[Random.Range(0, GameObjects.Length)];
        // // Debug.Log(gof.Possibility + " " +( Random.value * 100));
        // if (gof.Possibility > Random.value * 100)
        // {
        //     Instantiate(gof.GameObject, (Vector3)position * CellSize.x, Quaternion.identity);
        // }
        //
        // // tileData.sprite = GameObjects[Random.Range(0, GameObjects.Length)].Sprite;
    }
}

//TODO Вынести в отдельный файл
[Serializable]
public struct GameObjectFrequency
{
    public GameObject GameObject;
    //TODO Сделать веса для генерации тайлов
    public int Frequency;
    public Sprite Sprite;
    [Range(0, 100)]
    public int Possibility;
}