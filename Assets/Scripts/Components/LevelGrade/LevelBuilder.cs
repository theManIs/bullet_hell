using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    public readonly Vector3Int LeftBottomCorner = new Vector3Int(1, 1);

    public Tilemap Tilemap;
    public Grid Grid;
    public TilemapRenderer TilemapRenderer;
    public BoundsInt BoundsInt;
    public Camera HudCamera;
    public Grid ObstacleGrid;

    // public Sprite A;
    // public Sprite B;
    public ScriptableTile ScriptableTile;
    public KnightCoxswain KnightCoxswain;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Tilemap.size);
        // Debug.Log(Tilemap.GetTile(new Vector3Int(0, 0, 1)));
        // Debug.Log(Tilemap.GetTile(Vector3Int.one));
        // TileData td = default;
        // ;
        // TileBase[] tileArray = Tilemap.GetTilesBlock(BoundsInt);
        // for (int index = 0; index < tileArray.Length; index++)
        // {
        //     print(index + " " + tileArray[index]);
        //     // Tilemap.DeleteCells(tileArray[index].GetTileData());
        // }

        // ScriptableTile st = GameAssets.MyFirstScriptableTile;
        // int xSize = Tilemap.size.x;
        // int ySize = Tilemap.size.y;
        // int xOffset = xSize / 2;
        // int yOffset = ySize / 2;
        // // Debug.Log(ySize + " " + yOffset);
        // for (int i = 0; i < xSize; i++)
        // {
        //     for (int j = -5; j < ySize; j++)
        //     {
        //         // Debug.Log((i - xOffset) + " " + (j - yOffset) + " " + 1);
        //         Tilemap.SetTile(new Vector3Int(i - xOffset, j - yOffset, 1), st);
        //     }
        // }


        // GameObject gmp = new GameObject("_MAX_");
        // gmp.transform.position = Tilemap.cellBounds.max;
        // print(Tilemap.cellBounds.min);
        // GameObject gmmin = new GameObject("_MIN_");
        // gmmin.transform.position = Tilemap.cellBounds.min;
        //
        // print("Tilemap.cellBounds.size " + Tilemap.cellBounds.size);
        // print("Tilemap.cellBounds.min " + Tilemap.cellBounds.min);
        // print("Tilemap.cellBounds.max " + Tilemap.cellBounds.max);
        // print("Tilemap.cellSize " + Tilemap.cellSize);


       
        

        // Tilemap.SetTile(new Vector3Int(0, 0), st);
        // print(Tilemap.GetTile(new Vector3Int(0, 0)));
        // Tilemap.SetTile(new Vector3Int(1, 1), st);
        // print(Tilemap.GetTile(new Vector3Int(1, 1)));
        // Tilemap.SetTile(new Vector3Int(1, 0), st);
        // print(Tilemap.GetTile(new Vector3Int(1, 0)));
        // Tilemap.SetTile(new Vector3Int(0, 1), st);
        // print(Tilemap.GetTile(new Vector3Int(0, 1)));
        // EditorApplication.isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(HudCamera.rect.max + " " + HudCamera.rect.center + " " + HudCamera.transform.position);
        // print(HudCamera.ViewportToWorldPoint(HudCamera.rect.min) + " " + HudCamera.ViewportToWorldPoint(HudCamera.rect.max));
        // print(Tilemap.cellBounds.position);
        // print(Tilemap.cellBounds.center);
        // print(Tilemap.cellBounds.max);

        Vector3 leftBottom = HudCamera.ViewportToWorldPoint(HudCamera.rect.min);
        Vector3 rightUpper = HudCamera.ViewportToWorldPoint(HudCamera.rect.max);
        
        Vector3 cellSize = Tilemap.cellSize;
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
                    // print(Tilemap.cellBounds.min.y);
                    Vector3Int tilePosition = new Vector3Int(x, y) - leftBottomCorner;
                    // float yCoord = tilePosition.y * cellSize.y - leftBottomCorner.y;
                    float yCoord = tilePosition.y * cellSize.y;

                    if (leftBottom.y - 1 <= yCoord && rightUpper.y > yCoord)
                    {
                        if (!Tilemap.HasTile(tilePosition))
                        {
                            // print("tilePosition " + tilePosition);
                            // print("leftBottom " + leftBottom);
                            // print("rightUpper " + rightUpper);

                            Tilemap.SetTile(tilePosition, st);
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

        Vector3 playerPos = KnightCoxswain.transform.position;
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
