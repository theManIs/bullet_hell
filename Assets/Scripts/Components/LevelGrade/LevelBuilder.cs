using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
    public Vector3 MovingShiftLeft = new Vector3(-100, -100);
    public Vector3 MovingShiftRight = new Vector3(100, 100);
    public readonly Vector3Int LeftBottomCorner = new Vector3Int(1, 1);
    private readonly int _generateThreshold = 1;

    public GameObject FieldGridPrefab;
    public GameObject ObstacleGridPrefab;
    public Grid FieldGrid;
    public Tilemap FieldTilemap;
    public Grid ObstacleGrid;
    public Tilemap ObstacleTilemap;
    public bool UpdateLevel = false;
    public LevelTimer LevelTimer;

    private ServiceRegistry _sr;
    private PickingPerkPanel _ppp;
    private List<GameObject> _listCScriptableTileObstacles = new List<GameObject>();
    private Vector3[] _lastCorners = new Vector3[2];
    private List<Transform> _playersGameObjects = new List<Transform>();
    private LevelCreator _lc;

    public void Awake()
    {
        _ppp = FindObjectOfType<PickingPerkPanel>();
        LevelTimer = FindObjectOfType<LevelTimer>();

        //FindObjectOfType<PickingCharacterScreen>().OnGo += () => gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.LevelBuilder = this;
        _lc = FindObjectOfType<LevelCreator>();
        //LevelTimer.OnTimeUp += () => gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }

    public void Update()
    {
        if (UpdateLevel && NetworkManager.Singleton.IsServer)
        {
            Vector3[] corners = FindEdges(_playersGameObjects.ConvertAll(p => p.position));
            
            if ((_lastCorners[0] - corners[0]).magnitude > _generateThreshold
                || (_lastCorners[1] - corners[1]).magnitude > _generateThreshold)
            {
                //print((_lastCorners[0] - corners[0]).magnitude + " " + (_lastCorners[1] - corners[1]).magnitude);
                UpdateEveryFrame(corners);
                BuildObstacleLayer(corners);
                _lastCorners = corners;
            }
        }
    }

    public void RecountPlayers()
    {
        _playersGameObjects = new List<KnightCoxswain>(FindObjectsOfType<KnightCoxswain>())
            .ConvertAll(kc => kc.transform);
        //print(_playersGameObjects.Count);
    }

    public void RemovePlayer(Transform t)
    {
        _playersGameObjects.Remove(t);
    }

    private Vector3[] FindEdges(List<Vector3> playersPosition)
    {
        Vector3  leftBottom = Vector3.zero;
        Vector3  rightUpper = Vector3.zero;

        if (playersPosition.Count != 0)
        {
            foreach (Vector3 pos in playersPosition)
            {
                if (leftBottom.x > pos.x)
                {
                    leftBottom.x = pos.x;
                }

                if (leftBottom.y > pos.y)
                {
                    leftBottom.y = pos.y;
                }

                if (pos.x > rightUpper.x)
                {
                    rightUpper.x = pos.x;
                }

                if (pos.y > rightUpper.y)
                {
                    rightUpper.y = pos.y;
                }
            }
        }

        leftBottom += MovingShiftLeft;
        rightUpper += MovingShiftRight;

        return new Vector3[] { leftBottom, rightUpper };
    }

    public void OnServer()
    {
        if (FieldGridPrefab != null)
        {
            GameObject fieldGridGoGameObject = Instantiate(FieldGridPrefab);

            if (fieldGridGoGameObject.GetComponent<NetworkObject>() is { } localFieldNetManager)
            {
                localFieldNetManager.Spawn();
            }

            if (fieldGridGoGameObject.GetComponent<Grid>() is { } localFieldGrid)
            {
                FieldGrid = localFieldGrid;
                FieldTilemap = GetTilemapFromGrid(FieldGrid);
            }
        }

        if (ObstacleGridPrefab is { })
        {
            GameObject obstacleGridGoGameObject = Instantiate(ObstacleGridPrefab);

            if (obstacleGridGoGameObject.GetComponent<NetworkObject>() is { } localObstacleNetManager)
            {
                localObstacleNetManager.Spawn();
            }

            if (obstacleGridGoGameObject.GetComponent<Grid>() is { } localObstacleGrid)
            {
                ObstacleGrid = localObstacleGrid;
                ObstacleTilemap = GetTilemapFromGrid(ObstacleGrid);
            }
        }

        UpdateLevel = true;
    }


    public void OnDisable()
    {
        // print(_listCScriptableTileObstacles.Count);
        foreach (GameObject sto in _listCScriptableTileObstacles)
        {
            // print(sto);
            Destroy(sto);
        }

        //Destroy(_kc.gameObject);
        UpdateLevel = false;
        //Destroy(GetTilemapFromGrid(Grid).gameObject);
        //Destroy(GetTilemapFromGrid(ObstacleGrid).gameObject);
        new List<CoinOperator>(FindObjectsOfType<CoinOperator>()).ForEach(co => Destroy(co.gameObject));
    }

    public void UpdateEveryFrame(Vector3[] corners)
    {
        // Debug.Log(HudCamera.rect.max + " " + HudCamera.rect.center + " " + HudCamera.transform.position);
        // print(HudCamera.ViewportToWorldPoint(HudCamera.rect.min) + " " + HudCamera.ViewportToWorldPoint(HudCamera.rect.max));
        // print(Tilemap.cellBounds.position);
        // print(Tilemap.cellBounds.center);
        // print(Tilemap.cellBounds.max);

        Tilemap tilemap = FieldTilemap;
        Vector3 leftBottom = corners[0];
        Vector3 rightUpper = corners[1];
        Vector3 cellSize = tilemap.cellSize;
        Vector3Int leftBottomCorner = new Vector3Int(1, 1);

        int xStart = Mathf.CeilToInt(leftBottom.x / cellSize.x);
        int yStart = Mathf.CeilToInt(leftBottom.y / cellSize.y);
        // print(corners[0].x + " " + xStart);
        //TODO установиь правильные границы для генерации карты
        for (int x = xStart; x < 100; x++)
        {
            float xCoord = x * cellSize.x - leftBottomCorner.x;
            // print(leftBottom.x + " " + xCoord);
            if (leftBottom.x - 1 <= xCoord && rightUpper.x > xCoord)
            {
                for (int y = yStart; y < 100; y++)
                {
                    // print(tilemap.cellBounds.min.y);
                    Vector3Int tilePosition = new Vector3Int(x, y) - leftBottomCorner;
                    // float yCoord = tilePosition.y * cellSize.y - leftBottomCorner.y;
                    float yCoord = tilePosition.y * cellSize.y;

                    if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
                    {
                        if (!tilemap.HasTile(tilePosition))
                        {
                            //tilemap.SetTile(tilePosition, st);

                            SmartTile smartTile = SpawnTileWithinDomain(tilePosition, GetTerrainDomain());
                            tilemap.SetTile(tilePosition, _sr.LevelCreator.TerrainTilePackage[smartTile.SpriteFrequencyIndex].TerrainTile);
                            _sr.NetworkLevel.SetSpawnedTile(smartTile);
                            _sr.NetworkLevel.SpawnTileClientRpc(smartTile);
                        }
                    }

                    if (!(rightUpper.y > yCoord))
                    {
                        break;
                    }
                }
            }
            
            if (!(rightUpper.x > xCoord))
            {
                break;
            }
        }
        // EditorApplication.isPaused = true;

    }

    public int[] GetTerrainDomain()
    {
        List<TerrainTilePackage> terrainTilePackages = new List<TerrainTilePackage>(_sr.LevelCreator.TerrainTilePackage);
        return terrainTilePackages.ConvertAll(s => s.Frequency).ToArray();
    }

    public Tilemap GetTilemapFromGrid(Grid grid) => grid.GetComponentInChildren<Tilemap>();

    public void BuildObstacleLayer(Vector3[] corners)
    {
        Vector3 leftBottom = corners[0];
        Vector3 rightUpper = corners[1];
        Tilemap tm = ObstacleTilemap;

        int xStart = Mathf.CeilToInt(leftBottom.x / tm.cellSize.x);
        int yStart = Mathf.CeilToInt(leftBottom.y / tm.cellSize.y);
        // print(tm.cellSize);
        FillLayer(xStart, yStart, leftBottom, rightUpper, tm);
        //_listCScriptableTileObstacles.AddRange(st.TileObstacles);
    }

    //public void FillLayer(int xStart, int yStart, Vector3 leftBottom, Vector3 rightUpper, Tilemap tm, ScriptableTileObstacle st)
    //{
    //    int xEnd = Mathf.CeilToInt(rightUpper.x / tm.cellSize.x);
    //    int yEnd = Mathf.CeilToInt(rightUpper.y / tm.cellSize.y);
    //    // Debug.Log(xStart + " " + yStart);
    //    for (int x = xStart; x < xEnd; x++)
    //    {
    //        float xCoord = x * tm.cellSize.x - LeftBottomCorner.x * tm.cellSize.x;
    //        // print(leftBottom.x + " " + xCoord);
    //        if (leftBottom.x - 1 <= xCoord && rightUpper.x > xCoord)
    //        {
    //            for (int y = yStart; y < yEnd; y++)
    //            {
    //                // print(curMap.cellBounds.min.y);
    //                Vector3Int tilePosition = new Vector3Int(x, y) - LeftBottomCorner;
    //                // float yCoord = tilePosition.y * tm.cellSize.y - leftBottomCorner.y;
    //                float yCoord = tilePosition.y * tm.cellSize.y;
    //                // print(leftBottom.y + " " + yCoord);
    //                if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
    //                {
    //                    if (!tm.HasTile(tilePosition))
    //                    {
    //                        // print("tilePosition " + tilePosition);
    //                        // print("leftBottom " + leftBottom);
    //                        // print("rightUpper " + rightUpper);

    //                        tm.SetTile(tilePosition, st);
    //                    }
    //                }

    //                if (!(rightUpper.y > yCoord))
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //        if (!(rightUpper.x > xCoord))
    //        {
    //            break;
    //        }
    //    }
    //}    
    
    public void FillLayer(int xStart, int yStart, Vector3 leftBottom, Vector3 rightUpper, Tilemap tm)
    {
        int xEnd = Mathf.CeilToInt(rightUpper.x / tm.cellSize.x);
        int yEnd = Mathf.CeilToInt(rightUpper.y / tm.cellSize.y);
        // Debug.Log(xStart + " " + yStart);
        for (int y = yStart; y < yEnd; y++)
        {
            float yCoord = y * tm.cellSize.x - LeftBottomCorner.y * tm.cellSize.y;
            // print(leftBottom.x + " " + xCoord);
            if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    float xCoord = x * tm.cellSize.x - LeftBottomCorner.x * tm.cellSize.x;
                    // print(curMap.cellBounds.min.y);
                    Vector3Int tilePosition = new Vector3Int(x, y) - LeftBottomCorner;
                    // float yCoord = tilePosition.y * tm.cellSize.y - leftBottomCorner.y;
                    // print(leftBottom.y + " " + yCoord);
                    if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
                    {
                        if (!tm.HasTile(tilePosition))
                        {
                            //tm.SetTile(tilePosition, st);

                            SmartTile smartTile = SpawnTileWithinDomain(tilePosition, GetObstacleDomain());
                            if (_lc.ObstacleTilePackage[smartTile.SpriteFrequencyIndex].ObstacleTile is { } ot)
                            {
                                ot.SetTilemap(tm);
                                tm.SetTile(tilePosition, ot);
                                _sr.NetworkLevel.SetSpawnedObstacle(smartTile);
                            }
                        }
                    }

                    if (!(rightUpper.x > xCoord))
                    {
                        break;
                    }
                }
            }

            if (!(rightUpper.y > yCoord))
            {
                break;
            }
        }
    }

    private int[] GetObstacleDomain()
    {
        List<ObstacleTilePackage> terrainTilePackages = new List<ObstacleTilePackage>(_lc.ObstacleTilePackage);
        return terrainTilePackages.ConvertAll(s => s.Frequency).ToArray();
    }


    private SmartTile SpawnTileWithinDomain(Vector3Int position, int[] domain)
    {
        PerlinNoiseGenerator png = new PerlinNoiseGenerator(_lc.Scale, domain, _lc.StartingNoiseBlockSize, _lc.StartingNoiseBlockSize);
        int spriteIndex = png.GetIndexWeighted(position.x, position.y);

        return new SmartTile { SpriteFrequencyIndex = spriteIndex, TilePosition = position };
    }
}
