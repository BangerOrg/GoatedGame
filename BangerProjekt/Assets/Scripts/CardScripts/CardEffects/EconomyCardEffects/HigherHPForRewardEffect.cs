using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/HigherHPForRewardEffect")]

public class HigherHPForRewardEffect : CardEffect
{
	float hpIncrease;
	int creditReward;
	public override void ExecuteEffect(string effect)
	{
		string[] strings = effect.Split(";");
		hpIncrease = float.Parse(strings[0], info.NumberFormat);
		creditReward = int.Parse(strings[1], info.NumberFormat);

		GameManager.EnemyHealthModifier += hpIncrease;
	}

	public override void RevertEffect(string effect)
	{
		string[] strings = effect.Split(";");
		hpIncrease = float.Parse(strings[0], info.NumberFormat);
		creditReward = int.Parse(strings[1], info.NumberFormat);
		GameManager.EnemyHealthModifier -= hpIncrease;
		GameManager.Instance.ChangeCredits(creditReward * (LayerManager.CurrentLayerNumber - 1));
	}
}
