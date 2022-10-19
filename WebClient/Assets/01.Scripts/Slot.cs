using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image _image;
    private RectTransform _rect;
    private bool _hasItem = false;

    public ItemUI slotItem;
    public int slotNumber = 0; 
    public ItemSO SlotItem
    {
        get => slotItem != null ? slotItem.Item : null;
    } 

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
    }




    public void RemveItem()
    {
        _hasItem = false;
        slotItem = null;
    }
    public void SetItem(ItemUI item)
    {
        slotItem = item;
        _hasItem = true;
        slotItem.SetData(transform, _rect.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        _image.color = Color.white;


        if (eventData.pointerDrag != null && _hasItem == false)
        {
            GameObject target = eventData.pointerDrag;

            slotItem = target.GetComponent<ItemUI>();
            if (slotItem == null) return;
            slotItem.SetData(transform, _rect.position);
            _hasItem = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = _hasItem ? Color.red : Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
    }

}
