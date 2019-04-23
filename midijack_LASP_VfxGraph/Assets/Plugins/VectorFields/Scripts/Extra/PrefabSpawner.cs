using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject ObjectToSpawn;
	public float TimeBetweenSpawns = 1.0f;
	public float SpawnRadius = 10.0f;
	
	private float nextSpawnTime;

	void Start()
	{
		nextSpawnTime = Time.time + TimeBetweenSpawns;
	}
	
	void LateUpdate () 
	{
		if (Time.time>nextSpawnTime)
		{
			Vector3 offset = Random.insideUnitSphere;
			offset.y = 0f;

			Instantiate(ObjectToSpawn, transform.position+offset*SpawnRadius, Quaternion.identity);
			nextSpawnTime = Time.time + TimeBetweenSpawns;
		}
	}
}
