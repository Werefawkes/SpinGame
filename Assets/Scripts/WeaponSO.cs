using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
	[HorizontalLine("Stats")]
	public float damage = 1;
	public float knockbackMultiplier = 1;
	public float speed = 10;
	public float firingArc = 5;
	public float duration = 10;
	public float primaryCooldown = 0.5f;
	public float secondaryCooldown = 0.5f;

	[HorizontalLine("Behaviour")]
	[Tooltip("If checked, the projectile will not be destroyed on collisions or when expiring.")]
	public bool isPersistent = false;
	//[Tooltip("The maximum number of this projectile that can be in the world at once. Set to -1 to allow an infinite amount.")]
	//public int maxCount = -1;
	public bool isOrbiter = false;

	[ShowIf(nameof(isOrbiter))]
	public float orbitDistance = 3;
	[ShowIf(nameof(isOrbiter))]
	public bool orbitMaxOnly = false;

	[HorizontalLine]
	public GameObject prefab;

	public virtual void PrimaryAction(PlayerController player)
	{
		SpawnProjectile(player);
	}

	public virtual void SecondaryAction(IShooter shooter)
	{

	}

	public Projectile SpawnProjectile(IShooter shooter)
	{
		Projectile p = Instantiate(prefab, shooter.ProjectileOrigin, Quaternion.identity).GetComponent<Projectile>();
		p.data = this;
		p.owner = shooter;

		float angle = Random.Range(-firingArc / 2, firingArc / 2);
		Vector2 dir = RotateBy(shooter.AimDirection, angle);
		p.velocity = dir * speed;

		return p;
	}

	public static Vector2 RotateBy(Vector2 v, float deltaAngle)
	{
		deltaAngle *= Mathf.Deg2Rad;
		return new Vector2(
			v.x * Mathf.Cos(deltaAngle) - v.y * Mathf.Sin(deltaAngle),
			v.x * Mathf.Sin(deltaAngle) + v.y * Mathf.Cos(deltaAngle)
		);
	}
}