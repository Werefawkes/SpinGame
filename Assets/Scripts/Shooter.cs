using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using Mirror;
using ReadOnlyAttribute = CustomInspector.ReadOnlyAttribute;

public class Shooter : NetworkBehaviour
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
	[ForceFill] public CrosshairController crosshair;

	[SelfFill(true)]
	public Rigidbody2D rb;

	[HorizontalLine("Debug")]
	public bool drawDebugLines = false;
	[ReadOnly] public float currentFiringAngle;
	[ReadOnly] public Vector2 aimDirection;
	[ReadOnly] public Vector2 targetPosititon;
	[ReadOnly] public bool isAttacking = false;
	[ReadOnly] public bool wasAttacking = false;
	[ReadOnly] public int currentMagazine = 10;
	[ReadOnly] public int currentReserve = 30;
	[ReadOnly] public bool isReloading = false;
	[ReadOnly] public float reloadTimer = 0;

	private void Start()
	{
		currentMagazine = weapon.magazineSize;
		currentReserve = weapon.reserveSize;

		EndReload();
	}

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
					if (Vector2.Angle(targetDir, dir) <= currentFiringAngle)
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

		currentFiringAngle = Mathf.Lerp(currentFiringAngle, weapon.firingAngle.x, weapon.recovery * Time.deltaTime);
		//currentFiringAngle = Mathf.Clamp(currentFiringAngle - weapon.recovery * Time.deltaTime, weapon.firingAngle.x, weapon.firingAngle.y);

		// Reloading
		if (isReloading)
		{
			reloadTimer -= Time.deltaTime;

			// Update graphic
			crosshair.SetReload(reloadTimer / weapon.reloadTime);

			// If done reloading
			if (reloadTimer <= 0)
			{
				EndReload();
			}
		}

		if (drawDebugLines) 
		{
			Debug.DrawRay(projectileOrigin.position, aimDirection * 100, Color.white);
			Debug.DrawRay(projectileOrigin.position, RotateBy(aimDirection, currentFiringAngle / 2) * 100, Color.red);
			Debug.DrawRay(projectileOrigin.position, RotateBy(aimDirection, -currentFiringAngle / 2) * 100, Color.red);
			float d = Mathf.Tan(currentFiringAngle / 2 * Mathf.Deg2Rad) * Vector2.Distance(projectileOrigin.position, targetPosititon);
			Debug.DrawRay(targetPosititon, Vector2.Perpendicular(aimDirection) * d, Color.blue);
		}
	}

	void StartReload()
	{
		if (currentMagazine >= weapon.magazineSize || currentReserve <= 0) return;

		isReloading = true;
		reloadTimer = weapon.reloadTime;

		if (weapon.reloadSound)
		{
			float pitch = weapon.reloadSound.length / weapon.reloadTime;
			audioPlayer.PlaySound(weapon.reloadSound, pitch);
		}
	}

	void EndReload()
	{
		isReloading = false;
		crosshair.SetReload(0);

		int bulletsNeeded = weapon.magazineSize - currentMagazine;
		currentMagazine += Mathf.Min(currentReserve, bulletsNeeded);
		currentReserve -= Mathf.Min(currentReserve, bulletsNeeded);

		OnAmmoUpdate();
	}

	void PrimaryActionBegin()
	{
		if (!weapon.isFullAuto && !isReloading)
		{
			TryFireWeapon();
		}

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
		if (weapon.isFullAuto && !isReloading)
		{
			TryFireWeapon();
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

	void TryFireWeapon()
	{
		// Don't spawn a projectile if we're at the max
		if (weapon.maxCount < 0 || projectiles.Count < weapon.maxCount)
		{
			// Don't spawn a projectile if the gun's empty
			if (currentMagazine > 0)
			{
				// increase recoil
				currentFiringAngle = Mathf.Clamp(currentFiringAngle + weapon.recoil, weapon.firingAngle.x, weapon.firingAngle.y);
				currentMagazine--;

				// Audio
				audioPlayer.PlayRandom(weapon.fireSounds, weapon.pitchRange);

				OnAmmoUpdate();

				SpawnProjectile();
			}
			else // Mag is empty
			{
				// Play empty mag sound
				if (weapon.emptySound)
				{
					audioPlayer.PlaySound(weapon.emptySound);
				}
			}
		}
	}

	void SpawnProjectile()
	{

		GameObject go = Instantiate(weapon.prefab, projectileOrigin.position, Quaternion.identity);
		Projectile p = go.GetComponent<Projectile>();
		p.data = weapon;
		p.owner = this;

		p.rb.linearDamping = 0;

		// Get firing angle
		float angle = Random.Range(-currentFiringAngle / 2, currentFiringAngle / 2);
		Vector2 dir = RotateBy(aimDirection, angle);

		if (weapon.IsOrbiter)
		{
			dir = Vector2.Perpendicular(dir);
		}

		p.velocity = dir * weapon.speed;

		NetworkServer.Spawn(go);

		return;
	}

	void OnAmmoUpdate()
	{
		UIManager.Instance.SetAmmoText(currentMagazine, currentReserve);
	}

	// Input methods
	public void OnReload()
	{
		StartReload();
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