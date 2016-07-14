/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace StateMachine.Editor {

	public class StateMachineEditorWindow : EditorWindow {

		#region Getter/Setter
		private StateMachine _stateMachine;
		public StateMachine StateMachine {
			get { EnsureStateMachine(); return _stateMachine; }
			set { _stateMachine = value; }
		}
		private void EnsureStateMachine() {
			if (_stateMachine == null) {
				this.StateMachine = ScriptableObject.CreateInstance<StateMachine>();
			}
		}
		#endregion

		private static StateMachineEditorWindow _window;
		
		[MenuItem("Window/State Machine Editor")]
		public static void ShowWindow() {
			_window = GetWindow<StateMachineEditorWindow>();
			_window.titleContent = new GUIContent("State Machine Editor");
		}

		[UnityEditor.Callbacks.OnOpenAsset(1)]
		public static bool OpenStateMachine(int instanceID, int line) {
			if (Selection.activeObject as StateMachine != null) {
				ShowWindow();
				_window.StateMachine = AssetDatabase.LoadAssetAtPath<StateMachine>(AssetDatabase.GetAssetPath(instanceID));
				return true;
			}
			return false;
		}

		void OnGUI () {
			GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
			{
				foreach (State s in StateMachine.States) {
					DrawState(s);
				}
			}
			GUI.EndGroup();
		}

		private void DrawState(State state) {
			GUILayout.BeginArea(new Rect(state.position.x, state.position.y, StateMachineConstants.STATE_WIDTH, StateMachineConstants.STATE_HEIGHT));
			{
				GUILayout.Box(state.name, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			}
			GUILayout.EndArea();
		}



	}

}
