using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [field: SerializeField] public Item Item { get; set; }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<ScrollRect>() == null)
        {
            Debug.Log(eventData.pointerDrag.name);
            RectTransform itemTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform thisTransform = this.GetComponent<RectTransform>();
            this.Item = eventData.pointerDrag.GetComponent<ItemInSlot>()?.Item;
            itemTransform.SetParent(transform);
            itemTransform.anchoredPosition = thisTransform.anchoredPosition;

        }
    }

}
