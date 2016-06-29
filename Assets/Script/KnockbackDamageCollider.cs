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

	void OnCollisionEnter2D(Collision2D coll) {
		WorldObject target = coll.gameObject.GetComponent<WorldObject>();

		if (target.ReceiveDamage(attackDamage)) {
			Vector2 hitForce = coll.contacts[0].point - (Vector2)transform.position;
			if (hitForce.x > hitForce.y) {
				hitForce.Set(Mathf.Sign(hitForce.x), 0);
			} else {
				hitForce.Set(0, Mathf.Sign(hitForce.y));
			}
			coll.rigidbody.AddForce(hitForce.normalized * knockbackForce, ForceMode2D.Impulse);
		}
	}

}
