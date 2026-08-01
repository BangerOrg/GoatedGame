using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CardEffect/AddBonusChestEffect")]
public class BonusChestEffect : CardEffect
{
	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		LootChest.MinCredits += value;
		LootChest.MaxCredits += value;
	}

	public override void RevertEffect(string effect)
	{
		LootChest.MinCredits -= value;
		LootChest.MaxCredits -= value;
	}
}
