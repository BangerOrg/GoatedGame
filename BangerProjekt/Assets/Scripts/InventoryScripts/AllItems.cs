using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
	[field: SerializeField] public List<Item> Items { get; set; }
}

[CreateAssetMenu(menuName = "Inventory/AllItems")]
public class AllItemsSO : ScriptableObject
{
	public List<Item> Items;
}
