using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    private float timeAlive; //the max time alive before spontaneously imploding
    protected Weapon weaponScript;
    protected Player playerScript;
    private Rigidbody2D rb;
    [field: SerializeField]private int Radius;
    private GameObject rangeIndicatorObject; //this is the grey circle that shows the range of the explosion
    private Vector2 bulletPos;
    CircleCollider2D circle;
    private float RocketSize = 1f;

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
            DamageCalculation(currObject,false);
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
        gameObject.transform.GetChild(0).GetComponent<Collider2D>().IsTouching(GameObject.FindWithTag("Enemy").GetComponent<Collider2D>());
        GameObject currObject = collision.gameObject; //the object with which the collision occured
        switch (currObject.tag)
        {
            case "Enemy": boom(); break; //we just damage enemies, and we can lifesteal from them
            case "Obstacle": CheckObstacleAndSetBehaivour(currObject); break; //check which type of obstacle and do stuff accordingly
            default:; break;
        }
    
    }
    public void boom()
    {
        rangeIndicatorObject.SetActive(true);
        GameObject explosion = Instantiate(rangeIndicatorObject,transform.position, Quaternion.identity);
        float explosionSize = Radius + RocketSize;
        explosion.transform.localScale = new Vector2(explosionSize, explosionSize);
        WaitForBomboclat();

        this.GetComponent<Collider2D>().enabled = false;
        Collider2D[] victims = Physics2D.OverlapCircleAll(transform.position, Radius + RocketSize);
        foreach (Collider2D victim in victims)
        {
            if (victim == this.gameObject) continue;
            if (victim.gameObject.GetComponent<Unit>())
            {
            switch (victim.gameObject.tag)
            {
                case "Enemy": DamageCalculation(victim.gameObject, true); break; //we just damage enemies, and we can lifesteal from them
                case "Obstacle": CheckObstacleAndSetBehaivour(victim.gameObject); break; //check which type of obstacle and do stuff accordingly
                default:; break;
            }
            }
        }
        Destroy(gameObject);
    }
     public IEnumerator WaitForBomboclat()
    {
        yield return new WaitForSeconds(1);
    }
}

