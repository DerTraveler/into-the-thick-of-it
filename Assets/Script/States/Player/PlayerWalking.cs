/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

    public class PlayerWalking : PlayerBase {

        public override State<Player> HandleInput(Player subject) {
            subject.MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            return Mathf.Approximately(subject.MoveDirection.sqrMagnitude, 0f) ? 
                Player.States.IDLE : base.HandleInput(subject);
        }

        public override void Update(Player subject) {
            subject.Animator.Play(subject.DirectedAnimationName(Player.Animations.WALKING));
			
            Vector3 newPos = subject.transform.position + (Vector3) (subject.MoveDirection * subject.speed * Time.deltaTime);
            subject.transform.position = newPos;
        }

    }

}

