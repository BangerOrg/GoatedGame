using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponItem : Item
{
    [field:SerializeField] public float ShotDelay {get; set;}
    [field:SerializeField] public int SpreadAngle {get; set;}
    [field:SerializeField] public int ShotSpeed {get; set;}
    [field:SerializeField] public GameObject BulletPrefab {get; set;}
    [field:SerializeField] public GameObject CorrespondingPrefab {get; set;}
}
