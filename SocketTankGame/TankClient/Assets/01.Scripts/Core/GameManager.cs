using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _mainMap;
    [SerializeField]
    private string _connectionUrl;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        MapManager.Instance = new MapManager(_mainMap);
        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        NetworkManager.Instance.Init(_connectionUrl);
        NetworkManager.Instance.Connection();
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.Disconnect();
    }

}
