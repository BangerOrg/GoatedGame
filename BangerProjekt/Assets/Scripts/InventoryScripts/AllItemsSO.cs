using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/AllItems")]
public class AllItemsSO : ScriptableObject
{
	public List<Item> Items;
}
