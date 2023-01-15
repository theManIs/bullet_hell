using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Tile-0", menuName = "Scriptables/Tile/Background", order = 1), Serializable]
public class ScriptableTile : TileBase
{
    public SpriteFrequency[] Sprites;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //TODO убрать обработчик весов в родительский класс
        int weight = new List<SpriteFrequency>(Sprites).Aggregate(0, (carrier, sf) => carrier += sf.Frequency);
        int pickValue = Random.Range(1, weight);
        int accumulator = 0;
        // Debug.Log(weight + " " + pickValue);
        foreach (SpriteFrequency sf in Sprites)
        {
            accumulator += sf.Frequency;

            if (accumulator >= pickValue)
            {
                tileData.sprite = sf.Sprite;

                break;
            }
        }

        // // bool evenCell = Mathf.Abs(position.y + position.x) % 2 > 0;
        
        // tileData.sprite = Sprites[Random.Range(0, Sprites.Length)].Sprite;
    }
}

//TODO Вынести в отдельный файл
[Serializable]
public struct SpriteFrequency
{
    public Sprite Sprite;
    //TODO Сделать веса для генерации тайлов
    public int Frequency;
}