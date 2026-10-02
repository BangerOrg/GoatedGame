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
		/*for (int i = 0; i < ItemsEquipped.Length; i++)
		{
			ItemsEquipped[i] = null;
			//reset all items, after that we can load them from save
		}*/
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
		//ItemsEquipped = new Item[(int)Enums.SlotTag.None];
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

	/*public static void EquipItem(Item itemToEquip)
	{
		ItemsEquipped[(int)itemToEquip.ItemTag] = itemToEquip;
		if (itemToEquip is WeaponItem)
		{
			WeaponItem tempWeapon = itemToEquip as WeaponItem; //this works man this is scuffed
			SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)itemToEquip.ItemTag], true); // true because we add the stats
																						  //if nothing is equipped, we equip the one we have and increase our stats accordingly
																						  //this gets called when the Player has nothing equipped
		}
	}

	public static void UnEquipItem(int tagOfItemInt)
	{
		Item itemToUnequip = ItemsEquipped[tagOfItemInt];
		if (itemToUnequip is WeaponItem)
		{
			SendNewWeapon?.Invoke(null); //just dont send a new weapon the PlayerScript does the magic :)
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(ItemsEquipped[tagOfItemInt], false); // false because subtract the stats
		}
		ItemsEquipped[tagOfItemInt] = null;
	}*/

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
