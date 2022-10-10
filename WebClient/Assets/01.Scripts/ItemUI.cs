using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _canvas;
    public Transform prevParent;
    private RectTransform _rect;
    private CanvasGroup _canvasGroup;
    private Vector3 _prevPos;

    private Image _image;
    public ItemSO _item;

    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>().transform;
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image.sprite = _item.sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevParent = transform.parent;
        _prevPos = _rect.position;

        transform.SetParent(_canvas);
        transform.SetAsLastSibling();

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == _canvas)
        {
            transform.SetParent(prevParent);
            _rect.position = _prevPos;
        }

        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }
}
