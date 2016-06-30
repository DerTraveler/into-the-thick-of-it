/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public abstract class WorldObject : MonoBehaviour {

	public GameObject body;
	private SpriteRenderer rend;

	private float unitPerPixel;

	void Awake () {
		rend = body.GetComponent<SpriteRenderer>();

		unitPerPixel = 1.0f / rend.sprite.pixelsPerUnit;
	}

	void LateUpdate () {
		if (rend.isVisible) {
			// Dynamic draw order based on y-coordinate
			rend.sortingOrder = (int) (body.transform.position.y * 64.0 * -1.0f);
			// Round position to next pixel perfect position
			transform.position = RoundToPixelPerfect(transform.position);
		}
	}

	private Vector2 RoundToPixelPerfect(Vector2 pos) {
		return new Vector2(Mathf.Round(pos.x / unitPerPixel) * unitPerPixel, Mathf.Round(pos.y / unitPerPixel) * unitPerPixel);
	}

	public Spawner spawnedBy = null;

	// Receive damage, returns true if the object was really hurt
	public abstract bool ReceiveDamage(int damage);

	protected void SendDeathNotification() {
		if (spawnedBy) {
			spawnedBy.NotifyOfDeath(this);
		}
	}
}
