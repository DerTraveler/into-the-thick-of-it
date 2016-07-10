/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;
using States;

public class SlimeEnemy : Actor {

	private Player player;

	public ParticleSystem deathAnimation;
	public float maxHitPoints = 3;

	public float maxStamina = 100.0f;
	public float staminaRegeneration = 60.0f;

	public float distancePerJump = 0.5f;
	public float staminaPerJump = 10.0f;

	public GameObject dropItem;
	public float dropChance = 0.1f;

	public static class Animations {
		public const string IDLE = "Idle";
		public const string JUMP = "Jump";
		public const string HURT = "Hurt";
		public const string RECOVER = "Recover";
	}

	public static class States {
		public static SlimeEnemyIdle IDLE = new SlimeEnemyIdle();
		public static SlimeEnemyFollow FOLLOW = new SlimeEnemyFollow();
		public static SlimeEnemyDead DEAD = new SlimeEnemyDead();
	}

	private readonly SlimeEnemyJumping _jumpingState = new SlimeEnemyJumping();

	public SlimeEnemyJumping JumpingState(Vector2 jumpGoal, State<SlimeEnemy> afterJump) {
		_jumpingState.jumpGoal = jumpGoal;
		_jumpingState.afterJumpState = afterJump;
		return _jumpingState;
	}

	private SlimeEnemyAI _state = States.IDLE;

	public float currentHitPoints;

	private float _currentStamina;
	public float CurrentStamina { get { return _currentStamina; } }

	float distanceFromPlayer {
		get { return (player.transform.position - transform.position).magnitude; }
	}

	public Vector2 PlayerDirection {
		get { return (player.transform.position - transform.position); }
	}

	void Start () {
		player = FindObjectOfType<Player>();
		currentHitPoints = maxHitPoints;
		_currentStamina = maxStamina;
	}

	// Update is called once per frame
	void FixedUpdate () {
		_state.Update(this);

		SlimeEnemyAI newState = _state.HandleInput(this) as SlimeEnemyAI;

		if (newState != null) {
			_state.Exit(this);
			newState.Entry(this);
			_state = newState;
		}
	}

	public float Rest() {
		float oldStamina = _currentStamina;
		_currentStamina = Mathf.Min(_currentStamina + staminaRegeneration * Time.deltaTime, maxStamina);
		return _currentStamina - oldStamina;
	}

	public float JumpExhaustion() {
		float oldStamina = _currentStamina;
		_currentStamina -= (staminaPerJump + Random.Range(-3, 3));
		return oldStamina - _currentStamina;
	}

	private bool IsPlayerInFrontOfMe() {
		Vector2 playerPos = player.transform.position;
		Rect attackRect = new Rect(transform.position.x - 0.6f, transform.position.y - 0.2f, 1.2f, 0.4f);
		return attackRect.Contains(playerPos);
	}

	public override bool ReceiveDamage(int damage) {
		return _state.ReceiveDamage(damage);
	}

	public void Die() {
		SendDeathNotification();
		body.GetComponent<SpriteRenderer>().enabled = false;
		deathAnimation.Play();

		if (Random.value < dropChance) {
			Instantiate(dropItem, transform.position, Quaternion.identity);
		}
		Destroy(gameObject, 0.5f);
	}
		
}
