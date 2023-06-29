using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelCreator : NetworkBehaviour
{
    private ServiceRegistry _sr;
    public NetworkLevel NetworkLevelPrefab;

    public SpriteFrequency[] TileSpriteFrequencies;
    public GameObjectFrequency[] GameObjectsFrequencies;
    public float Scale = 0.1f;
    public int StartingNoiseBlockSize = 10;
    public int RandomSeed = 100;


    // Start is called before the first frame update
    void Start()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.LevelCreator = this;
    }

    public void OnEnable()
    {
        NetworkManager.Singleton.OnServerStarted += StartLevel;
    }    
    
    public void OnDisable()
    {
        //NetworkManager.Singleton.OnServerStarted -= StartLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        NetworkLevel nl = Instantiate(NetworkLevelPrefab);

        if (nl.GetComponent<NetworkObject>() is { } localNetworkObject)
        {
            localNetworkObject.Spawn();
        }

        //print(tl.GetComponent<TestList>().m_ints.Count);
    }
    private void LevelBuilderOnServer()
    {
        _sr.LevelBuilder.OnServer();
    }
}
