using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public WorldObject spawned;
	public Transform[] possiblePositions;

	[SerializeField]
	private int totalSpawnedCount = 0;
	[SerializeField]
	private List<WorldObject> spawnedList = new List<WorldObject>();
	[SerializeField]
	private int spawnedCount = 0;

	public int minimum = 1;
	public int maximum = 10;

	public float spawnedDelayShort = 1.0f;
	public float spawnDelayNormal = 4.0f;

	public float spawnPosition = 1.1f;
	private float lastSpawnedTime;

	private float spawnDelay {
		get { return spawnedCount < minimum ? spawnedDelayShort : spawnDelayNormal; }
	}

	void Start () {
		lastSpawnedTime = -spawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastSpawnedTime > spawnDelay) {
			if (spawnedCount < maximum) {
				Spawn();
			}
			lastSpawnedTime = Time.time;
		}
	}

	private void Spawn() {
		foreach (Transform pos in possiblePositions) {
			Vector2 posOnScreen = Camera.main.WorldToScreenPoint(pos.position);
			if (Mathf.Abs(posOnScreen.x) > spawnPosition || Mathf.Abs(posOnScreen.y) > spawnPosition) {
				WorldObject newSpawned = Instantiate(spawned);
				spawned.spawnedBy = this;
				newSpawned.transform.SetParent(this.transform);
				newSpawned.transform.position = pos.position;

				totalSpawnedCount += 1;
				spawnedCount += 1;
				spawnedList.Add(newSpawned);
				break;
			}
		}
	}

	public void NotifyOfDeath(WorldObject spawnedObject) {
		if (spawnedList.Remove(spawnedObject)) {
			spawnedCount -= 1;
		}
	}
}
