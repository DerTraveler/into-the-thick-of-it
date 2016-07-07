/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

	public enum State {
		Idle,
		Walking,
		Attacking,
		Hurt,
		Dead
	}

	private State state = State.Idle;
	private string currentAnimation = "IdleDown";

	private Vector2 moveDirection = new Vector2(0, 0);
	private Vector2 faceDirection = new Vector2(0, -1);
	private bool directionPressed = false;

	private bool attackTrigger = false;
	private bool hurtTrigger = false;

	void Start () {
		Animator.SetFloat("Speed", speed);
		Health = maxHealth;
	}

	// Update is called once per frame
	void Update () {
		ReadInput();

		UpdateState();

		PlayAnimation();

		UpdateTransform();
	}

	private void ReadInput() {
		moveDirection.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		directionPressed = moveDirection.sqrMagnitude > float.Epsilon;

		attackTrigger = Input.GetButtonDown("Attack");

		if (Input.GetButtonDown("Cancel")) {
			Application.Quit();
		}
	}

	private void UpdateState() {
		if (_health <= 0 && state != State.Dead) {
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<Collider2D>().enabled = false;
			gameOverText.enabled = true;
			state = State.Dead;
		} else {
			switch(state) {
			case State.Idle:
				if (directionPressed) {
					state = State.Walking;
				}
				break;
			case State.Walking:
				if (!directionPressed) {
					state = State.Idle;
				}
				break;
			case State.Attacking:
				ResumeAfterAnimation();
				break;
			case State.Hurt:
				if (hurtTrigger) {
					hurtTrigger = false;
				} else {
					ResumeAfterAnimation();
				}
				break;
			case State.Dead:
				if (attackTrigger) {
					int scene = SceneManager.GetActiveScene().buildIndex;
					SceneManager.LoadScene(scene, LoadSceneMode.Single);
				}
				break;
			}

			if (attackTrigger && (state == State.Idle || state == State.Walking)) {
				state = State.Attacking;
			}
		}
	}

	private void ResumeAfterAnimation() {
		if (IsAnimationFinished()) {
			// Return to walk or idle after end of animation
			state = directionPressed ? State.Walking : State.Idle;
		}
	}

	private void PlayAnimation() {
		if (state != State.Dead) {
			SetFaceDirection();

			string nextAnimation = state.ToString() + getDirectionName();

			if (nextAnimation != currentAnimation) {
				currentAnimation = nextAnimation;
				Animator.Play(currentAnimation);
			}
		}
	}

	private void FlipIfNecessary() {
		Vector3 scale = transform.localScale;
		float scaleX = Mathf.Abs(scale.x);
		scale.Set(Mathf.Approximately(faceDirection.x, -1.0f) ? -scaleX: scaleX, scale.y, scale.z);
		transform.localScale = scale;
	}

	private string getDirectionName() {
		if (faceDirection.y > 0) {
			return "Up";
		} else if (faceDirection.y < 0) {
			return "Down";
		} else {
			return "Side";
		}
	}

	private readonly IList<State> FixedDirectionStates = new List<State> { State.Attacking, State.Hurt }.AsReadOnly();

	private void SetFaceDirection() {
		if (!FixedDirectionStates.Contains(state)) {
			if (Mathf.Approximately(moveDirection.sqrMagnitude, 1.0f)) {
				faceDirection.Set(moveDirection.x, moveDirection.y);
			}
			FlipIfNecessary();
		}
	}

	private void UpdateTransform() {
		if (state == State.Walking) {
			Vector3 newPos = transform.position + (Vector3)(moveDirection * speed * Time.deltaTime);
			transform.position = newPos;
		}
	}

	public override bool ReceiveDamage(int damage) {
		if (state == State.Hurt) {
			return false;
		}

		state = State.Hurt;
		hurtTrigger = true;

		Health -= damage;

		return true;
	}

	private void UpdateHealthText() {
		healthText.color = _health < 2 ? Color.red : Color.yellow;
		healthText.text = _health > 0 ? new string('0', _health) : "";
	}

}
