using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
	public float damage;
	public float speed;
	public float duration;
	public float cooldown;
	public float knockback;

	public GameObject prefab;

	public void SpawnProjectile(Vector2 position, Vector2 direction)
	{
		Projectile p = Instantiate(prefab, position, Quaternion.identity).GetComponent<Projectile>();
		p.Damage = damage;
		p.Velocity = direction * speed;
		p.Duration = duration;
		p.Knockback = knockback;
	}
}
