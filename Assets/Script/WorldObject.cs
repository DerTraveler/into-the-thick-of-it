/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public abstract class WorldObject : MonoBehaviour {

	public GameObject body;
	private SpriteRenderer rend;

	private Animator _animator;
	public Animator Animator { get { return _animator; } }

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
