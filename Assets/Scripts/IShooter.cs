using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
	public Vector3 ProjectileOrigin { get; }
	public Vector2 AimDirection { get; }
	public Rigidbody2D Rigidbody { get; }
}
