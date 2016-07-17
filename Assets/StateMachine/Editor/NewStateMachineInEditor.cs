/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections.Generic;

namespace StateMachine.Editor {

    [CreateAssetMenu(fileName = "NewStateMachine.asset", menuName = "State Machine", order = 101)]
    public class NewStateMachineInEditor : NewStateMachine {

        [SerializeField] List<NewStateInEditor> _states = new List<NewStateInEditor>();

        public override NewState[] States { 
            get { return _states.ToArray(); } 
        }

        [SerializeField] int _nextId = 1;

        public NewStateInEditor AddState(Vector2 position) {
            var newState = ScriptableObject.CreateInstance<NewStateInEditor>();
            newState.Initialize(_nextId, position);
            _nextId++;

            _states.Add(newState);

            return newState;
        }

        public bool RemoveState(NewStateInEditor state) {
            return _states.Remove(state);
        }

    }
}
