using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{
	//these obstacles make percentage damage, not absolute numbers of damage

	private int damagePerTick;
	private Obstacle Obstacle;


	void Awake()
	{
		Obstacle = GetComponent<ObstacleScript>().Obstacle;
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
		player.DamageUnit(Mathf.CeilToInt((damagePerTick / 100f) * Player.Instance.MaxHealth), 1);
	}
}
