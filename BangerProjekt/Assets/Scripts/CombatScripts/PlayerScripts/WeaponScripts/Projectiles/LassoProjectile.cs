using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoProjectile : MonoBehaviour
{
	public Unit Unit { get; set; }
	public Rigidbody2D Rb { get; set; }
	public int Pierce { get; set; }
	protected Weapon weaponScript;

	private List<GameObject> trappedEnemies = new List<GameObject>();
	private EdgeCollider2D lassoArea;
	private LineRenderer lineRenderer; // <-- HIER: LineRenderer Referenz

	private bool isShrinking = true;
	private int pointsCount = 32;
	private float totalEnemyArea = 0f;
	private float currentRadius;

	void Awake()
	{
		Rb = GetComponent<Rigidbody2D>();
		lassoArea = GetComponent<EdgeCollider2D>();
		lineRenderer = GetComponent<LineRenderer>(); // <-- HIER: Automatisches Holen der Komponente

		SetupLineRenderer();
	}

	private void SetupLineRenderer()
	{
		if (lineRenderer != null)
		{
			lineRenderer.useWorldSpace = false; // Punkte sind lokal zum Lasso-Objekt
			lineRenderer.loop = true; // Geschlossener Kreis
			lineRenderer.positionCount = pointsCount;
		}
	}

	public void InitializeLasso(Weapon weapon)
	{
		weaponScript = weapon;

		if (!weaponScript.HoldingTrigger)
		{
			Debug.LogWarning("[Lasso] Trigger war beim Spawnen nicht gedrückt – Lasso zerstört.");
			Destroy(gameObject);
			return;
		}

		weaponScript.OnTriggerReleased += HandleTriggerReleased;

		currentRadius = weaponScript.ShotDelay * (weaponScript.BulletAmount * 0.5f);

		StartCapture();
		GenerateCircleCollider(currentRadius);
		StartCoroutine(Shrink());
	}

	private void HandleTriggerReleased()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (weaponScript != null)
		{
			weaponScript.OnTriggerReleased -= HandleTriggerReleased;
		}
	}

	public void StartCapture()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, currentRadius);
		trappedEnemies.Clear();

		foreach (Collider2D col in hitEnemies)
		{
			if (col.CompareTag("Enemy"))
			{
				trappedEnemies.Add(col.gameObject);
				totalEnemyArea += EstimateColliderArea(col);
			}
		}
	}

	private IEnumerator Shrink()
	{
		while (isShrinking)
		{
			int previousCount = trappedEnemies.Count;
			trappedEnemies.RemoveAll(e => e == null);

			if (trappedEnemies.Count == 0)
			{
				Destroy(gameObject);
				yield break;
			}

			if (trappedEnemies.Count < previousCount)
			{
				totalEnemyArea = 0f;
				foreach (GameObject t in trappedEnemies)
				{
					if (t != null && t.TryGetComponent(out Collider2D col))
					{
						totalEnemyArea += EstimateColliderArea(col);
					}
				}
			}

			float area = Mathf.PI * currentRadius * currentRadius;

			if (area <= totalEnemyArea * 1.15f || AreAllEnemiesImmobalized())
			{
				isShrinking = false;
				foreach (GameObject e in trappedEnemies)
				{
					if (e != null && e.TryGetComponent(out Unit u))
					{
						u.DamageUnit(weaponScript.Damage, 1f);
					}
				}
				yield break;
			}

			currentRadius = Mathf.Max(0.5f, currentRadius - (weaponScript.ShotSpeed * Time.deltaTime));
			GenerateCircleCollider(currentRadius);

			if (currentRadius <= 0.5f)
			{
				Destroy(gameObject);
				yield break;
			}

			yield return null;
		}
	}

	private bool AreAllEnemiesImmobalized()
	{
		foreach (GameObject t in trappedEnemies)
		{
			if (t != null && t.TryGetComponent(out Rigidbody2D enemyRb))
			{
				if (enemyRb.velocity.magnitude > 0.008f)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void GenerateCircleCollider(float radius)
	{
		Vector2[] colliderPoints = new Vector2[pointsCount + 1];
		Vector3[] linePoints = new Vector3[pointsCount]; // LineRenderer braucht Vector3

		float angleStep = 360f / pointsCount;

		for (int i = 0; i < pointsCount; i++)
		{
			float angleRad = Mathf.Deg2Rad * (i * angleStep);
			Vector2 point = new Vector2(Mathf.Cos(angleRad) * radius, Mathf.Sin(angleRad) * radius);

			colliderPoints[i] = point;
			linePoints[i] = new Vector3(point.x, point.y, 0f); // Für Visuals
		}

		// Collider-Schleife schließen
		colliderPoints[pointsCount] = colliderPoints[0];
		lassoArea.points = colliderPoints;

		// LineRenderer Punkte setzen
		if (lineRenderer != null)
		{
			lineRenderer.SetPositions(linePoints);
		}
	}

	private float EstimateColliderArea(Collider2D col)
	{
		if (col == null) return 0f;

		if (col is CircleCollider2D circle)
		{
			float r = circle.radius * Mathf.Max(col.transform.lossyScale.x, col.transform.lossyScale.y);
			return Mathf.PI * r * r;
		}
		else
		{
			Bounds b = col.bounds;
			return b.size.x * b.size.y * 0.8f;
		}
	}
}
