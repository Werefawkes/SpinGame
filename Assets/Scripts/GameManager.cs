using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CustomInspector;

public class GameManager : NetworkBehaviour
{
	public static PlayerController localPlayer;

	[ForceFill]
	public GameObject zombiePrefab;

	public float zombieSpawnTime = 1;
	float zombieSpawnTimer;

	private void Start()
	{
		if (isServer)
		{
			zombieSpawnTimer = zombieSpawnTime;
		}
	}

	private void Update()
	{
		if (!isServer) return;
		
		//if (zombieSpawnTimer <= 0)
		//{
		//	SpawnZombie();
		//	zombieSpawnTimer = zombieSpawnTime;
		//}

		//zombieSpawnTimer -= Time.deltaTime;
	}

	public void SpawnZombie()
	{
		GameObject z = Instantiate(zombiePrefab);

		NetworkServer.Spawn(z);
	}
}