using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/DamageEveryoneWithPlayerEffect")]
public class DamageEveryoneWithPlayerEffect : CardEffect
{
	int percentageDamage;

	bool RoomCleared;
	public override void ExecuteEffect(string effect)
	{
		percentageDamage = int.Parse(effect, info.NumberFormat);
		RoomScript.StartWaves += _ => StartDamage(effect); //to throw away the parameter (hahaha flutter be like)(those who know)
		RoomScript.RoomCleared += StopDamage;
		RoomCleared = false;
	}

	public void DamageEveryone(string effect)
	{
		if (RoomCleared) return;
		percentageDamage = int.Parse(effect, info.NumberFormat); //we need this if there are multiple of those going on
		Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
		int playerDamageTaken = Mathf.RoundToInt((percentageDamage / 100f) * player.MaxHealth);
		List<GameObject> enemiesAlive = new List<GameObject>(EnemySpawner.Instance.AliveEnemies);
		foreach (GameObject enemyObject in enemiesAlive)
		{
			Unit enemyScript = enemyObject.GetComponent<Unit>();
			enemyScript.DamageUnit(playerDamageTaken, 1);
			//every enemy
		}
		if (player.CalculateTotalDamageTaken(playerDamageTaken) > 0) //only if we survive
		{
			player.DamageUnit(playerDamageTaken, 1);
		}
		GameManager.Instance.ExecuteCoroutine(DamageCooldown(effect));
	}

	public void StartDamage(string effect)
	{
		RoomCleared = false;
		DamageEveryone(effect);
	}
	public void StopDamage()
	{
		RoomCleared = true;
	}

	public IEnumerator DamageCooldown(string effect)
	{
		yield return new WaitForSeconds(GameManager.STANDARD_TICK_TIME * 5); //5 ticks sounds reasonable
		DamageEveryone(effect);
	}

	public override void RevertEffect(string effect)
	{
		percentageDamage = int.Parse(effect, info.NumberFormat);
	}
}
