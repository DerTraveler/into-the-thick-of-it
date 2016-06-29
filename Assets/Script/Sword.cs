/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public int attackDamage = 1;
	public float attackForce = 1.0f;

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			SlimeEnemy enemy = coll.gameObject.GetComponent<SlimeEnemy>();
			enemy.DealDamage(attackDamage);

			Vector2 hitForce = coll.contacts[0].point - (Vector2)transform.position;
			coll.rigidbody.AddForce(hitForce.normalized * attackForce, ForceMode2D.Impulse);
			print("Hit " + coll.gameObject.name + " with force: " + (hitForce.normalized * attackForce));
		}
	}

}
