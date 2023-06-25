using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkLevel : NetworkBehaviour
{
    private List<SmartTile> _spawnedBeforeConnectionTiles = new List<SmartTile>();
    private ServiceRegistry _sr;


    // Start is called before the first frame update
    void Start()
    {
        //_spawnedBeforeConnectionTiles = new NetworkList<SmartTile>();
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.NetworkLevel = this;

        NetworkManager.Singleton.OnClientConnectedCallback += SpawnAtTheStart;
        //NetworkManager.Singleton.OnServerStarted += StartLevel;
        //print(NetworkManager.Singleton.IsServer);


        if (NetworkManager.Singleton.IsServer)
        {
            StartLevel();
            CacheValue();
        }
        else
        {
            CacheValue();
            SpawnAtTheStart(NetworkManager.Singleton.LocalClientId);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CacheValue()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.FieldGrid = FindObjectOfType<Grid>();
        _sr.FieldTilemap = FindObjectOfType<Tilemap>();

        //print(_sr.FieldGrid + " " + _sr.FieldTilemap);
    }

    private void StartLevel()
    {
        _sr.LevelBuilder.OnServer();
    }

    //[ServerRpc]
    //private void GetSmartTilesServerRpc(ulong uid)
    //{
    //    SpawnAtTheStartClientRpc(_spawnedBeforeConnectionTiles);
    //}

    private void SpawnAtTheStart(ulong uid)
    {
        //if (IsClient)
        //{
            //print(_spawnedBeforeConnectionTiles);
            //print(_sr.TestList.SmartThings.Count);

            foreach (SmartTile spawnedBeforeConnectionTile in _sr.TestList.SmartThings)
            {
                //print(spawnedBeforeConnectionTile);
                SpawnTileByIndex(spawnedBeforeConnectionTile.TilePosition, spawnedBeforeConnectionTile.SpriteFrequencyIndex);
            }
            //_spawnedBeforeConnectionTiles.ForEach(
            //    smartTile =>
            //    {
            //        print(smartTile);
            //        SpawnTileByIndex(smartTile.TilePosition, smartTile.SpriteFrequencyIndex);
            //    });
        //}
    }

    private void SpawnTileByIndex(Vector3Int tilePosition, int spIndex)
    {
        ScriptableChosenTile sct = GameAssets.ScriptableChosenTile;
        ScriptableTile st = GameAssets.MyFirstScriptableTile;
        sct.SetSpriteFrequency(st.Sprites[spIndex]);
        //print(_sr.FieldGrid.gameObject + " " + _sr.FieldTilemap.gameObject + " " + tilePosition);
        _sr.FieldTilemap.SetTile(tilePosition, sct);
    }

    //[ClientRpc]
    //public void CacheValueClientRpc()
    //{
    //    _sr.FieldGrid = FindObjectOfType<Grid>();
    //    _sr.FieldTilemap = FindObjectOfType<Tilemap>();

    //    print(_sr.FieldGrid + " " + _sr.FieldTilemap);
    //}

    public void SetSpawnedTile(SmartTile smartTile)
    {
        //print(NetworkManager.Singleton.IsServer + " " + smartTile.SpriteFrequencyIndex);
        //print("_spawnedBeforeConnectionTiles " + _spawnedBeforeConnectionTiles);
        if (NetworkManager.Singleton.IsServer)
        {
            _sr.TestList.AddElement(smartTile.SpriteFrequencyIndex);
            _sr.TestList.SmartThings.Add(smartTile);
            _spawnedBeforeConnectionTiles.Add(smartTile);
            //_spawnedBeforeConnectionTilesIndexes.Add(smartTile.SpriteFrequencyIndex);
            //print(_sr.TestList.SmartThings.Count);
        }
    }

    [ClientRpc]
    public void SpawnTileClientRpc(SmartTile smartTile) =>
        SpawnTileByIndex(smartTile.TilePosition, smartTile.SpriteFrequencyIndex);
}
