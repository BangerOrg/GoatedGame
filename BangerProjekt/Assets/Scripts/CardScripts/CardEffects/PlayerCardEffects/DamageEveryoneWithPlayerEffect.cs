using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEveryoneWithPlayerEffect : CardEffect
{
	int percentageDamage;
	public override void ExecuteEffect(string effect)
	{
		percentageDamage = int.Parse(effect, info.NumberFormat);
	}

	public override void RevertEffect(string effect)
	{
		percentageDamage = int.Parse(effect, info.NumberFormat);
	}
}
