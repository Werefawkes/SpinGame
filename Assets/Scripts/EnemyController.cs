using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{
	[HorizontalLine("Enemy")]
	public float maxHealth;
	public float currentHealth;

	public float speed;

	public float damage = 1;
	public float knockback = 1;

	[SelfFill(true)]
	public Rigidbody2D rb;
	[ForceFill]
	public Image healthbar;

	public virtual void Start()
	{
		currentHealth = maxHealth;
		healthbar.fillAmount = 1;
	}

	public virtual void Update()
	{
		// Move towards the player
		if (GameManager.localPlayer)
		{
			Vector2 dir = GameManager.localPlayer.transform.position - transform.position;
			rb.AddForce(dir.normalized * speed);
		}
	}

	public void TakeDamage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		currentHealth -= amount;
		rb.AddForceAtPosition(knockback, hitPosition, ForceMode2D.Impulse);

		healthbar.fillAmount = currentHealth / maxHealth;

		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Damage the player on collision
		if (collision.gameObject.TryGetComponent(out PlayerController p))
		{
			p.TakeDamage(damage, (p.transform.position - transform.position) * knockback, transform.position);
		}
	}
}
