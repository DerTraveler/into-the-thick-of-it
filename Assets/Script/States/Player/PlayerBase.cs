/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace States {

    public abstract class PlayerBase : State<Player> {

        int _receivedDamage;

        public override State<Player> HandleInput(Player subject) {
            if (subject.Health <= 0) {
                return Player.States.DEAD;	
            } else if (_receivedDamage > 0) {
                int damage = _receivedDamage;
                _receivedDamage = 0;
                return new PlayerHurt(this, damage);
            } else if (Input.GetButtonDown("Attack")) {
                return new PlayerAttack(this);
            }
            return null;
        }

        public virtual bool ReceiveDamage(int damage) { 
            _receivedDamage = damage;
            return true;
        }

    }

}

