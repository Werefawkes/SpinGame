using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class Projectile : MonoBehaviour
{
	public WeaponSO data;
	public Vector2 velocity;

	public Shooter owner;

	float bornTime;

	[SelfFill(hideIfFilled:true)]
	public Rigidbody2D rb;
	[SelfFill(true)]
	public DistanceJoint2D distanceJoint;

	public virtual void Start()
	{
		owner.projectiles.Add(this);

		rb.AddForce(velocity, ForceMode2D.Impulse);
		bornTime = Time.time;

		distanceJoint.enabled = data.IsFlail || data.IsOrbiter;

		distanceJoint.connectedBody = owner.rb;
		distanceJoint.distance = data.orbitDistance;
		distanceJoint.maxDistanceOnly = data.orbitMaxOnly;
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
		if (collision.TryGetComponent(out IDamageable d) && d as Object != owner)
		{
			Vector2 kb = rb.linearVelocity;
			kb.Normalize();
			kb *= data.knockbackMultiplier;
			d.TakeDamage(data.damage, kb, transform.position);
		}

		if (!data.isPersistent && d as Object != owner)
		{
			Destroy(gameObject);
		}
	}


	private void OnDestroy()
	{
		owner.projectiles.Remove(this);
	}
}
