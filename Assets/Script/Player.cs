/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using States;

public class Player : Actor {

	public int maxHealth = 5;
	private int _health;
	public int Health {
		get { return _health; }
		set { _health = Mathf.Min(value, maxHealth); UpdateHealthText(); }
	}
	public float speed = 2.5f;

	public UnityEngine.UI.Text healthText;
	public UnityEngine.UI.Text gameOverText;

	public static class Animations {
		public const string IDLE = "Idle";
		public const string WALKING = "Walking";
		public const string HURT = "Hurt";
		public const string ATTACKING = "Attacking";
	}

	public static class States {
		public static PlayerIdle IDLE = new PlayerIdle();
		public static PlayerWalking WALKING = new PlayerWalking();
		public static PlayerDead DEAD = new PlayerDead();
	}

	private PlayerBase _state = States.IDLE;

	void Start () {
		Animator.SetFloat("Speed", speed);
		Health = maxHealth;
	}

	// Update is called once per frame
	void Update () {
		_state.Update(this);

		PlayerBase newState = _state.HandleInput(this) as PlayerBase;

		if (newState != null) {
			_state.Exit(this);
			newState.Entry(this);
			_state = newState;
		}
	}

	public override bool ReceiveDamage(int damage) {
		return _state.ReceiveDamage(damage);
	}

	private void UpdateHealthText() {
		healthText.color = _health < 2 ? Color.red : Color.yellow;
		healthText.text = _health > 0 ? new string('0', _health) : "";
	}

	public void TriggerGameOver() {
		GetComponent<SpriteRenderer>().enabled = false;
		Destroy(GetComponent<Collider2D>());
		Destroy(GetComponent<Rigidbody2D>());
		Destroy(transform.Find("Sword").gameObject);
		gameOverText.enabled = true;
	}

}
