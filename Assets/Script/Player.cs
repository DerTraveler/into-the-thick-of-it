/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private int direction;

	void Start () {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Horizontal") != 0) {
			direction = (int) Input.GetAxisRaw("Horizontal") * 2;
			spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") < 0;
		} else if (Input.GetAxisRaw("Vertical") != 0) {
			direction = (int) Input.GetAxisRaw("Vertical");
		}
		animator.SetInteger("Direction", direction);
	}
}
