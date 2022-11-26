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

    [SerializeField]
    private TankController tankPrefab;

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

    public void CreateTank(int id, Position position)
    {
        TankController tank = Instantiate(tankPrefab);

        tank.GetComponent<NetworkObject>().id = id;

        tank.transform.position = new Vector3(position.X, position.Y, 0);
        tank.transform.rotation = Quaternion.Euler(0, 0, position.Rotate);
        tank.TankTurret.rotation = Quaternion.Euler(0, 0, position.TurretRotate);
    }

}
