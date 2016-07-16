/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace StateMachine.Editor {

	[CustomEditor(typeof(StateMachine))]
	public class StateMachineEditor : UnityEditor.Editor {

		private static UnityEditor.Editor _editor;

		public static StateMachineEditor GetEditor(StateMachine stateMachine) {
			UnityEditor.Editor.CreateCachedEditor(stateMachine, typeof(StateMachineEditor), ref _editor);
			return _editor as StateMachineEditor;
		}

		#region ScriptableObject-Wrapper
		public List<StateInEditor> states = new List<StateInEditor>();

		void OnEnable () {
			ReadStatesIntoEditor();
		}

		private void ReadStatesIntoEditor() {
			StateMachine stateMachine = target as StateMachine;
			foreach (State state in stateMachine.States) {
				StateInEditor converted = ScriptableObject.CreateInstance<StateInEditor>();
				converted.Initialize(state);
				states.Add(converted);
			}
		}
		#endregion

		#region Actions
		public State AddState(Vector2 position) {
			StateMachine stateMachine = target as StateMachine;
			Undo.RecordObject(stateMachine, "Add new State");
			State newState = stateMachine.AddState(position);

			StateInEditor inEditor = ScriptableObject.CreateInstance<StateInEditor>();
			Undo.RegisterCreatedObjectUndo(inEditor, "Add new State");
			inEditor.Initialize(newState);

			Undo.RecordObject(this, "Add new State");
			states.Add(inEditor);

			EditorUtility.SetDirty(stateMachine);

			serializedObject.ApplyModifiedProperties();
			return newState;
		}

		public void RemoveState(StateInEditor state) {
			StateMachine stateMachine = target as StateMachine;
			Undo.RecordObject(stateMachine, "Remove state");
			stateMachine.RemoveState(state.source);

			Undo.DestroyObjectImmediate(state);

			Undo.RecordObject(this, "Remove state");
			states.Remove(state);

			EditorUtility.SetDirty(stateMachine);

			serializedObject.ApplyModifiedProperties();
		}
		#endregion

		public override void OnInspectorGUI() {
			serializedObject.Update();

			serializedObject.ApplyModifiedProperties();
		}
	}

}
