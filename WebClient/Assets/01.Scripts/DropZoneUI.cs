using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropZoneUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image _image ;
    private RectTransform _rect;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        _image.color = Color.white;
        if(eventData.pointerDrag != null)
        {
            GameObject target = eventData.pointerDrag;
            target.transform.SetParent(transform);
            target.GetComponent<ItemUI>().SetData(transform, eventData.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
    }
}
