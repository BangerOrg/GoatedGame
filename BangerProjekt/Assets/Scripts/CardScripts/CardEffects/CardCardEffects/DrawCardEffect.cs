using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/DrawCardEffect")]
public class DrawCardEffect : CardEffect
{

	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		DeckLogic.Instance.DrawCards(value);
	}

	public override void RevertEffect()
	{
		//literally nothing
	}
}
