using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : NetworkBehaviour
{
    public readonly Vector3 MovingShiftLeft = new Vector3(-15, -10);
    public readonly Vector3 MovingShiftRight = new Vector3(15, 10);
    public readonly Vector3Int LeftBottomCorner = new Vector3Int(1, 1);

    public GameObject FieldGridPrefab;
    public GameObject ObstacleGridPrefab;
    public Grid FieldGrid;
    public Tilemap FieldTilemap;
    public Camera HudCamera;
    public Grid ObstacleGrid;
    public Tilemap ObstacleTilemap;
    public bool UpdateLevel = false;
    public LevelTimer LevelTimer;
    public KnightCoxswain KnightCoxswain;

    private ServiceRegistry _sr;
    private PickingPerkPanel _ppp;
    private KnightCoxswain _kc;
    private List<GameObject> _listCScriptableTileObstacles = new List<GameObject>();

    public void Awake()
    {
        _ppp = FindObjectOfType<PickingPerkPanel>();
        LevelTimer = FindObjectOfType<LevelTimer>();
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.LevelBuilder = this;

        //FindObjectOfType<PickingCharacterScreen>().OnGo += () => gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //LevelTimer.OnTimeUp += () => gameObject.SetActive(false);
        //gameObject.SetActive(false);
        //MultiplayerController.OnServer += OnServer;
        //MultiplayerController.OnClient += OnClient;

    }

    public void OnClient()
    {
        
    }


    public void Update()
    {
        if (UpdateLevel && NetworkManager.Singleton.IsServer)
        {
            Vector3[] corners = FindEdges();
            UpdateEveryFrame(corners);
            BuildObstacleLayer(corners);
        }

    }

    private Vector3[] FindEdges()
    {
        Vector3  leftBottom = Vector3.zero;
        Vector3  rightUpper = Vector3.zero;
        List<Vector3> playerPositions = new List<KnightCoxswain>(FindObjectsOfType<KnightCoxswain>())
            .ConvertAll(kc => kc.transform.position);

        if (playerPositions.Count != 0)
        {
            foreach (Vector3 pos in playerPositions)
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
        //if (Grid == null)
        //{
        //    GameObject tilemapGridGoGameObject = new GameObject("FieldGrid", typeof(Grid), typeof(NetworkObject));
        //    tilemapGridGoGameObject.transform.parent = gameObject.transform.parent;

        //    if (tilemapGridGoGameObject.GetComponent<Grid>() is { } localTilemapGrid)
        //    {
        //        Grid = localTilemapGrid;
        //        Grid.cellSize = new Vector3(1, 1, 0);
        //    }

        ////if (tilemapGridGoGameObject.GetComponent<NetworkObject>() is { } localFieldNetManager)
        ////{
        ////    //NetworkManager.Singleton.AddNetworkPrefab(tilemapGridGoGameObject);
        ////    //localFieldNetManager.Spawn();
        ////}
        //}

        //if (!GetTilemapFromGrid(Grid))
        //{
        //    GameObject fieldTilemap = new GameObject("FieldTilemap", typeof(Tilemap), typeof(TilemapRenderer));
        //    fieldTilemap.transform.parent = Grid.transform;

        //    if (fieldTilemap.GetComponent<TilemapRenderer>() is { } fieldRender)
        //    {
        //        fieldRender.sortingOrder = -10;
        //    }
        //}

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

        //if (ObstacleGrid == null)
        //{
        //    GameObject obstacleGridGoGameObject = new GameObject("ObstacleGrid", typeof(Grid));
        //    obstacleGridGoGameObject.transform.parent = gameObject.transform.parent;

        //    if (obstacleGridGoGameObject.GetComponent<Grid>() is { } localGrid)
        //    {
        //        ObstacleGrid = localGrid;
        //        ObstacleGrid.cellSize = new Vector3(.5f, .5f, 0);
        //    }
        //}

        //if (!GetTilemapFromGrid(ObstacleGrid))
        //{
        //    GameObject obstacleTilemap = new GameObject("ОbstacleTilemap", typeof(Tilemap), typeof(TilemapRenderer));
        //    obstacleTilemap.transform.parent = ObstacleGrid.transform;

        //    if (obstacleTilemap.GetComponent<TilemapRenderer>() is { } obstacleRenderer)
        //    {
        //        obstacleRenderer.sortingOrder = -9;
        //    }

        //    ObstacleTilemap = GetTilemapFromGrid(ObstacleGrid);
        //}

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
        //Vector3 leftBottom = HudCamera.ViewportToWorldPoint(HudCamera.rect.min);
        Vector3 leftBottom = corners[0];
        Vector3 rightUpper = corners[1];
        //Vector3 rightUpper = HudCamera.ViewportToWorldPoint(HudCamera.rect.max);

        Vector3 cellSize = tilemap.cellSize;
        ScriptableTile st = GameAssets.MyFirstScriptableTile;
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
                            // print("tilePosition " + tilePosition);
                            // print("leftBottom " + leftBottom);
                            // print("rightUpper " + rightUpper);
                            // print(tilemap + " " + tilePosition + " " + st);
                            tilemap.SetTile(tilePosition, st);
                            //print("SpawnTitle " + st.LastSmartTile.TilePosition);
                            _sr.NetworkLevel.SetSpawnedTile(st.LastSmartTile);
                            _sr.NetworkLevel.SpawnTileClientRpc(st.LastSmartTile);
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

    //public Vector3 GetLeftBottom() => HudCamera.ViewportToWorldPoint(HudCamera.rect.min);
    //public Vector3 GetRightUpper() => HudCamera.ViewportToWorldPoint(HudCamera.rect.max);
    public Tilemap GetTilemapFromGrid(Grid grid) => grid.GetComponentInChildren<Tilemap>();

    public void BuildObstacleLayer(Vector3[] corners)
    {
        Vector3 leftBottom = corners[0];
        Vector3 rightUpper = corners[1];
        Tilemap tm = ObstacleTilemap;
        ScriptableTileObstacle st = GameAssets.ObstacleTile;
        st.SetHost(ObstacleGrid.transform);
        // print(st);
        int xStart = Mathf.CeilToInt(leftBottom.x / tm.cellSize.x);
        int yStart = Mathf.CeilToInt(leftBottom.y / tm.cellSize.y);
        // print(tm.cellSize);
        FillLayer(xStart, yStart, leftBottom, rightUpper, tm, st);
        _listCScriptableTileObstacles.AddRange(st.TileObstacles);
    }

    public void FillLayer(int xStart, int yStart, Vector3 leftBottom, Vector3 rightUpper, Tilemap tm, ScriptableTileObstacle st)
    {
        int xEnd = Mathf.CeilToInt(rightUpper.x / tm.cellSize.x);
        int yEnd = Mathf.CeilToInt(rightUpper.y / tm.cellSize.y);
        // Debug.Log(xStart + " " + yStart);
        for (int x = xStart; x < xEnd; x++)
        {
            float xCoord = x * tm.cellSize.x - LeftBottomCorner.x * tm.cellSize.x;
            // print(leftBottom.x + " " + xCoord);
            if (leftBottom.x - 1 <= xCoord && rightUpper.x > xCoord)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    // print(curMap.cellBounds.min.y);
                    Vector3Int tilePosition = new Vector3Int(x, y) - LeftBottomCorner;
                    // float yCoord = tilePosition.y * tm.cellSize.y - leftBottomCorner.y;
                    float yCoord = tilePosition.y * tm.cellSize.y;
                    // print(leftBottom.y + " " + yCoord);
                    if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
                    {
                        if (!tm.HasTile(tilePosition))
                        {
                            // print("tilePosition " + tilePosition);
                            // print("leftBottom " + leftBottom);
                            // print("rightUpper " + rightUpper);

                            tm.SetTile(tilePosition, st);
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
    }
}
