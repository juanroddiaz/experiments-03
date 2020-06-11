using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnTapEventListener : MonoBehaviour, IPointerClickHandler
{
    private Action _onClick;

    public void Initialize(Action onClick)
    {
        _onClick = onClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        _onClick?.Invoke();
    }
}
