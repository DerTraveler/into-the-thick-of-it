/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace StateMachine {

    [System.Serializable]
    public class State {

        [SerializeField] int _id;

        public int id {
            get { return _id; }
        }

        public string name;
        public Vector2 position;

        public State(int id, Vector2 position) {
            _id = id;
            name = "NewState";
            this.position = position;
        }

    }

}
