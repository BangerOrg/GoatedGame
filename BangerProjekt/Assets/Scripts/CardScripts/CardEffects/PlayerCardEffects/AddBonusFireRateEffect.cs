using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddFireRateEffect")]
public class AddBonusFireRateEffect : CardEffect
{

	float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusFireRate += value;
	}

	public override void RevertEffect()
	{
		Player.Instance.BonusFireRate -= value;
	}
}
