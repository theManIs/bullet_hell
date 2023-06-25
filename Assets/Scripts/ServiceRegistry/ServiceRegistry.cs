using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ServiceRegistry : MonoBehaviour
{
    public Grid FieldGrid;
    public Tilemap FieldTilemap;
    public NetworkLevel NetworkLevel;
    public LevelBuilder LevelBuilder;
    public TestList TestList;

    public void RegisterFieldGrid(Grid fieldGrid) => FieldGrid = fieldGrid;
    public void RegisterFieldTilemap(Tilemap fieldTilemap) => FieldTilemap = fieldTilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
