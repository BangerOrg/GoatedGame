using System.Collections.Generic;
using UnityEngine;
using System;
using System.Dynamic;

[CreateAssetMenu(menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{

	public static Action<Item> SendItem;
	public static Action<Item, bool> ChangeItemPlayerStats;
	public static Action<GameObject> SendNewWeapon;
	public static Action NewAbility;
	[field: SerializeField] public Item[] Slots { get; set; }
	[field: SerializeField] public Item[] EquippedItems { get; set; } = new Item[(int)Enums.SlotTag.None];

	public void Init(int initSize) //Initialize the array. Necessary before usage
	{
		Slots = new Item[initSize];
	}

	public List<Item> Resize(int size) //If inventory resized by any Upgrade/item etc. Needs the Value of slots that should be there.
	{
		List<Item> overflow = new List<Item>();

		if (size < Slots.Length)
		{
			for (int i = size; i < Slots.Length; i++)
			{
				if (Slots[i] != null)
				{
					overflow.Add(Slots[i]);
				}
			}
		}
		Item[] newSlots = new Item[size];
		int itemsToCopy = Mathf.Min(Slots.Length, size);
		for (int i = 0; i < itemsToCopy; i++)//Copy to new array since Arrays are not dynamic like lists
		{
			newSlots[i] = Slots[i];
		}

		Slots = newSlots;

		return overflow; //Returns items that didn't fit into the new array if the inventory has been shrunk. Overflow needs to be handled by the Script that caused it.
	}

	public bool TryAddItem(Item newItem) //Function that returns false if the item was not able to be added to the inventory.
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			if (Slots[i] == null)
			{
				Slots[i] = newItem;
				return true;
			}
		}
		return false; // Inventory full
	}

	public void SwapSlots(int slotA, int slotB) //Self explaining (i know its inefficient because i make 2 helper Objects but who cares about these approx. 10 bytes of ram more or less)
	{
		Item slotAItem;
		Item slotBItem;

		slotAItem = Slots[slotA];
		slotBItem = Slots[slotB];

		Slots[slotB] = slotAItem;
		Slots[slotA] = slotBItem;
	}

	public Item RemoveItem(int slot) //Removes an item from a slot. Returns the item that was removed (can be ignored in cases and has been for now).
	{
		if (slot < 0 || slot >= Slots.Length)
		{
			Debug.LogError("You dingus. That slot doesn't exist.");
			return null;
		}
		Item itemToReturn = Slots[slot];
		Slots[slot] = null;
		return itemToReturn;
	}


	public void AddItemToSlot(Item newItem, int slot) //Adds an item to a specific slot unlike TryAddItem (use with caution, overwrites existing items)
	{
		if (slot < 0 || slot >= Slots.Length)
		{
			Debug.LogError("You dingus. That slot doesn't exist.");
			return;
		}
		Slots[slot] = newItem;

	}

	public void EquipItem(int slot, Enums.SlotTag tag)
	{

		Item itemFromSlot = Slots[slot];
		if (itemFromSlot != null)
			//Debug.Log("Equipping" + itemFromSlot.name);
			EquippedItems[(int)tag] = itemFromSlot;
		if (itemFromSlot is WeaponItem)
		{
			WeaponItem tempWeapon = Slots[slot] as WeaponItem; //this works man this is scuffed
			SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(itemFromSlot, true); // true because we add the stats
															   //if nothing is equipped, we equip the one we have and increase our stats accordingly
															   //this gets called when the Player has nothing equipped
		}
	}

	public void EquipItem(Item item, Enums.SlotTag tag)
	{
		EquippedItems[(int)tag] = item;
		if (item is WeaponItem)
		{
			WeaponItem tempWeapon = item as WeaponItem; //this works man this is scuffed
			SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(item, true); // true because we add the stats
													   //if nothing is equipped, we equip the one we have and increase our stats accordingly
													   //this gets called when the Player has nothing equipped
		}
		NewAbility?.Invoke();
	}

	public void UnEquipItem(Enums.SlotTag tag)
	{
		Item itemToUnequip = EquippedItems[(int)tag];
		if (itemToUnequip is WeaponItem)
		{
			SendNewWeapon?.Invoke(null); //just dont send a new weapon the PlayerScript does the magic :)
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(EquippedItems[(int)tag], false); // false because subtract the stats
		}
		EquippedItems[(int)tag] = null;
	}

	public void SwapEquippedItem(int slot, Enums.SlotTag tag) //slot 6 ability (3)
	{
		if (Slots[slot] != null)
		{
			Item itemToEquip = Slots[slot];
			Item currentlyEquipped = EquippedItems[(int)tag];

			if (currentlyEquipped != null)
			{
				UnEquipItem(tag);
			}

			EquipItem(itemToEquip, tag);
			Slots[slot] = currentlyEquipped; // Vorheriges Item geht zurück in den Slot
		}
		else if (EquippedItems[(int)tag] != null)
		{
			Slots[slot] = EquippedItems[(int)tag];
			UnEquipItem(tag);
		}
		NewAbility?.Invoke();
		for (int i = 0; i < EquippedItems.Length; i++)
		{
			Debug.Log(EquippedItems[i]);
		}

	}


	//TODO: Inventory sorting
}
