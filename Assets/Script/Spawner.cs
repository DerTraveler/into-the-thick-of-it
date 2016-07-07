/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public Actor spawned;
	public Transform[] possiblePositions;

	public UnityEngine.UI.Text defeatedEnemyText;

	[SerializeField]
	private int totalSpawnedCount = 0;
	[SerializeField]
	private int defeatedCount = 0;
	private List<Actor> spawnedList = new List<Actor>();
	private int spawnedCount = 0;

	public int minimum = 1;
	public int maximum = 3;

	public float spawnedDelayShort = 1.0f;
	public float spawnDelayNormal = 4.0f;

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
				Transform randomSpawnPoint = GetSpawningPosition();
				if (randomSpawnPoint != null) {
					Spawn(randomSpawnPoint);	
				}
			}
			lastSpawnedTime = Time.time;
		}
	}

	private void Spawn(Transform spawnPoint) {
		Actor newSpawned = Instantiate(spawned);
		spawned.spawnedBy = this;
		newSpawned.transform.SetParent(this.transform);
		newSpawned.transform.position = spawnPoint.position;

		totalSpawnedCount += 1;
		spawnedCount += 1;
		spawnedList.Add(newSpawned);
	}

	private Transform GetSpawningPosition() {
		List<Transform> availablePoints = new List<Transform>();
		Rect screenRect = new Rect(-0.1f, -0.1f, 1.2f, 1.2f);

		foreach (Transform t in possiblePositions) {
			if(!screenRect.Contains(Camera.main.WorldToViewportPoint(t.position))){
				availablePoints.Add(t);
			}
		}

		return availablePoints[Random.Range(0, availablePoints.Count)];
	}

	private int nextStage = 5;

	public void NotifyOfDeath(Actor spawnedObject) {
		if (spawnedList.Remove(spawnedObject)) {
			spawnedCount -= 1;
			defeatedCount += 1;
			defeatedEnemyText.text = defeatedCount.ToString();

			if (defeatedCount >= nextStage) {
				minimum++;
				maximum++;
				nextStage += 10;
			}
		}
	}
}
