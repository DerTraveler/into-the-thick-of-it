/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace States {

    public class SlimeEnemyBiting : SlimeEnemyAI {

        public override void Entry(SlimeEnemy subject) {
            subject.Animator.Play(SlimeEnemy.Animations.BITE, 0, 0f);
        }

        public override State<SlimeEnemy> HandleInput(SlimeEnemy subject) {
            State<SlimeEnemy> aiDecision = base.HandleInput(subject);
            if (IsHighPriorityState(aiDecision))
                return aiDecision;
			
            return subject.IsAnimationFinished() ? 
                aiDecision : null;
        }

    }

}

