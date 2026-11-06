using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lasso : Weapon
{
	Camera mainCamera;

	private bool lassoExists = false;
	new void Awake()
	{
		base.Awake();
		mainCamera = Camera.main;
	}

	new void OnEnable()
	{
		base.OnEnable();
		LassoProjectile.lassoEnd += StartCooldown;
	}


	new void OnDisable()
	{
		base.OnDisable();
		LassoProjectile.lassoEnd -= StartCooldown;
	}
	public override void Shoot(int bulletCount)
	{
		if (lassoExists) return;
		lassoExists = true;
		Vector2 screenPos = Mouse.current.position.ReadValue();
		Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
		GameObject newBullet = Instantiate(bulletPrefab, worldPos, Quaternion.identity);
		if (newBullet.TryGetComponent(out LassoProjectile lassoProj))
		{
			lassoProj.InitializeLasso(this);
		}
	}

	public void StartCooldown()
	{
		StartCoroutine(StartReloadCooldown());
	}

	public override IEnumerator StartReloadCooldown()
	{
		lassoExists = false;
		return base.StartReloadCooldown();
	}
}
