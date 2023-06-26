using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PersistentPlayer : NetworkBehaviour
{
    private ServiceRegistry _sr;
    public KnightCoxswain _kc;
    private float _lastXAxis;
    private float _lastYAxis;

    [Range(0, 10)]
    public float MoveSpeed = 5;

    private KnightCoxswain GetKnightCoxswain() => GameAssets.CursedKnight;

    private bool IsAnyMovement(float xAxis, float yAxis) => 
        !_lastXAxis.Equals(xAxis) || !_lastYAxis.Equals(yAxis) || xAxis.Equals(-1f) ||
        yAxis.Equals(-1f) || xAxis.Equals(1f) || yAxis.Equals(1f);

    // Start is called before the first frame update
    void Start()
    {
        _sr = FindObjectOfType<ServiceRegistry>();
        _sr.PersistentPlayer = this;

        if (IsOwner)
        {
            OnClientServerRpc();
        }
    }

    public override void OnDestroy()
    {
        Destroy(_kc.gameObject);
        base.OnDestroy();
    }

    [ServerRpc]
    private void OnClientServerRpc()
    {
        //print(_kc);
        //if (_kc == null)
        //{
            _kc = Instantiate(GetKnightCoxswain(), Vector3.zero, Quaternion.identity);
        //_kc.PerkProcessor = new PerkProcessor().Subscribe(_ppp);
        //print(_kc);

        if (_kc.GetComponent<NetworkObject>() is { } knightCoxswainNetworkObject)
        {
            knightCoxswainNetworkObject.Spawn();
            FigurePlayerInstanceClientRpc(knightCoxswainNetworkObject.NetworkObjectId);
        }
        //print(NetworkManager.Singleton.ConnectedClientsIds);
        //foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
        //{
        //    PersistentPlayer pp = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PersistentPlayer>();
            //print(pp);
            //print(pp.GetKnightsCoxswain());
            //print(NetworkManager.Singleton.IsServer);
            //print(pp.GetComponent<NetworkObject>().NetworkObjectId);
            //if (!pp.HasKnightsCoxswain())
            //{
            //    print("before set KnightCoxswain " +_kc);
            //    pp.SetKnightsCoxswain(_kc);
            //}
    //}

        //}
    }

    [ClientRpc]
    private void FigurePlayerInstanceClientRpc(ulong networkId)
    {
        //print(networkId);
        foreach (KnightCoxswain knightCoxswain in FindObjectsOfType<KnightCoxswain>())
        {
            if (knightCoxswain.GetComponent<NetworkObject>() is { } knightCoxswainNetworkObject)
            {
                if (knightCoxswainNetworkObject.NetworkObjectId == networkId)
                {
                    //print(networkId + " " + (knightCoxswainNetworkObject.NetworkObjectId == networkId));
                    _kc = knightCoxswain;
                    _sr.KnightCoxswain = _kc;
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            InterceptKeyInput();
        }
    }

    private void InterceptKeyInput()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        if (IsAnyMovement(xAxis, yAxis))
        {
            _lastXAxis = xAxis;
            _lastYAxis = yAxis;
            //print("InterceptKeyInput IsAnyMovement");

            MovePlayerLocal(xAxis, yAxis);
            MoveCameraForPlayerLocal();
        }
    }

    private void MovePlayerLocal(float xAxis, float yAxis)
    {
        //print(_kc);
        MovePlayerServerRpc(xAxis, yAxis);

        if (_kc.GetComponent<SpriteRenderer>() is { } knightCoxswainSpriteRenderer)
        {
            if (!xAxis.Equals(0f))
            {
                knightCoxswainSpriteRenderer.flipX = !(xAxis > 0);
            }
        }
    }

    private void MoveCameraForPlayerLocal()
    {
        //float xAxis = Input.GetAxis("Horizontal");
        //float yAxis = Input.GetAxis("Vertical");

        //if (IsAnyMovement(xAxis, yAxis))
        //{
        //print(_kc);
        Vector3 playerPos = _kc.transform.position;
        playerPos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = playerPos;
        //print(playerPos + "client");

        //_lastXAxis = xAxis;
        //_lastYAxis = yAxis;

        //MoveCameraForPlayerServerRpc();
        //}
    }

    [ServerRpc]
    private void MoveCameraForPlayerServerRpc()
    {
        Vector3 playerPos = _kc.transform.position;
        playerPos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = playerPos;
        //Debug.Log(playerPos + "server" + " " + _kc);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(float xAxis, float yAxis)
    {
        if (!xAxis.Equals(0) || !yAxis.Equals(0))
        {
            _kc.transform.position += Vector3.right * Time.deltaTime * MoveSpeed * xAxis +
                                      Vector3.up * Time.deltaTime * MoveSpeed * yAxis;

            if (_kc.GetComponent<SpriteRenderer>() is { } knightCoxswainSpriteRenderer)
            {
                if (!xAxis.Equals(0f))
                {
                    knightCoxswainSpriteRenderer.flipX = !(xAxis > 0);
                    _kc.LastDirectionLeft = !(xAxis > 0);
                }
            }

            if (_kc.GetComponent<SpriteRendererEffectAdder>() is { } knightCoxswainSpriteRendererEffectAdder)
            {
                knightCoxswainSpriteRendererEffectAdder.Move();
            }
        }
        else
        {
            if (_kc.GetComponent<SpriteRendererEffectAdder>() is { } knightCoxswainSpriteRendererEffectAdder)
            {
                knightCoxswainSpriteRendererEffectAdder.Stop();
            }
        }
    }
}
