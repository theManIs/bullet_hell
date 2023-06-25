using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelCreator : NetworkBehaviour
{
    private ServiceRegistry _sr;
    public TestList TestList;

    // Start is called before the first frame update
    void Start()
    {
        
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
        NetworkLevel nl = Instantiate(GameAssets.NetworkLevel);

        if (nl.GetComponent<NetworkObject>() is { } localNetworkObject)
        {
            localNetworkObject.Spawn();
        }

        GameObject tl = Instantiate(TestList.gameObject);

        if (tl.GetComponent<NetworkObject>() is { } tlNetworkObject)
        {
            tlNetworkObject.Spawn();
        }

        for (int i = 0; i < 10; i++)
        {
            tl.GetComponent<TestList>().AddElement(i);
        }

        //print(tl.GetComponent<TestList>().m_ints.Count);
    }
    private void LevelBuilderOnServer()
    {
        _sr.LevelBuilder.OnServer();
    }
}
