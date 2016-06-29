using UnityEngine;
using System.Collections;

public class KnockbackDamageCollider : MonoBehaviour {

	public int attackDamage = 1;
	public float knockbackForce = 1.0f;

	void OnCollisionEnter2D(Collision2D coll) {
		WorldObject target = coll.gameObject.GetComponent<WorldObject>();

		if (target.ReceiveDamage(attackDamage)) {
			Vector2 hitForce = coll.contacts[0].point - (Vector2)transform.position;
			coll.rigidbody.AddForce(hitForce.normalized * knockbackForce, ForceMode2D.Impulse);
		}
	}

}
