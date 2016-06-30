/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class SlimeEnemy : WorldObject {

	private Player player;

	public ParticleSystem deathAnimation;
	public float maxHitPoints = 3;

	public float maxStamina = 100.0f;
	public float staminaRegeneration = 60.0f;

	public float distancePerJump = 0.5f;
	public float staminaPerJump = 10.0f;

	public GameObject dropItem;
	public float dropChance = 0.1f;

	public enum State {
		Idle,
		Following,
		Hurt,
		Recover,
		Bite
	}

	[SerializeField]
	private State state = State.Idle;

	[SerializeField]
	private float currentHitPoints;
	[SerializeField]
	private float currentStamina;

	[SerializeField]
	private bool jumpTrigger = false;
	[SerializeField]
	private bool jumping = false;
	[SerializeField]
	private Vector2 jumpOrigin;
	[SerializeField]
	private Vector2 jumpDirection = new Vector2(1, 0);

	[SerializeField]
	private bool lookToLeft = false;

	float distanceFromPlayer {
		get { return (player.transform.position - transform.position).magnitude; }
	}

	Vector2 playerDirection {
		get { return (player.transform.position - transform.position); }
	}

	void Start () {
		player = FindObjectOfType<Player>();
		currentHitPoints = maxHitPoints;
		currentStamina = maxStamina;
	}

	// Update is called once per frame
	void FixedUpdate () {
		UpdateState();

		PlayAnimation();

		UpdateTransform();
	}

	private void UpdateState() {
		dropChance = player.Health == 1 ? 0.3f : 0.1f;

		switch(state) {
		case State.Idle:
			currentStamina = Mathf.Min(currentStamina + staminaRegeneration * Time.deltaTime, maxStamina);
			if (Mathf.Approximately(currentStamina, maxStamina)) {
				state = state = NextNormalAction();
			}
			break;
		case State.Following:
			if (jumping && IsAnimationFinished()) {
				jumping = false;
			} else if (jumpTrigger) {
				jumpTrigger = false;
				jumping = true;
			} else if (!jumping) {
				if (IsPlayerInFrontOfMe()){
					lookToLeft = player.transform.position.x < transform.position.x;
					state = State.Bite;
				}
				if (currentStamina >= staminaPerJump) {
					Jump(playerDirection);
				} else {
					state = State.Idle;
				}
			}
			break;
		case State.Hurt:
			if (IsAnimationFinished()) {
				state = State.Recover;
			}
			break;
		case State.Recover:
		case State.Bite:
			if (IsAnimationFinished()) {
				state = NextNormalAction();
			}
			break;
		}
	}

	private State NextNormalAction() {
		return State.Following;
	}

	private bool IsPlayerInFrontOfMe() {
		Vector2 playerPos = player.transform.position;
		Rect attackRect = new Rect(transform.position.x - 0.6f, transform.position.y - 0.2f, 1.2f, 0.4f);
		return attackRect.Contains(playerPos);
	}

	private void Jump(Vector2 direction) {
		jumpTrigger = true;

		jumpOrigin = transform.position;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
			jumpDirection.Set(Mathf.Sign(direction.x) * 1.0f, 0f);
			lookToLeft = jumpDirection.x < 0;
		} else {
			jumpDirection.Set(0f, Mathf.Sign(direction.y) * 1.0f);
		}

		currentStamina -= (staminaPerJump + Random.Range(-3, 3));
	}

	private void PlayAnimation() {
		FlipIfNecessary();

		switch(state) {
		case State.Idle:
			animator.Play("Idle");
			break;
		case State.Hurt:
			animator.Play("Hurt");
			break;
		case State.Recover:
			animator.Play("Recover");
			break;
		case State.Bite:
			animator.Play("Bite");
			break;
		default:
			if (jumpTrigger) {
				animator.Play("Jump", 0, 0);
			}
			break;
		}
	}

	private void FlipIfNecessary() {
		Vector3 scale = transform.localScale;
		float scaleX = Mathf.Abs(scale.x);
		scale.Set(lookToLeft ? -scaleX: scaleX, scale.y, scale.z);
		transform.localScale = scale;
	}

	private const float jumpStartTime = 0.305f;
	private const float jumpEndTime = 0.78f;
	private float jumpDuration {
		get { return jumpEndTime - jumpStartTime; }
	}

	private void UpdateTransform() {
		if (jumping && GetAnimationTime() > jumpStartTime) {
			Vector2 newPos = jumpOrigin + Mathf.Lerp(0, distancePerJump, (GetAnimationTime() - jumpStartTime) / jumpDuration ) * jumpDirection;
			transform.position = newPos;
		}
	}

	public override bool ReceiveDamage(int damage) {
		if (state == State.Hurt) {
			return false;
		}

		jumpTrigger = false;
		jumping = false;
		state = State.Hurt;

		currentHitPoints -= damage;
		if (currentHitPoints <= 0) {
			Die();
		}
		return true;	
	}

	private void Die() {
		SendDeathNotification();
		body.GetComponent<SpriteRenderer>().enabled = false;
		deathAnimation.Play();

		if (Random.value < dropChance) {
			Instantiate(dropItem, transform.position, Quaternion.identity);
		}
		Destroy(gameObject, 0.5f);
	}
		
}
