using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lasso : Weapon
{
	Camera mainCamera;
	void Awake()
	{
		mainCamera = Camera.main;
		Debug.Log("This is a lasso");
	}
	public override void Shoot(int bulletCount)
	{
		Debug.Log("shooting");
		Vector2 screenPos = Mouse.current.position.ReadValue();
		Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
		GameObject newBullet = Instantiate(bulletPrefab, worldPos, Quaternion.identity);
		if (newBullet.TryGetComponent(out LassoProjectile lassoProj))
		{
			lassoProj.InitializeLasso(this);
		}
	}
}
