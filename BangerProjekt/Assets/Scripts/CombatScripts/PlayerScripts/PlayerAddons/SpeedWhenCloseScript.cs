using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedWhenCloseScript : MonoBehaviour
{

	private float radius;
	private float speedMultiplier;


	bool givesSpeed = false;



	void Awake()
	{
		InvokeRepeating("CheckForEnemiesInRange", 1, 0.2f);
	}
	public void Init(float radius, float speedMultiplier)
	{
		this.radius = radius;
		this.speedMultiplier = speedMultiplier;

	}

	public void CheckForEnemiesInRange()
	{
		bool shouldGiveSpeed = false;

		Collider2D[] collisionObjects = Physics2D.OverlapCircleAll(transform.position, radius);

		foreach (Collider2D col in collisionObjects)
		{
			if (col.CompareTag("Enemy"))
			{
				shouldGiveSpeed = true;
				break;
			}
		}
		if (shouldGiveSpeed && !givesSpeed)
		{
			Player.Instance.BonusMoveSpeed += speedMultiplier;
			givesSpeed = true;
		}
		else if (!shouldGiveSpeed && givesSpeed)
		{
			Player.Instance.BonusMoveSpeed -= speedMultiplier;
			givesSpeed = false;
		}
	}


	void OnDestroy()
	{
		if (givesSpeed && Player.Instance)
		{
			Player.Instance.BonusMoveSpeed -= speedMultiplier;
		}
	}
}
