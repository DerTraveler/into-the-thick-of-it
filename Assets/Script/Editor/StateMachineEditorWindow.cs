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

				return true;
			}
			return false;
		}

		void OnGUI () {
			
		}


	}

}
