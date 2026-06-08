using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddDamageEffect")]
public class AddDamageEffect : CardEffect
{
	private float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect);
		Player.Instance.BonusDamage += value;
		Debug.Log("effect amount: " + value);
		Debug.Log(Player.Instance.BonusDamage);
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.BonusDamage -= value;
	}

}
