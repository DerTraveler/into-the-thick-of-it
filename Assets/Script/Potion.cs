﻿using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour {

	public bool killTrigger = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Player player = other.gameObject.GetComponent<Player>();
			player.Health += 2;
			Destroy(gameObject.transform.parent.gameObject);
		}
	}

	void Update() {
		if (killTrigger) {
			Destroy(gameObject.transform.parent.gameObject);
		}
	}
}
