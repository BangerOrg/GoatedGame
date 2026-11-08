using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{
	//these obstacles make percentage damage, not absolute numbers of damage

	protected int damagePerTick;
	protected Obstacle Obstacle;




	public virtual void Start()
	{
		Obstacle = GetComponent<ObstacleScript>().Obstacle;
		damagePerTick = Obstacle.ContactDamage;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && damagePerTick != 0)
		{
			InvokeRepeating("DamagePlayer", 0, 0.2f);
		}
	}


	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && damagePerTick != 0)
		{
			CancelInvoke("DamagePlayer");
		}
	}

	public virtual void DamagePlayer()
	{
		Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
		if (damagePerTick < 0)
		{
			player.HealUnit(Mathf.CeilToInt(-1 * (damagePerTick / 100f) * Player.Instance.MaxHealth));
			//we do * -1 so we do not heal a negative amount (-> damage)
		}
		else
		{
			player.DamageUnit(Mathf.CeilToInt((damagePerTick / 100f) * Player.Instance.MaxHealth), 1);

		}
	}
}
