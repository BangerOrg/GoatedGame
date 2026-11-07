using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDamagingObstacle : DamagingObstacle
{
	private int damageCount = 0;

	private Obstacle Obstacle;
	public int TotalTicks
	{
		get
		{
			return Obstacle.HP;
		}
		private set
		{
			Obstacle.HP = value;
		}
	}

	void Awake()
	{
		Obstacle = GetComponent<ObstacleScript>().Obstacle;
	}

	public override void DamagePlayer()
	{
		if (damageCount < TotalTicks)
		{
			base.DamagePlayer();
			damageCount++;
		}
	}
}
