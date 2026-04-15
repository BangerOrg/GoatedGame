using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas { get; set; }
    [field: SerializeField] public int MaxInventorySlots { get; set; }
    [field: SerializeField] public List<Item> InventoryItems { get; set; }
    [field: SerializeField] public GameObject ItemInInventoryPrefab { get; set; }
    [field: SerializeField] public GameObject ItemViewPrefab { get; set; }
    [field: SerializeField] public GameObject ItemSlotPrefab { get; set; }
    [field: SerializeField] public GameObject[] InventorySlots { get; set; }
    [field: SerializeField] public InventoryLogic InvLogic { get; set; }
    private Transform content;


    private void Start()
    {
        MaxInventorySlots = InventoryLogic.MaxInventorySlots;
        InvLogic = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryLogic>();
        content = this.GetComponentInChildren<GridLayoutGroup>().gameObject.transform;
        SetupInventory();
    }
    public void SetupInventory()
    {
        Debug.Log(MaxInventorySlots);
        InventoryItems = InventoryLogic.InventoryItems;
        for(int i = 0; i < MaxInventorySlots ; i++)
        {
           GameObject IS = Instantiate(ItemSlotPrefab,content);
           IS.GetComponent<ItemSlot>().SlotId = i;
        }
        int itemNum = 0;
        foreach(Item item in InventoryItems)
        {
            GameObject IIS = Instantiate(ItemInInventoryPrefab,content.GetChild(itemNum));
            IIS.GetComponent<ItemInSlot>().Item = item;
            IIS.GetComponent<Image>().sprite = item.Icon;

        }
        
    }

    


}
