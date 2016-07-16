/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace States {

    public class PlayerAttack : PlayerBase {

        PlayerBase _previous;

        public PlayerAttack(PlayerBase previous) {
            _previous = previous;
        }

        public override State<Player> HandleInput(Player subject) {
            return subject.IsAnimationFinished() ? 
                _previous : null;
        }

        public override void Entry(Player subject) {
            subject.Animator.Play(subject.DirectedAnimationName(Player.Animations.ATTACKING), 0, 0f);
        }

    }

}

