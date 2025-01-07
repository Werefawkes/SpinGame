using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class Projectile : MonoBehaviour
{
	public WeaponSO data;
	public Vector2 velocity;

	public IShooter owner;

	float bornTime;

	[SelfFill(hideIfFilled:true)]
	public Rigidbody2D rb;

	public virtual void Start()
	{
		rb.AddForce(velocity, ForceMode2D.Impulse);
		bornTime = Time.time;
	}

	public virtual void Update()
	{
		if (!data.isPersistent && Time.time >= bornTime + data.duration)
		{
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IDamageable d) && d != owner)
		{
			Vector2 kb = rb.velocity;
			kb.Normalize();
			kb *= data.knockbackMultiplier;
			d.Damage(data.damage, kb, transform.position);
		}

		if (!data.isPersistent && d != owner)
		{
			Destroy(gameObject);
		}
	}
}
