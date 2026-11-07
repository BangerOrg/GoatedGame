using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionBullet : PlayerBullet
{

	public int Damage { get; set; }


	public void Init(int Damage)
	{
		this.Damage = Damage;
	}

	public override void DamageCalculation(GameObject currObject, bool isLifestealable)
	{
		if (isLifestealable) LifeStealCalculate();
		//this cant crit so crit is out
		int totalDamage = (int)(Damage * Player.Instance.BonusDamage);
		if (totalDamage <= 0) totalDamage = 1;
		currObject.GetComponent<Unit>().DamageUnit(totalDamage, 1);
		RemainingPierce--; //reduce the pierce
		if (RemainingPierce <= 0)
		{
			Destroy(gameObject);
		}
	}
}
