using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Codice.Client.BaseCommands.Differences;
using Codice.CM.Common;
using PlasticPipe.PlasticProtocol.Messages;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    private float timeAlive; //the max time alive before spontaneously imploding
    protected Weapon weaponScript;
    protected Player playerScript;
    private Rigidbody2D rb;
    [field: SerializeField]private float Radius;
    private GameObject rangeIndicatorObject; //this is the grey circle that shows the range of the explosion
    private Vector2 bulletPos;
    private void Awake()
    {
        timeAlive = 20;
        weaponScript = GameObject.FindWithTag("Weapon").GetComponent<Weapon>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        rangeIndicatorObject = gameObject.transform.GetChild(0).gameObject;
        rangeIndicatorObject.SetActive(false);
    }


    void Start()
    {
        StartCoroutine(BulletCountDown());
    }


    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator BulletCountDown()
    {
        yield return new WaitForSeconds(timeAlive); //wait for the specified time
        Destroy(gameObject); //Destroy the Object
    }


    public float CritCalculate() // starts the Crit roulet
    {
        int temp = Random.Range(1, 101);
        if (temp <= weaponScript.CritChance * playerScript.BonusCritChance)
        {
            float CritValue = 1 + weaponScript.CritDamage / 100f;
            CritValue *= playerScript.BonusCritDamage;
            return CritValue; // returns the crit damage as a 1.x multiplier
        }
        else return 1; //1 means a multiplier of 1.0, so normal DMG
    }


    public void CheckObstacleAndSetBehaivour(GameObject currObject)
    {
        if (currObject.GetComponent<ObstacleScript>().Obstacle.Passable) return; //we dont care about passable obstacles
        if (currObject.GetComponent<DestroyableObstacle>()) //we damage obstacles that u can destroy
        {
            boom();
        }
    }


    public void DamageCalculation(GameObject currObject, bool isLifestealable)
    {
        float CritDamage = CritCalculate();
        int totalDamage = (int)((weaponScript.Damage * weaponScript.DamageMult * playerScript.BonusDamage) * CritDamage);
        if (totalDamage <= 0) totalDamage = 1;
        currObject.GetComponent<Unit>().DamageUnit(totalDamage, CritDamage);
        return;
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        switch (currObject.tag)
        {
            case "Enemy": boom(); break; //we just damage enemies, and we can lifesteal from them
            case "Obstacle": CheckObstacleAndSetBehaivour(currObject); break; //check which type of obstacle and do stuff accordingly
            case "Wall": boom(); break; //bounce on walls
            default:; break;
        }
    
    }


    public void boom()
    {
        rangeIndicatorObject.SetActive(true);
        rangeIndicatorObject.transform.localScale = new Vector2(Radius, Radius);
        this.GetComponent<Collider2D>().enabled = false;
        Collider2D[] victims = Physics2D.OverlapCircleAll(transform.position, Radius);
        foreach (Collider2D victim in victims)
        {
            if (victim == this.gameObject) continue;
            if (victim.gameObject.GetComponent<Unit>())
            {
            switch (victim.gameObject.tag)
            {
                case "Enemy": DamageCalculation(victim.gameObject, true); break; //we just damage enemies, and we can lifesteal from them
                case "Obstacle": DamageCalculation(victim.gameObject,false); break; //check which type of obstacle and do stuff accordingly
                default:; break;
            }
            }
        }
        Destroy(rangeIndicatorObject);
        Destroy(this.gameObject);
    }
}

