using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeBullet : MonoBehaviour
{
    private Weapon weaponScript;

    private void Awake()
    {
        weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        if (currObject.CompareTag("Enemy")||currObject.CompareTag("Obstacle")) //if its an enemy (as set by its tag)
        {
            //Damage the Enemy
            currObject.GetComponent<Unit>().DamageUnit((int)(weaponScript.Damage * weaponScript.DamageMult));
        }

    }
}
