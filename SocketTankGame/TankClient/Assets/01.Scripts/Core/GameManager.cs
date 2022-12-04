using Cinemachine;
using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _mainMap;
    [SerializeField]
    private string _connectionUrl;

    [SerializeField]
    private TankController tankPrefab;

    private TankController _player = null;
    public TankController Player => _player;

    private CinemachineVirtualCamera _cmVcam;

    public static GameManager Instance;

    [SerializeField]
    private List<PoolableMono> _prefabs;
    private Dictionary<int, Bullet> _activeBullet;


    private void Awake()
    {
        Instance = this;
        MapManager.Instance = new MapManager(_mainMap);
        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        NetworkManager.Instance.Init(_connectionUrl);
        NetworkManager.Instance.Connection();

        TankManager.Instance = new TankManager();

        _activeBullet = new Dictionary<int, Bullet>();
        PoolManager.Instance = new PoolManager(transform);

        foreach(var prefab in _prefabs)
        {
            PoolManager.Instance.CreatePool(prefab);
        }

        _cmVcam = GameObject.Find("FollowCam").GetComponent<CinemachineVirtualCamera>();
    }

    public void AddActiveBullet(int key, Bullet value)
    {
        _activeBullet.Add(key, value);
    }

    public void RemoveActiveBullet(int key)
    {
        Bullet b = _activeBullet[key];
        _activeBullet.Remove(key);
        PoolManager.Instance.Push(b);
    }

    public Bullet GetActiveBullet(int key)
    {
        return _activeBullet[key];
    }

    public TankController SpawnTank(Vector3 pos, int playerId, bool isPlayer = false)
    {
        TankController tank = Instantiate(tankPrefab, pos, Quaternion.identity);
        tank.SetUp(isPlayer, playerId);

        if(isPlayer)
        {
            _player = tank;
            _cmVcam.m_Follow = _player.transform;
        }

        else
        {
            TankManager.Instance.AddRemoteTank(tank);
        }

        return tank;
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.Disconnect();
    }

}
