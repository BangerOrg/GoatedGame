using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
	public Unit Unit { get; set; }
	public Rigidbody2D Rb { get; set; }
	public int Pierce { get; set; }

}
