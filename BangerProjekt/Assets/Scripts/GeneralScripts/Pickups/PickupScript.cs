using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
	private Pickup pickupType;
	void Awake()
	{
		pickupType = PickupManager.Instance.PickupList[Random.Range(0, PickupManager.Instance.PickupList.Count)];
		gameObject.GetComponent<SpriteRenderer>().sprite = pickupType.PickupImage;
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			foreach (Pair<CardEffect, string> pair in pickupType.PickupEffects)
			{
				pair.First.ExecuteEffect(pair.Second);

			}
			if (pickupType.doesRevert) StartCoroutine(PickupManager.WaitForRevert(pickupType.durationInSeconds, pickupType.PickupEffects));
			Destroy(gameObject);
		}
	}
}
