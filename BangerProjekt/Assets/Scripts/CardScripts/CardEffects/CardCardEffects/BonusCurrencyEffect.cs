using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/BonusCurrencyEffect")]
public class BonusCurrencyEffect : CardEffect
{

	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		DeckLogic.Instance.CurrencyAmount += value;
	}

	public override void RevertEffect()
	{
		//literally nothing
	}
}
