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

    int totalSpawnedCount;
    int defeatedCount;
    List<Actor> spawnedList = new List<Actor>();
    int spawnedCount;

    public int minimum = 1;
    public int maximum = 3;

    public float spawnedDelayShort = 1.0f;
    public float spawnDelayNormal = 4.0f;

    float lastSpawnedTime;

    float spawnDelay {
        get { return spawnedCount < minimum ? spawnedDelayShort : spawnDelayNormal; }
    }

    void Start() {
        lastSpawnedTime = Time.time - spawnDelay;
    }

    // Update is called once per frame
    void Update() {
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

    void Spawn(Transform spawnPoint) {
        Actor newSpawned = Instantiate(spawned);
        newSpawned.spawnedBy = this;
        newSpawned.transform.SetParent(transform);
        newSpawned.transform.position = spawnPoint.position;

        totalSpawnedCount += 1;
        spawnedCount += 1;
        spawnedList.Add(newSpawned);
    }

    Transform GetSpawningPosition() {
        var availablePoints = new List<Transform>();
        var screenRect = new Rect(-0.1f, -0.1f, 1.2f, 1.2f);

        foreach (Transform t in possiblePositions) {
            if (!screenRect.Contains(Camera.main.WorldToViewportPoint(t.position))) {
                availablePoints.Add(t);
            }
        }

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    int nextStage = 5;

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
