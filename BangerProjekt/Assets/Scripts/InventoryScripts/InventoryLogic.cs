using System;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
	//[field: SerializeField] public static Item[] ItemsEquipped { get; set; } = new Item[(int)Enums.SlotTag.None]; //Serialized for testing
	public const int STANDARD_INVENTORY_SLOTS = 18;
	[field: SerializeField] public static int InventorySlots { get; set; } = STANDARD_INVENTORY_SLOTS; //amount of slots in the inv
	[field: SerializeField] public Inventory InventoryBlueprint { get; set; }
	public static Inventory ActiveInventory;
	public static Action<Item> SendItem;
	public static Action<Item, bool> ChangeItemPlayerStats;
	public static Action<GameObject> SendNewWeapon;
	public static InventoryLogic Instance;
	public AllItems AllItemList { get; private set; }


	private void Awake()
	{
		ActiveInventory = Instantiate(InventoryBlueprint);
		AllItemList = gameObject.GetComponent<AllItems>();
	}

	void Start()
	{
		ActiveInventory.Init(InventorySlots);
	}
	private void OnEnable()
	{
		SaveManager.SavingGame += SaveInventory;
		SaveManager.LoadingGame += LoadInventory;
		ShopHover.purchaseItem += ObtainItem;
	}

	private void OnDisable()
	{
		SaveManager.SavingGame -= SaveInventory;
		SaveManager.LoadingGame -= LoadInventory;
		ShopHover.purchaseItem -= ObtainItem;
	}

	public void OnDestroy()
	{
		Instance = null;
		InventorySlots = STANDARD_INVENTORY_SLOTS;
		ActiveInventory = null;
	}

	public static void ObtainItem(Item itemToGet)
	{
		if (!ActiveInventory.TryAddItem(itemToGet))
		{
			InventoryScript.Instance.DropItem(itemToGet);
		}
	}


	private void SaveInventory()
	{
		SaveManager.currentSave.InventoryItems = ActiveInventory.Slots;
		SaveManager.currentSave.EquippedItems = ActiveInventory.EquippedItems;
	}

	private void LoadInventory()
	{
		ActiveInventory.Slots = SaveManager.currentSave.InventoryItems;

		foreach (Item item in SaveManager.currentSave.EquippedItems)
		{
			if (item != null)
			{
				ActiveInventory.EquipItem(item, item.ItemTag);
			}

		}
	}
}
