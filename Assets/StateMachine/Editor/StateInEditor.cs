/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

namespace StateMachine.Editor {

    // Concrete State implementation used by the Editor.
    public class StateInEditor : State, ISerializationCallbackReceiver {

        #region Getter/Setter
        [SerializeField] int _stateId;

        public override int StateId {
            get {
                return _stateId;
            }
        }

        Rect _drawRect;

        public Rect DrawRect {
            get { return _drawRect; }
        }

        [SerializeField] Vector2 _position;

        public Vector2 Position {
            get { return _position; }
            set {
                _position = value;
                _drawRect.position = value;
            }
        }
        #endregion

        public void Initialize(int stateId, Vector2 position) {
            _stateId = stateId;
            name = StateMachineConstants.NEW_STATE_NAME;
            _position = position;
            CalcDrawRect();
        }

        void CalcDrawRect() {
            _drawRect = new Rect(Position.x, Position.y, StateMachineConstants.STATE_WIDTH, StateMachineConstants.STATE_HEIGHT);
        }

        #region ISerializationCallbackReceiver implementation
        public void OnBeforeSerialize() {
        }

        public void OnAfterDeserialize() {
            CalcDrawRect();
        }
        #endregion
    }

}
