using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerController : MonoBehaviour
{
    public static event Action OnServer;
    public static event Action OnClient;

    private PickingCharacterScreen _pcs;

    // Start is called before the first frame update
    void Start()
    {
        //StartHost();
        //_pcs = FindObjectOfType<PickingCharacterScreen>();
        //Debug.Log(FindObjectOfType<PickingCharacterScreen>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnEnable() => _pcs.OnGo += StartClient;
    //private void OnDisable() => _pcs.OnGo -= StartClient;

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) StartClient();
        if (GUILayout.Button("Server")) StartServer();
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    private static void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        OnServer?.Invoke();
    }
    private static void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        OnClient?.Invoke();
    }
}
