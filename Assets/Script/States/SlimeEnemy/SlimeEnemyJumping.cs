/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

	public class SlimeEnemyJumping : SlimeEnemyAI  {

		public Vector2 jumpGoal;
		public State<SlimeEnemy> afterJumpState;

		private Vector2 _jumpOrigin;

		private const float _startTime = 0.305f;
		private const float _endTime = 0.78f;
		private float duration {
			get { return _endTime - _startTime; }
		}

		public override void Entry(SlimeEnemy subject) {
			subject.JumpExhaustion();

			_jumpOrigin = subject.transform.position;
			if (Mathf.Abs(jumpGoal.x) > Mathf.Abs(jumpGoal.y)) {
				subject.MoveDirection = new Vector2(Mathf.Sign(jumpGoal.x) * 1.0f, 0f);
			} else {
				subject.MoveDirection = new Vector2(0f, Mathf.Sign(jumpGoal.y) * 1.0f);
			}

			subject.Animator.Play(SlimeEnemy.Animations.JUMP, 0, 0f);
		}

		public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
			State<SlimeEnemy> aiDecision = base.HandleInput(subject);
			if (IsHighPriorityState(aiDecision))
				return aiDecision;
			
			return subject.IsAnimationFinished() ? afterJumpState : null;
		}

		public override void Update(SlimeEnemy subject) {
			if (subject.GetAnimationTime() > _startTime) {
				Vector2 newPos = _jumpOrigin + Mathf.Lerp(0, subject.distancePerJump, (subject.GetAnimationTime() - _startTime) / duration ) * subject.MoveDirection;
				subject.transform.position = newPos;
			}
		}

	}

}

