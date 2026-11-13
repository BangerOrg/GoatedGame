using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : Enemy
{
	//Damage for this enemy decides contact damage (which will almost never happen) and explosion damage
	[SerializeField] private float distanceForKamikaze; //how far the enemy has to be for the explosion to start

	[SerializeField] private float timeUntilKamikaze; //the time the player has to react until the enemy actually explodes

	private GameObject rangeIndicatorObject; //this is the grey circle that shows the range of the explosion
	private GameObject redIndicator; //this is he growing red circle

	private float indicatorGrowPerTick;
	[SerializeField] private float kamikazeSize;

	private bool isKamikazing = false;

	new void Awake()
	{
		base.Awake();
		rangeIndicatorObject = gameObject.transform.GetChild(0).gameObject;
		redIndicator = gameObject.transform.GetChild(1).gameObject;
		rangeIndicatorObject.SetActive(false);
		redIndicator.SetActive(false);
	}

	new void Start()
	{
		base.Start();
		InvokeRepeating("CheckForKamikaze", 0, GameManager.STANDARD_TICK_TIME);
	}

	public void CheckForKamikaze()
	{
		if (Distance <= distanceForKamikaze)
		{
			StartKamikaze();
		}
	}

	public void GrowKamikaze()
	{
		if (isKamikazing)
		{
			redIndicator.transform.localScale = new Vector3(redIndicator.transform.localScale.x + indicatorGrowPerTick, redIndicator.transform.localScale.y + indicatorGrowPerTick, 1);
		}
	}
	public void StartKamikaze()
	{
		if (!isKamikazing)
		{
			isKamikazing = true;
			rangeIndicatorObject.SetActive(true); //set the Rangeindicator for the Bomboclat true
			redIndicator.SetActive(true);
			indicatorGrowPerTick = kamikazeSize / (timeUntilKamikaze * 10f); //*10 since 10 ticks to grow per sec
			StartCoroutine(WaitForKamikaze());
			MoveSpeed = 1.25f;
			CancelInvoke("TurnToPlayer");
			InvokeRepeating("GrowKamikaze", 0, 0.1f);
		}
	}

	public void Kamikaze()
	{
		//Explode
		if (gameObject.transform.GetChild(0).GetComponent<Collider2D>().IsTouching(GameObject.FindWithTag("Player").GetComponent<Collider2D>()))
		{
			//if Player and our Collider is Touching, big ouch
			playerScript.DamageUnit(Damage, 1);
		}
		enemyDies?.Invoke(gameObject); //counts As Dying
		Destroy(gameObject);
	}

	public IEnumerator WaitForKamikaze()
	{
		yield return new WaitForSeconds(timeUntilKamikaze);
		Kamikaze();

	}

}
