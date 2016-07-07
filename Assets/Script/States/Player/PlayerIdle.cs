/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class PlayerIdle : PlayerState {

		public override State<Player> HandleInput(Player subject) {
			subject.MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			if (subject.MoveDirection.sqrMagnitude > float.Epsilon) {
				return Player.States.WALKING;
			}
			return base.HandleInput(subject);
		}

		public override void Update(Player subject) {
			subject.Animator.Play(subject.DirectedAnimationName(Player.Animations.IDLE));
		}

	}

}

