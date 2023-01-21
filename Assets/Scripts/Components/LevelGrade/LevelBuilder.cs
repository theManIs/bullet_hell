using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    public readonly Vector3Int LeftBottomCorner = new Vector3Int(1, 1);

    public Grid Grid;
    public Camera HudCamera;
    public Grid ObstacleGrid;
    public bool UpdateLevel = false;
    public LevelTimer LevelTimer;

    // public Sprite A;
    // public Sprite B;
    // public ScriptableTile ScriptableTile;
    public KnightCoxswain KnightCoxswain;

    private PickingPerkPanel _ppp;
    private KnightCoxswain _kc;
    private List<GameObject> _listCScriptableTileObstacles = new List<GameObject>();

    public void Awake()
    {
        _ppp = FindObjectOfType<PickingPerkPanel>();
        LevelTimer = FindObjectOfType<LevelTimer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelTimer.OnTimeUp += () => gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        // _gc = FindObjectOfType<GameController>();

        if (!FindObjectOfType<KnightCoxswain>())
        {
            _kc = Instantiate(KnightCoxswain, Vector3.zero, Quaternion.identity);
            _kc.PerkProcessor = new PerkProcessor().Subscribe(_ppp);
        }

        if (!GetTilemapFromGrid(Grid))
        {
            GameObject BackgroundT = new GameObject("BackgroundT", typeof(Tilemap), typeof(TilemapRenderer));
            BackgroundT.transform.parent = Grid.transform;
        }

        if (!GetTilemapFromGrid(ObstacleGrid))
        {
            GameObject ObstacleT = new GameObject("ObstacleT", typeof(Tilemap), typeof(TilemapRenderer));
            ObstacleT.transform.parent = ObstacleGrid.transform;
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

        Destroy(_kc.gameObject);
        UpdateLevel = false;
        Destroy(GetTilemapFromGrid(Grid).gameObject);
        Destroy(GetTilemapFromGrid(ObstacleGrid).gameObject);
        new List<CoinOperator>(FindObjectsOfType<CoinOperator>()).ForEach(co => Destroy(co.gameObject));
    }

    public void Update()
    {
        if (UpdateLevel)
        {
            UpdateEveryFrame();
        }
    }

    public void UpdateEveryFrame()
    {
        // Debug.Log(HudCamera.rect.max + " " + HudCamera.rect.center + " " + HudCamera.transform.position);
        // print(HudCamera.ViewportToWorldPoint(HudCamera.rect.min) + " " + HudCamera.ViewportToWorldPoint(HudCamera.rect.max));
        // print(Tilemap.cellBounds.position);
        // print(Tilemap.cellBounds.center);
        // print(Tilemap.cellBounds.max);

        Tilemap tilemap = GetTilemapFromGrid(Grid);
        Vector3 leftBottom = HudCamera.ViewportToWorldPoint(HudCamera.rect.min);
        Vector3 rightUpper = HudCamera.ViewportToWorldPoint(HudCamera.rect.max);
        
        Vector3 cellSize = tilemap.cellSize;
        ScriptableTile st = GameAssets.MyFirstScriptableTile;
        Vector3Int leftBottomCorner = new Vector3Int(1, 1);
        int xStart = Mathf.CeilToInt(leftBottom.x / cellSize.x);
        int yStart = Mathf.CeilToInt(leftBottom.y / cellSize.y);
        
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

                            tilemap.SetTile(tilePosition, st);
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

        Vector3 playerPos = _kc.transform.position;
        playerPos.z = HudCamera.transform.position.z;
        HudCamera.transform.position = playerPos;

        BuildObstacleLayer();
    }

    public Vector3 GetLeftBottom() => HudCamera.ViewportToWorldPoint(HudCamera.rect.min);
    public Vector3 GetRightUpper() => HudCamera.ViewportToWorldPoint(HudCamera.rect.max);
    public Tilemap GetTilemapFromGrid(Grid grid) => grid.GetComponentInChildren<Tilemap>();

    public void BuildObstacleLayer()
    {
        Vector3 leftBottom = GetLeftBottom();
        Vector3 rightUpper = GetRightUpper();
        Tilemap tm = GetTilemapFromGrid(ObstacleGrid);
        ScriptableTileObstacle st = GameAssets.ObstacleTile;
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
