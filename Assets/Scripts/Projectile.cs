using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class Projectile : MonoBehaviour
{
	public Vector2 Velocity;
	public float Duration;
	public float Damage;
	public float Knockback;

	float bornTime;

	[SelfFill(hideIfFilled:true)]
	public Rigidbody2D rb;

	private void Start()
	{
		rb.velocity = Velocity;
		bornTime = Time.time;
	}

	private void Update()
	{
		if (Time.time >= bornTime + Duration)
		{
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IDamageable d))
		{
			//Vector2 kb = collision.transform.position - transform.position;
			Vector2 kb = rb.velocity;
			kb.Normalize();
			kb *= Knockback;
			d.Damage(Damage, kb, transform.position);
		}
		Destroy(gameObject);
	}
}
