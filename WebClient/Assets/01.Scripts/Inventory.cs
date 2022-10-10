using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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




public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot _slotPrefab;

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

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            string data = GetSlotDataJson();
            Debug.Log(data);
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
