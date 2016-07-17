﻿/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections.Generic;

namespace StateMachine {

    [CreateAssetMenu(fileName = "NewStateMachine.asset", menuName = "State Machine", order = 101)]
    public class StateMachine : ScriptableObject {

        [SerializeField] int _nextStateId;
        [SerializeField] List<State> _states = new List<State>();

        public State[] States {
            get { return _states.ToArray(); }
        }

        public State AddState(Vector2 position) {
            var newState = new State(_nextStateId, position);
            _states.Add(newState);
            _nextStateId++;
            return newState;
        }

        public void RemoveState(State state) {
            _states.Remove(state);
        }

        public State GetState(int id) {
            foreach (State state in _states) {
                if (state.id == id)
                    return state;
            }
            return null;
        }

    }
}