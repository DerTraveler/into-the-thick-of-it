/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace StateMachine.Editor {

	public class StateInEditor : ScriptableObject {

		[System.NonSerialized]
		public State source;

		public string Name {
			get { return source.name; }
			set { source.name = value; }
		}

		public Vector2 Position {
			get { return source.position; }
			set { source.position = value; }
		}

		public void Initialize(State sourceState) {
			this.source = sourceState;
			this.name = sourceState.name;
		}

	}

}
