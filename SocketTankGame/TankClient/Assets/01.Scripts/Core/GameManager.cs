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

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
            worldPos.z = 0;

            Vector3Int tilePos = MapManager.Instance.GetTilePos(worldPos);
            MapCategory mc = MapManager.Instance.GetTileCategory(tilePos);

            Debug.Log(tilePos);
            Debug.Log(mc);

            C_Pos cPos = new C_Pos { X = tilePos.x, Y = tilePos.y };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CPos, cPos);

        }
    }
}
