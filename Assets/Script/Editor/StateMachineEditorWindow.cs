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
			set { _stateMachine = value; _stateMachineEditor = StateMachineEditor.GetEditor(value); }
		}
		private void EnsureStateMachine() {
			if (_stateMachine == null) {
				this.StateMachine = ScriptableObject.CreateInstance<StateMachine>();
			}
		}

		private StateMachineEditor _stateMachineEditor;
		#endregion

		void OnGUI () {
			Event currentEvent = Event.current;

			GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
			{
				foreach (State s in StateMachine.States) {
					DrawState(s);
				}
			}
			GUI.EndGroup();

			HandleContextMenu(currentEvent);
		}

		private void DrawState(State state) {
			GUILayout.BeginArea(new Rect(state.position.x, state.position.y, StateMachineConstants.STATE_WIDTH, StateMachineConstants.STATE_HEIGHT));
			{
				GUILayout.Box(state.name, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			}
			GUILayout.EndArea();
		}

		#region Window Lifecycle
		private static StateMachineEditorWindow _window;
		public static StateMachineEditorWindow Window {
			get { EnsureWindow(); return _window; }
		}

		private static void EnsureWindow() {
			if (_window == null) {
				_window = GetWindow<StateMachineEditorWindow>(false, "State Machine Editor");
				AddEventHandlers();
			}
		}

		[MenuItem("Window/State Machine Editor")]
		public static void ShowWindow() {
			Window.Show();
		}

		void OnDestroy() {
			RemoveEventHandlers();
		}

		[UnityEditor.Callbacks.OnOpenAsset(1)]
		public static bool OpenStateMachine(int instanceID, int line) {
			if (Selection.activeObject as StateMachine != null) {
				ShowWindow();
				Window.StateMachine = AssetDatabase.LoadAssetAtPath<StateMachine>(AssetDatabase.GetAssetPath(instanceID));
				Window.Repaint();
				return true;
			}
			return false;
		}
		#endregion

		#region Context Menu
		private void HandleContextMenu(Event ev) {
			if (ev.type == EventType.ContextClick) {
				Vector2 mousePos = ev.mousePosition;
				GenericMenu contextMenu = new GenericMenu();
				contextMenu.AddItem(new GUIContent("Create State"), false, this.CreateState, mousePos);
				contextMenu.ShowAsContext();
				ev.Use();
			}
		}

		private void CreateState(object position) {
			Vector2 statePosition = (Vector2) position;
			statePosition.Set(statePosition.x - StateMachineConstants.STATE_WIDTH / 2f, 
				              statePosition.y - StateMachineConstants.STATE_HEIGHT / 2f);
			_stateMachineEditor.AddState(statePosition);
		}
		#endregion

		#region Undo/Redo
		public static void AddEventHandlers() {
			if (_window != null)
				Undo.undoRedoPerformed += _window.Repaint;
		}

		public static void RemoveEventHandlers() {
			if (_window != null)
				Undo.undoRedoPerformed -= _window.Repaint;
		}
		#endregion
	}

}
