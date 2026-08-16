using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [SerializeField] private float distanceToStartVisibility;


    new void Start()
    {
        base.Start();
        InvokeRepeating("SetVisibility",0,0.2f);
    }

    public void SetVisibility()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
       renderer.color = new Color(renderer.color.r,renderer.color.g,renderer.color.b,(distanceToStartVisibility - Distance)/distanceToStartVisibility);
    }

    //identical to "normal" enemies but we need to change the .a value (alpha value) of the spriteRenderer to "make them invisible"
    public override IEnumerator ShowHit() {

		float halfDuration = hitColorDuration / 2f;
        float t = 0f;

        while (t < halfDuration)
        {
            t += Time.deltaTime;
            renderer.color = Color.Lerp(originalColor, hitColor, t / halfDuration);
            yield return null;
        }

        t = 0f;

        while (t < halfDuration)
        {
            t += Time.deltaTime;
            renderer.color = Color.Lerp(hitColor, originalColor, t / halfDuration);
            yield return null;
        }
        renderer.color = originalColor;
        renderer.color = new Color(renderer.color.r,renderer.color.g,renderer.color.b,(distanceToStartVisibility - Distance)/distanceToStartVisibility);
	}
}
