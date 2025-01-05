using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	public void Damage(float amount, Vector2 knockback, Vector2 hitPosition);
}