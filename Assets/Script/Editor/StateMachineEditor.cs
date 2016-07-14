/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace StateMachine.Editor {

	[CustomEditor(typeof(StateMachine))]
	public class StateMachineEditor : UnityEditor.Editor {

		private static UnityEditor.Editor _editor;

		public static StateMachineEditor GetEditor(StateMachine stateMachine) {
			UnityEditor.Editor.CreateCachedEditor(stateMachine, typeof(StateMachineEditor), ref _editor);
			return _editor as StateMachineEditor;
		}

		public State AddState(Vector2 position) {
			StateMachine stateMachine = target as StateMachine;

			State newState = stateMachine.AddState(position);

			serializedObject.ApplyModifiedProperties();
			return newState;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}

}
