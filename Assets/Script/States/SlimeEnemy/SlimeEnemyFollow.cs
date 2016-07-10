/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class SlimeEnemyFollow : State<SlimeEnemy>  {

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			if (subject.CurrentStamina > subject.staminaPerJump) {
				return subject.JumpingState(subject.PlayerDirection, this);
			} else {
				return SlimeEnemy.States.IDLE;
			}
		}


	}

}

