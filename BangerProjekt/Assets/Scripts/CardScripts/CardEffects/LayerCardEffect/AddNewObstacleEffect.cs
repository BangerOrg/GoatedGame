using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddNewObstacleEffect")]

public class AddNewObstacleEffect : CardEffect
{
	[SerializeField] private Obstacle obstacle;


	public override void ExecuteEffect(string effect)
	{
		foreach (GameObject obj in RoomManager.Instance.Rooms)
		{
			RoomScript script = obj.GetComponent<RoomScript>();
			script.RemoveObstacles();
			script.AllAvailableObstacles.Add(obstacle);
			script.SetObstacles();
		}
	}

	public override void RevertEffect(string effect)
	{
		//we dont need a revert
	}
}
