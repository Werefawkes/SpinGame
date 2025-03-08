using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class Shooter : MonoBehaviour
{
	[HorizontalLine("Shooter")]
	[Foldout]
	public WeaponSO weapon;
	float cooldownEndTime;

	public List<Projectile> projectiles;

	[HorizontalLine("Audio")]
	public float volumeMultiplier = 1;

	[HorizontalLine("References")]
	public Transform projectileOrigin;
	[SelfFill(true)] public AudioPlayer audioPlayer;

	[SelfFill(true)]
	public Rigidbody2D rb;

	[HorizontalLine]
	[ReadOnly]
	public Vector2 aimDirection;
	[ReadOnly]
	public Vector2 targetPosititon;
	[ReadOnly]
	public bool isAttacking = false;
	[ReadOnly]
	public bool wasAttacking = false;

	private void Update()
	{
		// First attack
		if (isAttacking && !wasAttacking && Time.time >= cooldownEndTime)
		{
			PrimaryActionBegin();
			wasAttacking = true;
		}

		if (isAttacking && Time.time >= cooldownEndTime)
		{
			PrimaryAction();
			cooldownEndTime = Time.time + weapon.cooldown;
		}

		if (!isAttacking)
		{
			// check the flail's angle
			if (weapon.IsFlail && wasAttacking)
			{
				foreach (Projectile p in projectiles)
				{
					Vector2 targetDir = (targetPosititon - (Vector2)p.transform.position).normalized;
					Vector2 dir = p.rb.linearVelocity.normalized;
					//Debug.Log(Vector2.)
					if (Vector2.Angle(targetDir, dir) <= weapon.firingArc)
					{
						PrimaryActionRelease();
						wasAttacking = false;
					}
					else
					{
						PrimaryAction();
						cooldownEndTime = Time.time + weapon.cooldown;
					}
				}
			}
			else
			{
				PrimaryActionRelease();
				wasAttacking = false;
			}
		}
	}

	void PrimaryActionBegin()
	{
		//// Spawn projectiles
		//if (weapon.maxCount < 0 || projectiles.Count < weapon.maxCount)
		//{
		//	SpawnProjectile();
		//}

		if (weapon.IsFlail)
		{
			foreach (Projectile p in projectiles)
			{
				p.rb.linearDamping = 0;

				Vector2 dir = transform.position - p.transform.position;
				p.rb.AddForce(dir * weapon.flailYankForce, ForceMode2D.Impulse);
			}
		}
	}

	void PrimaryAction()
	{
		// Spawn projectiles
		if (weapon.maxCount < 0 || projectiles.Count < weapon.maxCount)
		{
			SpawnProjectile();
		}

		if (weapon.IsFlail)
		{
			foreach (Projectile p in projectiles)
			{
				p.distanceJoint.distance = Mathf.Lerp(p.distanceJoint.distance, weapon.flailSwingDistance, weapon.flailDistanceDelta * Time.deltaTime);

				Vector2 dir = p.transform.position - projectileOrigin.position;
				dir = Vector2.Perpendicular(dir);
				p.rb.AddForce(dir * weapon.speed);
			}
		}
	}


	void PrimaryActionRelease()
	{
		if (weapon.IsFlail)
		{
			foreach (Projectile p in projectiles)
			{
				p.rb.linearDamping = Mathf.Lerp(p.rb.linearDamping, weapon.drag, weapon.flailDragDelta * Time.deltaTime);
				p.distanceJoint.distance = weapon.flailLaunchDistance;
			}
		}
	}

	public Projectile SpawnProjectile()
	{
		Projectile p = Instantiate(weapon.prefab, projectileOrigin.position, Quaternion.identity).GetComponent<Projectile>();
		p.data = weapon;
		p.owner = this;

		p.rb.linearDamping = 0;

		float angle = Random.Range(-weapon.firingArc / 2, weapon.firingArc / 2);
		Vector2 dir = RotateBy(aimDirection, angle);
		p.velocity = dir * weapon.speed;

		// Audio
		audioPlayer.PlayRandom(weapon.fireSounds, weapon.pitchRange);

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
