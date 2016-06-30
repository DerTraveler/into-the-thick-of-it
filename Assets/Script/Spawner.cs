/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public WorldObject spawned;
	public Transform[] possiblePositions;

	public UnityEngine.UI.Text defeatedEnemyText;

	[SerializeField]
	private int totalSpawnedCount = 0;
	[SerializeField]
	private int defeatedCount = 0;
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
			defeatedCount += 1;
			defeatedEnemyText.text = defeatedCount.ToString();
		}
	}
}
