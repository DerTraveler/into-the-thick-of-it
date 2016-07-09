/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class SlimeEnemyIdle : SlimeEnemyAI  {

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			State<SlimeEnemy> aiDecision = base.HandleInput(subject);
			if (Mathf.Approximately(subject.CurrentStamina, subject.maxStamina)) {
				return aiDecision;
			}
			return null;
		}

		public override void Update(SlimeEnemy subject) {
			subject.Animator.Play(SlimeEnemy.Animations.IDLE);

			subject.Rest();
		}

	}

}

