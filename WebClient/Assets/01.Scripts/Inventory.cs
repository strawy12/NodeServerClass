using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SlotVO
{
    public bool isEmpty;
    public int slotNumber;
    public int code;
    //수량
}

[System.Serializable]
public class InventoryVO
{
    public int count;
    public List<SlotVO> list;
}

[System.Serializable]
public class InventoriesJson
{
    public int user_id;
    public string json;
}

[System.Serializable]
public class ReturnMsg
{
    public string msg;
    public List<InventoriesJson> data;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot _slotPrefab;
    [SerializeField] private List<ItemUI> _itemList;

    private List<Slot> _inventoryList = new List<Slot>();
    private void Awake()
    {
        for(int i = 0; i < 8 * 7; i++)
        {
            Slot slot  = Instantiate(_slotPrefab, transform);
            slot.slotNumber = i;

            _inventoryList.Add(slot);
        }
    }

    private void Start()
    {
        StartCoroutine(GetInventoryData());
        // 서버로부터 데이터를 받아서 파싱한 다음에 
        // 그거에 맞게 슬롯에 넣음
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            string data = GetSlotDataJson();

            InventoriesJson inventoriesData = new InventoriesJson { user_id = 1, json = data };
            StartCoroutine(PostSlotDataJson(inventoriesData));
            //이 데이터와 유저 ID(1)와 함께 서버에 전송
            //{user_id:1,json:data}
            //post로 쏘면 된다.
        }
    }

    IEnumerator PostSlotDataJson(InventoriesJson jsonData)
    {
        string data = JsonUtility.ToJson(jsonData);
        UnityWebRequest req = UnityWebRequest.Post("http://localhost:50000/insert/inventories", "POST");
        byte[] dataByte = new System.Text.UTF8Encoding().GetBytes(data);
        req.uploadHandler = new UploadHandlerRaw(dataByte);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-type", "application/json");

        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(req.downloadHandler.text);
        }

        else
        {
            Debug.LogError("에러");
        }
    }

    IEnumerator GetInventoryData()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://localhost:50000/record/inventories");
       
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            ReturnMsg returnMsg = JsonUtility.FromJson<ReturnMsg>(data);

            InventoriesJson inventories = returnMsg.data.Find(x => x.user_id == 1);
            InventoryVO inventoryData = JsonUtility.FromJson<InventoryVO>(inventories.json);

            foreach(SlotVO slotData in inventoryData.list)
            {
                if (slotData.isEmpty) continue;

                Slot slot = _inventoryList.Find(x => x.slotNumber == slotData.slotNumber);
                ItemUI item = _itemList.Find(x => x._item.code == slotData.code);

                slot.SetItem(item.gameObject);
            }
        }

        else
        {
            Debug.LogError("에러");
        }
    }

    private string GetSlotDataJson()
    {
        InventoryVO sendData = new InventoryVO();
        sendData.list = new List<SlotVO>();

        foreach(Slot slot in _inventoryList)
        {
            ItemSO so = slot.SlotItem;
            if (so != null)
            {
                SlotVO vo = new SlotVO {code = so.code, isEmpty = false, slotNumber = slot.slotNumber };
                sendData.list.Add(vo);
            }
        }

        sendData.count = sendData.list.Count;

        string json = JsonUtility.ToJson(sendData);
        return json;
    }
}
