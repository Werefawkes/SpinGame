using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{
	[HorizontalLine("Enemy")]
	public float maxHealth;
	public float currentHealth;

	public float speed;

	[SelfFill(true)]
	public Rigidbody2D rb;
	[ForceFill]
	public Image healthbar;

	public virtual void Start()
	{
		currentHealth = maxHealth;
		healthbar.fillAmount = 1;
	}

	public void Damage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		currentHealth -= amount;
		rb.AddForceAtPosition(knockback, hitPosition, ForceMode2D.Impulse);

		healthbar.fillAmount = currentHealth / maxHealth;

		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}
}
