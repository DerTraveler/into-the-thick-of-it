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

		private Rect _drawRect;
		public Rect DrawRect {
			get { return _drawRect; }
			set { _drawRect = value; source.position.Set(value.x, value.y); }
		}

		public void Initialize(State sourceState) {
			this.source = sourceState;
			this.name = sourceState.name;
			this._drawRect = new Rect(sourceState.position.x, sourceState.position.y, 0, 0);
		}

	}

}
