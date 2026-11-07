using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CardEffect/SpeedWhenCloseEffect")]

public class SpeedWhenCloseEffect : CardEffect
{
	float radius;
	float bonusSpeed;

	GameObject speedObject;
	SpeedWhenCloseScript speedScript;
	public override void ExecuteEffect(string effect)
	{
		string[] strings = effect.Split(";");
		radius = float.Parse(strings[0], info.NumberFormat);
		bonusSpeed = float.Parse(strings[1], info.NumberFormat);

		speedObject = Instantiate(new GameObject(), Player.Instance.gameObject.transform);
		speedScript = speedObject.AddComponent<SpeedWhenCloseScript>();
		speedScript.Init(radius, bonusSpeed);
		speedObject.name = "CloseSpeedUpChecker";
	}

	public override void RevertEffect(string effect)
	{
		Destroy(speedObject);
	}
}
