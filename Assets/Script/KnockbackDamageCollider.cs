/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class KnockbackDamageCollider : MonoBehaviour {

	public int attackDamage = 1;
	public float knockbackForce = 1.0f;
	public AudioClip hitSound = null;

	void OnTriggerEnter2D(Collider2D other) {
		Actor target = other.attachedRigidbody.GetComponent<Actor>();

		if (target.ReceiveDamage(attackDamage)) {
			Vector2 hitForce = other.bounds.center - transform.position;
			if (hitForce.x > hitForce.y) {
				hitForce.Set(Mathf.Sign(hitForce.x), 0);
			} else {
				hitForce.Set(0, Mathf.Sign(hitForce.y));
			}
			other.attachedRigidbody.AddForce(hitForce.normalized * knockbackForce, ForceMode2D.Impulse);

			if (hitSound) {
				AudioManager.instance.PlaySound(hitSound);
			}
		}
	}

}
