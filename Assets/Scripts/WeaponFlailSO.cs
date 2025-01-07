using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponFlailSO : WeaponSO
{
	public static Projectile projectile;

	public override void PrimaryAction(PlayerController player)
	{
		// Launch projectile
		CheckProjectile(player);
	}

	public override void SecondaryAction(IShooter shooter)
	{
		// Spin projectile
		CheckProjectile(shooter);

		//Vector2 dir = transform.position - owner.ProjectileOrigin;
		//rb.velocity = Vector2.Perpendicular(dir) * data.speed;
		Vector2 direction = projectile.transform.position - shooter.ProjectileOrigin;
		direction = Vector2.Perpendicular(direction);
		projectile.rb.AddForce(direction * speed);
	}

	void CheckProjectile(IShooter shooter)
	{
		if (projectile == null)
		{
			projectile = SpawnProjectile(shooter);
		}
	}

}
