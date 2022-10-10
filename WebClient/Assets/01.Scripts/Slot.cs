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
    private ItemUI _slotItem;
    public int slotNumber = 0; 
    public ItemSO SlotItem => _slotItem != null ? _slotItem._item : null;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
    }


    public void SetItem(GameObject target)
    {
        _hasItem = true;
        _slotItem = target.GetComponent<ItemUI>();

        target.transform.SetParent(transform);
        target.GetComponent<RectTransform>().position = _rect.position;

        if(_slotItem.prevParent != null)
        {
            Slot slot = _slotItem.prevParent.GetComponent<Slot>();

            slot?.RemveItem();
        }
    }

    public void RemveItem()
    {
        _hasItem = false;
        _slotItem = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        _image.color = Color.white;

        if (eventData.pointerDrag != null && _hasItem == false)
        {
            SetItem(eventData.pointerDrag);

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
