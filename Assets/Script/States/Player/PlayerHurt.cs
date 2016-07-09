/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class PlayerHurt : PlayerBase {

		private PlayerBase _previous;
		private int _damage;

		public PlayerHurt(PlayerBase previous, int damage) {
			_previous = previous;
			_damage = damage;
		}

		public override void Entry(Player subject) {
			subject.Health -= _damage;
			subject.Animator.Play(subject.DirectedAnimationName(Player.Animations.HURT), 0, 0f);
		}

		public override State<Player> HandleInput(Player subject) {
			if (subject.IsAnimationFinished()) {
				return _previous;
			}
			return null;
		}

		public override void Update(Player subject) { }

		public override bool ReceiveDamage(int damage) { 
			return false;
		}

	}

}

