using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestRunner;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using TMPro;

[TestFixture]
public class EnemyPlayModeTests
{
	GameObject dummyEnemyObject;
	Enemy enemy;
	Rigidbody2D dummyrb;

	GameObject dummyObject;
	GameObject dummyWeaponObject;

	GameObject dummyShootingMiddle;
	GameObject dummyShootingPoint;

	WeaponItem dummyWeaponSO;

	InputAction dummyFireAction;

	GameObject dummyBullet;

	GameObject dummyGameOverScreen;
	Player player;
	RangedWeapon dummyWeapon;
	UseAbilities dummyAbilities;
	PlayerInput dummyInput;

	GameObject dummyPlayerObject;

	Player dummyPlayerScript;


	GameObject dummyPopUp;
	[SetUp]
	public void Setup()
	{


		dummyObject = new GameObject();
		dummyObject.tag = "Player";
		dummyWeaponObject = new GameObject();
		dummyWeaponObject.transform.parent = dummyObject.transform;
		dummyWeaponObject.tag = "Weapon";
		dummyShootingMiddle = new GameObject();
		dummyShootingMiddle.transform.parent = dummyWeaponObject.transform;
		dummyShootingMiddle.name = "ShootingMiddle";
		dummyShootingPoint = new GameObject();
		dummyShootingPoint.transform.parent = dummyShootingMiddle.transform;
		dummyBullet = new GameObject();
		dummyWeaponSO = ScriptableObject.CreateInstance<WeaponItem>();
		dummyWeaponSO.BulletPrefab = dummyBullet;
		dummyFireAction = new InputAction();


		dummyGameOverScreen = new GameObject();
		dummyGameOverScreen.tag = "GameOver";
		player = dummyObject.AddComponent<Player>();
		dummyWeaponObject.AddComponent<RangedWeapon>().Fire = dummyFireAction;
		dummyWeapon = dummyWeaponObject.GetComponent<RangedWeapon>();
		dummyWeapon.CorrespondingItem = dummyWeaponSO;
		dummyAbilities = dummyObject.AddComponent<UseAbilities>();
		dummyInput = dummyObject.AddComponent<PlayerInput>();

		dummyPopUp = new GameObject("PopUpPrefab");
		dummyPopUp.AddComponent<TextMeshPro>();
		dummyPopUp.AddComponent<PopUp>();


		dummyEnemyObject = new GameObject();
		enemy = dummyEnemyObject.AddComponent<Enemy>();
		dummyrb = dummyEnemyObject.AddComponent<Rigidbody2D>();

		enemy.AddMaxHealth(20);
		enemy.playerObject = dummyObject;
	}



	[UnityTest]
	public IEnumerator EnemyDeathTest()
	{
		enemy.DamageUnit(20, 1);
		yield return new WaitForEndOfFrame();
		Assert.AreEqual(enemy.CurrentHealth, 0);
		Assert.AreEqual(player.KillCount, 1);


	}
}
