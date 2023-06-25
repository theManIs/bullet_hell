using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MultiplayerController : MonoBehaviour
{
    public static event Action OnServer;

    private float _connectingTimer = 0;
    private float _connectingTimerDefault = 0;
    private readonly float _msToSec = 0.001f;

    private PickingCharacterScreen _pcs;

    // Start is called before the first frame update
    void Start()
    {
        //StartHost();
        //_pcs = FindObjectOfType<PickingCharacterScreen>();
        //Debug.Log(FindObjectOfType<PickingCharacterScreen>());
        UnityTransport ut =  GetComponent<UnityTransport>();
        _connectingTimerDefault = ut.ConnectTimeoutMS * ut.MaxConnectAttempts * _msToSec;
    }

    // Update is called once per frame
    void Update()
    {
        if (_connectingTimer > 0)
        {
            _connectingTimer -= Time.deltaTime;

            if (_connectingTimer <= 0)
            {
                
            }
        }
    }

    private void ResetTimer() => _connectingTimer = 0;

    private bool ConnectionInProgress() => _connectingTimer > 0;

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else if (ConnectionInProgress())
        {
            //GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Connecting: " + ((int)_connectingTimer % 4 == 0 ? "..." 
                : (int)_connectingTimer % 4 == 3 ? ".." : (int)_connectingTimer % 4 == 2 ? "." : ""));

            if (GUILayout.Button("Stop"))
            {
                //NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
                NetworkManager.Singleton.Shutdown();
                ResetTimer();
            }
                
        }
        else if (NetworkManager.Singleton.ShutdownInProgress)
        {
            GUILayout.Label("Shutting down client");
        }

        GUILayout.EndArea();
    }

    private void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) StartClient();
        if (GUILayout.Button("Server")) StartServer();
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        OnServer?.Invoke();
    }

    private void ResetAndUnsubscribe(ulong uid)
    {
        ResetTimer();
        NetworkManager.Singleton.OnClientConnectedCallback -= ResetAndUnsubscribe;
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        _connectingTimer = _connectingTimerDefault;
        NetworkManager.Singleton.OnClientConnectedCallback += ResetAndUnsubscribe;
    }
}
