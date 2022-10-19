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
    [SerializeField]
    private ItemSO _item;

    public ItemSO Item
    {
        get => _item;
        set
        {
            _item = value;
            LoadSprite();
        }
    }

    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>().transform;
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
    }

    public void SetData(Transform parent, Vector3 rectPos)
    {
        transform.SetParent(parent);
        _rect.position = rectPos;

        // 이전에 만약에 슬롯에 있었다면 이전 슬롯에서 이녀석을 존재했음을 제거
        if (prevParent != null)
        {
            Slot slot = prevParent.GetComponent<Slot>();
            if (slot != null)
            {
                slot.RemveItem();
            }
        }


    }

    private void Start()
    {
        LoadSprite();
    }


    private async void LoadSprite()
    {
        if (_item.sprite == null)
        {
            _item.sprite = await _item._assetSprite.LoadAssetAsync().Task;
        }

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
        if (transform.parent == _canvas)
        {
            transform.SetParent(prevParent);
            _rect.position = _prevPos;
        }

        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }
}
