using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupCategory
{
    Register = 0,
    Login = 1
}

public class PopupPanel : MonoBehaviour
{
    public static PopupPanel Inst;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        Inst = this;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenPopup(PopupCategory category)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void ClosePopup()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}
