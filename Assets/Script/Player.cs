/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 1.0f;

	private Animator animator;

	public enum State {
		Idle,
		Walking,
		Attacking
	}

	[SerializeField]
	private State state = State.Idle;
	[SerializeField]
	private string currentAnimation = "IdleDown";
	[SerializeField]
	private Vector2 moveDirection = new Vector2(0, 0);
	[SerializeField]
	private Vector2 faceDirection = new Vector2(0, -1);
	[SerializeField]
	private bool directionPressed = false;
	[SerializeField]
	private bool attackTrigger = false;

	void Start () {
		animator = GetComponent<Animator>();
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

		if (Mathf.Approximately(moveDirection.sqrMagnitude, 1.0f)) {
			faceDirection.Set(moveDirection.x, moveDirection.y);
		}

		attackTrigger = Input.GetButtonDown("Attack");
	}

	private void UpdateState() {
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
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) {
				state = directionPressed ? State.Walking : State.Idle;
			}
			break;
		}
		if (attackTrigger && state != State.Attacking) {
			state = State.Attacking;
		}
	}

	private void PlayAnimation() {
		FlipIfNecessary();

		string nextAnimation = currentAnimation;
		switch (state) {
		case State.Idle:
			nextAnimation = "Idle" + getDirectionName();
			break;
		case State.Walking:
			nextAnimation = "Walk" + getDirectionName();
			break;
		case State.Attacking:
			nextAnimation = "Attack" + getDirectionName();
			break;
		}
		if (nextAnimation != currentAnimation) {
			currentAnimation = nextAnimation;
			animator.Play(currentAnimation);
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

	private void UpdateTransform() {
		if (state == State.Walking) {
			Vector3 newPos = transform.position + (Vector3)(moveDirection * speed * Time.deltaTime);
			transform.position = newPos;
		}
	}
}
