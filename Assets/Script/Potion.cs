/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour {

	public AudioClip potionSound;
	public bool killTrigger = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Player player = other.gameObject.GetComponent<Player>();
			player.Health += 2;
			AudioManager.instance.PlaySound(potionSound);
			Destroy(gameObject.transform.parent.gameObject);
		}
	}

	void Update() {
		if (killTrigger) {
			Destroy(gameObject.transform.parent.gameObject);
		}
	}
}
