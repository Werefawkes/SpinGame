using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class BossSlime : BossController
{
	[HorizontalLine("Slime Boss")]
	public float scale;
	public GameObject prefab;

	public float splitPercent = 0.5f;
	public int splitCount = 2;

	public override void Start()
	{
		base.Start();

		transform.localScale = new(scale, scale);
	}

	private void Update()
	{
		// Move towards the player
		Vector2 dir = GameManager.player.transform.position - transform.position;
		rb.AddForce(dir.normalized * speed);

		// Split
		if (currentHealth / maxHealth <= splitPercent)
		{
			for (int i = 0; i < splitCount; i++)
			{
				Vector3 offset = new(Random.value, Random.value, 0);
				BossSlime b = Instantiate(prefab, transform.position + offset, Quaternion.identity).GetComponent<BossSlime>();
				b.maxHealth = currentHealth / splitCount;
				b.scale = scale / splitCount;
			}

			Destroy(gameObject);
		}
	}
}
