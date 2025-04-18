using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class CrateController : MonoBehaviour, IDamageable
{
	public float health;

	[SelfFill]
	public Rigidbody2D rb;

	public void TakeDamage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		health -= amount;
		rb.AddForceAtPosition(knockback, hitPosition, ForceMode2D.Impulse);

		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
