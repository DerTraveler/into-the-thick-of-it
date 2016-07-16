/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace StateMachine.Editor {

    public class StateInEditor : ScriptableObject {

        #region Getter/Setter
        public State source;

        public string Name {
            get { return source.name; }
            set { source.name = value; }
        }

        [SerializeField]
        Rect _drawRect;

        public Rect DrawRect {
            get { return _drawRect; }
            set {
                _drawRect = value;
                source.position.Set(value.x, value.y);
            }
        }
        #endregion

        public void Initialize(State sourceState) {
            source = sourceState;
            name = sourceState.name;
            _drawRect = new Rect(sourceState.position.x, sourceState.position.y, StateMachineConstants.STATE_WIDTH, StateMachineConstants.STATE_HEIGHT);
        }

    }

}
