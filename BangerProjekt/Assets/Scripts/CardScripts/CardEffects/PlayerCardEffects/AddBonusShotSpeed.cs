using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddShotSpeed")]
public class AddBonusShotSpeed : CardEffect
{
	private float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect);
		Player.Instance.BonusShotSpeed += value;
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.BonusShotSpeed -= value;
	}
}
