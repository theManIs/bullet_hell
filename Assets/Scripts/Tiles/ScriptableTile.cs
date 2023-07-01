using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "Assets/Resources/Scriptables/Tiles/Tile-0", menuName = "Scriptables/Tile/Background", order = 2), Serializable]
public class ScriptableTile : TileBase
{
    private ServiceRegistry _sr;
    public SpriteFrequency[] Sprites;
    public SmartTile LastSmartTile;
    public float Scale = 0.1f;
    public int StartingNoiseBlockSize = 10;

    public void OnEnable()
    {
        StartingNoiseBlockSize = Convert.ToInt32(Random.value * 100);
        //Debug.Log(StartingNoiseBlockSize);
    }

    //public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    //{
    //    List<SpriteFrequency> spriteList = new List<SpriteFrequency>(Sprites);
    //    //TODO убрать обработчик весов в родительский класс
    //    int weight = new List<SpriteFrequency>(Sprites).Aggregate(0, (carrier, sf) => carrier += sf.Frequency);
    //    int pickValue = Random.Range(1, weight);
    //    int accumulator = 0;
    //    SpriteFrequency spriteFrequency = default;
    //    // Debug.Log(weight + " " + pickValue);
    //    foreach (SpriteFrequency sf in Sprites)
    //    {
    //        accumulator += sf.Frequency;

    //        if (accumulator >= pickValue)
    //        {
    //            tileData.sprite = sf.Sprite;
    //            spriteFrequency = sf;
    //            break;
    //        }
    //    }

    //    _sr = FindObjectOfType<ServiceRegistry>();
    //    SmartTile st = new SmartTile
    //    {
    //        SpriteFrequencyIndex = spriteList.IndexOf(spriteFrequency),
    //        TilePosition = position
    //    };

    //    LastSmartTile = st;

    //    //_sr.NetworkLevel.SetSpawnedTile(st);
    //    //_sr.NetworkLevel.SpawnTileClientRpc(st);
    //}

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        List<SpriteFrequency> spriteList = new List<SpriteFrequency>(Sprites);
        int[] domain = spriteList.ConvertAll(s => s.Frequency).ToArray();
        PerlinNoiseGenerator png = new PerlinNoiseGenerator(Scale, domain, StartingNoiseBlockSize, StartingNoiseBlockSize);
        int spriteIndex = png.GetIndexWeighted(position.x, position.y);
        //Debug.Log(position.x + " " + position.y + " " + spriteIndex);
        //Debug.Log(position.x * Scale + " " + position.y * Scale + " " + Mathf.PerlinNoise(position.x * Scale, position.y * Scale));
        tileData.sprite = spriteList[spriteIndex].Sprite;
        LastSmartTile = new SmartTile { SpriteFrequencyIndex = spriteIndex, TilePosition = position };
    }
}
