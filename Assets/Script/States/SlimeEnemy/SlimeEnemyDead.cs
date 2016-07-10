/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class SlimeEnemyDead : SlimeEnemyAI  {

		public override void Entry(SlimeEnemy subject) {
			subject.Die();
		}

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			return null;
		}

		public override bool ReceiveDamage(int damage) { 
			return false;
		}

	}

}

