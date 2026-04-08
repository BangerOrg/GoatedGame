using UnityEngine;

public class ObstacleExplosionVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        transform.localScale += Vector3.one * Time.fixedDeltaTime * 5f;
        Color c = sr.color;
        c.a -= Time.fixedDeltaTime * 2f;
        sr.color = c;

        if (c.a <= 0) Destroy(gameObject);
    }
}
