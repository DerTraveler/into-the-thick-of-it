/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class SlimeEnemyHurt : SlimeEnemyAI  {

		private SlimeEnemyAI _previous;
		private int _damage;
		private int _hurtStage = 0;  // 1 Hurt animation, 2 Small Jump, 3 Recover shake, 4 finished

		public SlimeEnemyHurt(SlimeEnemyAI previous, int damage) {
			_previous = previous;
			_damage = damage;
		}

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			return _hurtStage == 4 ? _previous : null;
		}

		public override void Update(SlimeEnemy subject) {
			switch (_hurtStage) {
			case 0:
				subject.currentHitPoints -= _damage;

				subject.Animator.Play(SlimeEnemy.Animations.HURT, 0, 0f);
				_hurtStage = 1;
				break;
			case 1:
				if (subject.IsAnimationFinished()) {
					_hurtStage = 4;
				}
				break;
			}

		}

		public override bool ReceiveDamage(int damage) { 
			if (_hurtStage > 1) {
				_damage = damage;
				_hurtStage = 0;
				return true;
			}
			return false;
		}

	}

}

