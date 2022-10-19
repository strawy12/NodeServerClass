using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class SlotVO
{
    public bool isEmpty;
    public int slotNumber;
    public int code;
    //¼ö·®
}

[System.Serializable]
public class InventoryVO
{
    public int count;
    public List<SlotVO> list;
}

[Serializable]
public class ReturnMsg
{
    public string msg;
    public string data;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot _slotPrefab;
    [SerializeField] private ItemUI _itemPrefab;

    private Dictionary<int, ItemSO> _itemDict = new Dictionary<int, ItemSO>();
    [SerializeField] private List<ItemSO> _itemList;

    private Dictionary<int, Slot> _slotDict = new Dictionary<int, Slot>();


    private void Awake()
    {
        foreach (ItemSO so in _itemList)
        {
            _itemDict[so.code] = so;
        }

        for (int i = 0; i < 8 * 7; i++)
        {
            Slot slot = Instantiate(_slotPrefab, transform);
            slot.slotNumber = i;
            _slotDict[i] = slot;
        }
    }

    private void Start()
    {
        LoadInven();
    }

    public void SaveInven()
    {
        InventoryVO saveData = new InventoryVO();
        saveData.list = new List<SlotVO>();

        foreach (int key in _slotDict.Keys)
        {
            Slot slot = _slotDict[key];
            if (slot.SlotItem == null) continue;

            SlotVO vo = new SlotVO
            {
                code = slot.SlotItem.code,
                slotNumber = slot.slotNumber
            };

            saveData.list.Add(vo);
        }

        saveData.count = saveData.list.Count;

        string data = JsonUtility.ToJson(saveData);
        DataManager.Inst.SaveData(data, "/inven", (json, success) =>
        {
            Debug.Log(json);
        });
    }

    public void LoadInven()
    {
        DataManager.Inst.LoadData("/inven", (json, success) =>
        {
            if (success)
            {
                ReturnMsg msg = JsonUtility.FromJson<ReturnMsg>(json);
                
                if(!string.IsNullOrEmpty(msg.data)) 
                {
                    InventoryVO invenVO = JsonUtility.FromJson<InventoryVO>(msg.data);

                    foreach (SlotVO slot in invenVO.list)
                    {
                        _slotDict[slot.slotNumber].RemveItem();

                        ItemUI item = Instantiate(_itemPrefab);
                        item.Item = _itemDict[slot.code];
                        Debug.Log(item);

                        _slotDict[slot.slotNumber].SetItem(item);

                    }
                }
            }
        });
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SaveInven();
        }
    }

}
