using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/EnemiesExplodeToBulletsEffect")]
public class EnemiesExplodeToBulletsEffect : CardEffect
{

	[SerializeField] private GameObject bulletPrefab;
	int damageValue;

	private const int SHOT_SPEED = 3;

	public override void ExecuteEffect(string effect)
	{
		damageValue = int.Parse(effect, info.NumberFormat);
		Enemy.enemyDies += Explode;
	}


	public void Explode(GameObject obj)
	{
		int amountBullets = Random.Range(1, 9); //between 1 and 8 bullets
		int rotationAngle = 0;
		for (int i = 0; i < amountBullets; i++)
		{
			rotationAngle += Random.Range(-180, 181);
			GameObject newBullet = Instantiate(bulletPrefab, obj.transform.position, Quaternion.identity);
			newBullet.transform.Rotate(new Vector3(0, 0, rotationAngle));
			newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.up * SHOT_SPEED * Player.Instance.BonusShotSpeed, ForceMode2D.Impulse);
			newBullet.GetComponent<EnemyExplosionBullet>().Damage = damageValue;
		}
	}
	public override void RevertEffect(string effect)
	{
		Enemy.enemyDies -= Explode;
	}
}
