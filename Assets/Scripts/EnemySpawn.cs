//  By Kasperi K
//
//  Handels the spawning of the enemies

using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{

    public float spawnTime;
	public float spawnTimeDelta = 4f;
    public float minSpawnTime = 1.5f;
    public GameObject enemy;

    private GameObject[] spawnPoints;

    void Start()
    {
        // Find all the spawnpoints in the scene
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }


    void Update()
    {
        // Spawn enemy when enough time has passed since the last spawn
		if (Time.time > spawnTime)
        {
            // Assing the next spawntime
			spawnTime = Time.time + spawnTimeDelta;

            // Spawn enemy to one of the spawn points
            Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);

            // Accelerate spawning until it's minSpawnTime
            if (spawnTimeDelta > minSpawnTime)
                spawnTimeDelta -= 0.2f;
        }
    }

}
