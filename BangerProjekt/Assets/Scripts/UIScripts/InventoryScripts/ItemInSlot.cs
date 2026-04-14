using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemInSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [field:SerializeField]public Item Item{get; set;}
    private Image image;
    private RectTransform rectTransform;
    protected Transform parentBeforeDrag;
    private CanvasGroup canvasGroup;
    [field:SerializeField] private Canvas canvas;

    void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<InventoryScript>().Canvas;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;
        Debug.Log(parentBeforeDrag.gameObject.name);
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Debug.Log(parentBeforeDrag.gameObject.name);
        if (!eventData.pointerCurrentRaycast.gameObject || !eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
        {
            transform.SetParent(parentBeforeDrag);
            rectTransform.anchoredPosition = parentBeforeDrag.position;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
         rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked");
	}
}
