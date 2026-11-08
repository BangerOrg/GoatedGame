using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDamagingObstacle : DamagingObstacle
{
	private int damageCount = 0;

	private int totalTicks;

	public override void Start()
	{
		base.Start();
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
