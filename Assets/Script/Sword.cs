using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public float attackForce = 1.0f;

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			SlimeEnemy enemy = coll.gameObject.GetComponent<SlimeEnemy>();
			enemy.DealDamage();
			Vector2 hitForce = coll.contacts[0].point - (Vector2)transform.position;
			coll.rigidbody.AddForce(hitForce.normalized * attackForce, ForceMode2D.Impulse);
			print("Hit " + coll.gameObject.name + " with force: " + (hitForce.normalized * attackForce));
		}
	}

}
