using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDamagingObstacle : ObstacleScript
{
	private int damageCount = 0;

	private int totalTicks;


	public void Setup(Obstacle obstacle)
	{
		Obstacle = obstacle;
		base.Setup();
	}
	public void Start()
	{
		totalTicks = Obstacle.HP;
	}
	public override void DamagePlayer()
	{
		if (damageCount < totalTicks)
		{
			base.DamagePlayer();
			damageCount++;
		}
	}
}
