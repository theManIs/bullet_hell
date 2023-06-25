using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestList : NetworkBehaviour
{
    public NetworkList<int> m_ints;
    public NetworkList<SmartTile> SmartThings;
    private ServiceRegistry _sr;

    private void Awake()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.TestList = this;
        m_ints = new NetworkList<int>();
        SmartThings = new NetworkList<SmartTile>();
        
        ListenChanges();
    }

    // Call this is in Awake or Start to subscribe to changes of the NetworkList.
    void ListenChanges()
    {
        m_ints.OnListChanged += OnIntChanged;
    }    
    
    // Call this is in Awake or Start to subscribe to changes of the NetworkList.
    public void AddElement(int integer)
    {
        m_ints.Add(integer);
    }

    // The NetworkListEvent has information about the operation and index of the change.
    void OnIntChanged(NetworkListEvent<int> changeEvent)
    {

    }



    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
