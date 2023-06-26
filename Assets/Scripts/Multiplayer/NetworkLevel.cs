using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkLevel : NetworkBehaviour
{
    private NetworkList<SmartTile> _spawnedBeforeConnectionTiles;
    private ServiceRegistry _sr;
    private NetworkVariable<ulong> _fieldGrid = new NetworkVariable<ulong>(0);

    private void Awake()
    {
        _spawnedBeforeConnectionTiles = new NetworkList<SmartTile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.NetworkLevel = this;

        NetworkManager.Singleton.OnClientConnectedCallback += SpawnAtTheStart;
        NetworkManager.Singleton.OnClientConnectedCallback += RecountPlayersClientRpc;
        //NetworkManager.Singleton.OnServerStarted += StartLevel;
        //print(NetworkManager.Singleton.IsServer);

        if (NetworkManager.Singleton.IsServer)
        {
            StartLevel();
            CacheValueServer();
        }
        else
        {
            CacheValuesClient();
            SpawnAtTheStart(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ClientRpc]
    private void RecountPlayersClientRpc(ulong uid)
    {
        if (IsClient)
        {
            _sr.LevelBuilder.RecountPlayers();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CacheValuesClient()
    {
        _sr.LevelBuilder.FieldGrid = CacheFieldGrid<Grid>(_fieldGrid.Value);
        _sr.LevelBuilder.FieldTilemap = _sr.LevelBuilder.GetTilemapFromGrid(_sr.LevelBuilder.FieldGrid);
    }

    public void CacheValueServer()
    {
        _fieldGrid.Value = _sr.LevelBuilder.FieldGrid.GetComponent<NetworkObject>().NetworkObjectId;

        //print(_sr.FieldGrid + " " + _sr.FieldTilemap);
    }

    private T CacheFieldGrid<T>(ulong networkId) where T : Component
    {
        List<GameObject> objects = new List<T>(FindObjectsOfType<T>()).ConvertAll(g => g.gameObject);
        GameObject targetGameObject = FindGameObjectByNetworkId(objects, networkId);

        if (targetGameObject && targetGameObject.GetComponent<T>() is { } endTarget)
        {
            return endTarget;
        }

        return null;
    }

    private GameObject FindGameObjectByNetworkId(List<GameObject> gameObjects, ulong networkId)
    {
        foreach (GameObject go in gameObjects)
        {
            if (go.GetComponent<NetworkObject>() is { } networkObject)
            {
                if (networkObject.NetworkObjectId == networkId)
                {
                    return go;
                }
            }
        }

        return null;
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

            foreach (SmartTile spawnedBeforeConnectionTile in _spawnedBeforeConnectionTiles)
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
        _sr.LevelBuilder.FieldTilemap.SetTile(tilePosition, sct);
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
            _spawnedBeforeConnectionTiles.Add(smartTile);
            //_spawnedBeforeConnectionTilesIndexes.Add(smartTile.SpriteFrequencyIndex);
            //print(_sr.TestList.SmartThings.Count);
        }
    }

    [ClientRpc]
    public void SpawnTileClientRpc(SmartTile smartTile) =>
        SpawnTileByIndex(smartTile.TilePosition, smartTile.SpriteFrequencyIndex);
}
