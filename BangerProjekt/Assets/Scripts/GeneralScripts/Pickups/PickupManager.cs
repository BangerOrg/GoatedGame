using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	public static PickupManager Instance;
	[field: SerializeField] public int DropChance;
	[SerializeField] private GameObject pickupPrefab;

	[field: SerializeField] public List<Pickup> PickupList;


	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
	}
	void OnEnable()
	{
		Enemy.enemyDies += CheckForPickupDrop;
	}

	void OnDisable()
	{
		Enemy.enemyDies -= CheckForPickupDrop;
	}


	void CheckForPickupDrop(GameObject droppingEnemy)
	{
		int doesDrop = Random.Range(1, 101); //between 1,100
		if (doesDrop <= DropChance)
		{
			Instantiate(pickupPrefab, droppingEnemy.transform.position, Quaternion.identity);
			//drop a Pickup;
		}
	}

	public static IEnumerator WaitForRevert(float duration, List<Pair<CardEffect, string>> effectsToRevert)
	{
		yield return new WaitForSeconds(duration);
		foreach (Pair<CardEffect, string> pair in effectsToRevert)
		{
			pair.First.RevertEffect(pair.Second);
		}
	}
}


