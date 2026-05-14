using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddBonusSpeedEffect")]
public class AddBonusSpeedEffect : CardEffect
{
	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		Player.Instance.InitialMoveSpeed += value;
		Player.Instance.MoveSpeed += value;
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.InitialMoveSpeed -= value;
		Player.Instance.MoveSpeed -= value;
	}
}
