using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager Instance;
	[SerializeField] private GameObject fistPrefab;
	public Weapon WeaponScript { get; private set; }

	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
	}

	void OnEnable()
	{
		Inventory.SendNewWeapon += NewWeapon;
		Inventory.ChangeItemPlayerStats += ChangeItemStats;
	}

	void OnDisable()
	{
		Inventory.SendNewWeapon -= NewWeapon;
		Inventory.ChangeItemPlayerStats -= ChangeItemStats;
	}
	public void NewWeapon(GameObject newWeaponItem)
	{
		if (!newWeaponItem)
		{
			newWeaponItem = fistPrefab;
		}
		//Debug.Log("Swapping Weapon in player");
		DestroyImmediate(GameObject.FindWithTag("Weapon")); //the weapon gets fucking blasted
		GameObject newWeaponObject = Instantiate(newWeaponItem, gameObject.transform);
		WeaponScript = newWeaponObject.GetComponent<Weapon>();
		//both 0 to just add the extra damage
		//simply adding that shit (might need to get a function later)
		//set new weapon and add stats

	}


	public void ChangeItemStats(Item itemToChangeStats, bool addSub)
	{
		//Debug.Log("Changing Item");
		if (!itemToChangeStats) //to catch errors, see if an item even got sent
		{
			Debug.LogError("no item sent!");
			return;
		}
		if (addSub)
		{
			Player.Instance.BonusDamage += itemToChangeStats.Damage;
			Player.Instance.BonusFireRate += itemToChangeStats.FireRate;
			Player.Instance.BonusBulletAmount += itemToChangeStats.BulletAmount;
			Player.Instance.BonusCritChance += itemToChangeStats.CritChance;
			Player.Instance.BonusCritDamage += itemToChangeStats.CritDamage;
			Player.Instance.Defense += itemToChangeStats.Defense;
			Player.Instance.AddMaxHealth(itemToChangeStats.HealthBonus);
		}
		else
		{
			Player.Instance.BonusDamage -= itemToChangeStats.Damage;
			Player.Instance.BonusFireRate -= itemToChangeStats.FireRate;
			Player.Instance.BonusBulletAmount -= itemToChangeStats.BulletAmount;
			Player.Instance.BonusCritChance -= itemToChangeStats.CritChance;
			Player.Instance.BonusCritDamage -= itemToChangeStats.CritDamage;
			Player.Instance.Defense -= itemToChangeStats.Defense;
			Player.Instance.AddMaxHealth(-itemToChangeStats.HealthBonus);
			//if equipment adds / subtracts more stats, this has to be added here
		}
		if (itemToChangeStats is AbilityItem)
		{
			//Debug.Log("Changing Ability item");
			AbilityItem tempAbility = itemToChangeStats as AbilityItem;
			//abilityScript.Cooldown = tempAbility.AbilityCooldown;
		}
	}
}
