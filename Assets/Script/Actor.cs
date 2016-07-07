/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public abstract class Actor : MonoBehaviour {

	public GameObject body;
	private SpriteRenderer rend;

	private Animator _animator;
	public Animator Animator { get { return _animator; } }

	private Vector2 _faceDirection = new Vector2(0, -1);
	public Vector2 FaceDirection { 
		get { return _faceDirection; } 
		set { 
			_faceDirection = value;
			FlipIfNecessary();
		}
	}

	private Vector2 _moveDirection = new Vector2(0, 0);
	public Vector2 MoveDirection { 
		get { return _moveDirection; } 
		set { 
			_moveDirection = value;
			UpdateFaceDirection();
		}
	}

	private void UpdateFaceDirection() {
		if (Mathf.Approximately(MoveDirection.sqrMagnitude, 1.0f)) {
			FaceDirection = MoveDirection;
		}
	}

	private void FlipIfNecessary() {
		Vector3 scale = transform.localScale;
		float scaleX = Mathf.Abs(scale.x);
		scale.Set(Mathf.Approximately(FaceDirection.x, -1.0f) ? -scaleX: scaleX, scale.y, scale.z);
		transform.localScale = scale;
	}

	public string DirectedAnimationName(string animationName) {
		if (FaceDirection.y > 0) {
			return animationName + "Up";
		} else if (FaceDirection.y < 0) {
			return animationName + "Down";
		} else {
			return animationName + "Side";
		}
	}

	void Awake () {
		rend = body.GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();

		gameObject.AddComponent<PixelPerfectPositioner>();
		gameObject.AddComponent<KeepInBounds>();
	}

	void LateUpdate () {
		if (rend.isVisible) {
			// Dynamic draw order based on y-coordinate
			rend.sortingOrder = (int) (body.transform.position.y * 64.0 * -1.0f);
		}
	}

	public float GetAnimationTime(int layer = 0) {
		return Animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
	}

	public bool IsAnimationFinished(int layer = 0) {
		return GetAnimationTime(layer) >= 1.0f;
	}

	public Spawner spawnedBy = null;

	// Receive damage, returns true if the object was really hurt
	public abstract bool ReceiveDamage(int damage);

	protected void SendDeathNotification() {
		if (spawnedBy) {
			spawnedBy.NotifyOfDeath(this);
		}
	}

	protected void PlaySoundEffect(AudioClip clip) {
		AudioManager.instance.PlaySound(clip);
	}
}
