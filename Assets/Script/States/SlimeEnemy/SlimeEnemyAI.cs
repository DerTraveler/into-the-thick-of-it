/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public abstract class SlimeEnemyAI : State<SlimeEnemy>  {

		private int _receivedDamage = 0;

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			if (_receivedDamage > 0) {
				int damage = _receivedDamage;
				_receivedDamage = 0;
				return new SlimeEnemyHurt(this, damage);
			} else {
				return SlimeEnemy.States.FOLLOW;	
			}
		}

		public virtual bool ReceiveDamage(int damage) { 
			_receivedDamage = damage;
			return true;
		}

		protected bool IsHighPriorityState(State<SlimeEnemy> state) {
			return state is SlimeEnemyHurt;
		}

	}

}

