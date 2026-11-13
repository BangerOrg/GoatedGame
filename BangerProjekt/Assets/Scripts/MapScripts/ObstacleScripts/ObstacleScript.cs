using NavMeshPlus.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleScript : MonoBehaviour
{
	[field: SerializeField] public Obstacle Obstacle { get; set; }
	private Player player;

	public virtual void Setup()
	{
		this.GetComponent<SpriteRenderer>().sprite = Obstacle.Sprite;

		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		if (Obstacle.DamagesUnitsTemporarily) DamagesUnitsTemp(); //this needs to be done first, because it will destroy this cript and replace it with itself if true
		Movable();
		if (Obstacle.Passable) Passable();
		if (Obstacle.Destroyable) Destroyable();
		if (Obstacle.Carnivore || Obstacle.Herbivore) Edible();
	}

	private void Passable()
	{
		Collider2D col = this.GetComponent<Collider2D>();
		col.isTrigger = true;
		if (Obstacle.HigherThanPlayer) this.GetComponent<SpriteRenderer>().sortingLayerName = "HighObstacle";
	}
	private void Movable()
	{
		if (Obstacle.Movable)
		{
			Rigidbody2D rb = this.AddComponent<Rigidbody2D>();
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.angularDrag = 5;
			rb.gravityScale = 0;
			rb.drag = 5;
			rb.freezeRotation = true;
			rb.mass = Obstacle.size switch
			{
				Enums.ObstacleSize.Small => 1f,
				Enums.ObstacleSize.Medium => 3f,
				Enums.ObstacleSize.Large => 10f,
				_ => 2f
			};
		}
		else if (!Obstacle.Passable)
		{
			NavMeshObstacle navObs = this.AddComponent<NavMeshObstacle>();
			navObs.shape = NavMeshObstacleShape.Box;
			float actualWidth = this.GetComponent<SpriteRenderer>().bounds.size.x / transform.localScale.x;

			navObs.size = new Vector3(actualWidth, actualWidth, 1f);
			navObs.carving = true;
			navObs.carveOnlyStationary = true;
		}


	}
	private void Destroyable()
	{
		this.AddComponent<DestroyableObstacle>();
	}

	private void Edible()
	{
		if (InventoryLogic.ActiveInventory.EquippedItems[(int)Enums.SlotTag.Ability].ItemName == "Carnivore" || InventoryLogic.ActiveInventory.EquippedItems[(int)Enums.SlotTag.Ability].ItemName == "Herbivore")
		{
			player.HealUnit(Obstacle.HP);
			Destroy(this.gameObject);
		}
	}


	private void DamagesUnitsTemp()
	{
		if (this is TemporaryDamagingObstacle) return;
		if (!GetComponent<Rigidbody2D>())
		{
			Rigidbody2D rb = this.AddComponent<Rigidbody2D>();
			rb.angularDrag = 0;
			rb.freezeRotation = true;
			rb.gravityScale = 0;
		}
		this.AddComponent<TemporaryDamagingObstacle>().Setup(Obstacle);
		Destroy(this);
	}

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Unit>())
			collision.GetComponent<Unit>().MoveSpeed += Obstacle.SpeedModifier;
		if (Obstacle.HigherThanPlayer && collision.CompareTag("Player"))
		{
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			Color tmpColor = sr.color;
			tmpColor.a = 0.5f;
			sr.color = tmpColor;
		}
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			InvokeRepeating("DamagePlayer", 0, GameManager.STANDARD_TICK_TIME);
		}
	}
	public virtual void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Unit>())
			collision.GetComponent<Unit>().MoveSpeed -= Obstacle.SpeedModifier;
		if (Obstacle.HigherThanPlayer && collision.CompareTag("Player"))
		{
			SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
			Color tmpColor = sr.color;
			tmpColor.a = 1f;
			sr.color = tmpColor;
		}
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			CancelInvoke("DamagePlayer");
		}

	}
	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			InvokeRepeating("DamagePlayer", 0, GameManager.STANDARD_TICK_TIME);
		}
	}

	public virtual void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Obstacle.DealsContactDamage)
		{
			CancelInvoke("DamagePlayer");
		}
	}

	public virtual void DamagePlayer()
	{
		int damagePerTick = Obstacle.ContactDamage;
		Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
		if (damagePerTick < 0)
		{
			if (Obstacle.DealsPercentageDamage)
			{
				player.HealUnit(Mathf.CeilToInt(-1 * (damagePerTick / 100f) * Player.Instance.MaxHealth));
				//we do * -1 so we do not heal a negative amount (-> damage)
			}
			else
			{
				player.HealUnit(-1 * damagePerTick);
			}

		}
		else
		{
			if (Obstacle.DealsPercentageDamage)
			{
				player.DamageUnit(Mathf.CeilToInt((damagePerTick / 100f) * Player.Instance.MaxHealth), 1);
			}
			else
			{
				player.HealUnit(-1 * damagePerTick);
			}

		}
	}

}
