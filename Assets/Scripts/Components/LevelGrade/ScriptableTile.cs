using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Tile-0", menuName = "Scriptables/Tile/Background", order = 1), Serializable]
public class ScriptableTile : TileBase
{
    private ServiceRegistry _sr;
    public SpriteFrequency[] Sprites;
    public SmartTile LastSmartTile;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        List<SpriteFrequency> spriteList = new List<SpriteFrequency>(Sprites);
        //TODO убрать обработчик весов в родительский класс
        int weight = new List<SpriteFrequency>(Sprites).Aggregate(0, (carrier, sf) => carrier += sf.Frequency);
        int pickValue = Random.Range(1, weight);
        int accumulator = 0;
        SpriteFrequency spriteFrequency = default;
        // Debug.Log(weight + " " + pickValue);
        foreach (SpriteFrequency sf in Sprites)
        {
            accumulator += sf.Frequency;

            if (accumulator >= pickValue)
            {
                tileData.sprite = sf.Sprite;
                spriteFrequency = sf;
                break;
            }
        }

        _sr = FindObjectOfType<ServiceRegistry>();
        SmartTile st = new SmartTile
        {
            SpriteFrequencyIndex = spriteList.IndexOf(spriteFrequency),
            TilePosition = position
        };

        LastSmartTile = st;

        //_sr.NetworkLevel.SetSpawnedTile(st);
        //_sr.NetworkLevel.SpawnTileClientRpc(st);
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