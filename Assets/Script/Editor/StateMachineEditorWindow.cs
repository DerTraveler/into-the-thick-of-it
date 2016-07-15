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
		private GUISkin _skin;
		#endregion

		void OnGUI () {
			Event currentEvent = Event.current;

			GUI.BeginGroup(new Rect(CanvasPosition.x, CanvasPosition.y, StateMachineConstants.CANVAS_WIDTH, StateMachineConstants.CANVAS_HEIGHT));
			{
				foreach (State s in StateMachine.States) {
					DrawState(s);
				}
			}
			GUI.EndGroup();

			HandleContextMenu(currentEvent);
			HandleCanvasDrag(currentEvent);
		}

		private void DrawState(State state) {
			GUILayout.BeginArea(new Rect(state.position.x, state.position.y, StateMachineConstants.STATE_WIDTH, StateMachineConstants.STATE_HEIGHT), state.name, _skin.window);
			{
				
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
			}
		}

		[MenuItem("Window/State Machine Editor")]
		public static void ShowWindow() {
			Window.Show();
			Window.OnEnable();
		}

		void OnEnable() {
			AddEventHandlers();
			_skin = AssetDatabase.LoadAssetAtPath<GUISkin>(StateMachineConstants.SKIN_PATH);
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

		// Added to EditorApplication.playmodeStateChanged callback
		public static void PlaymodePersistence() {
			// First is current state, second is next state
			if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
				// Save Window state before play mode
				if (_window != null) {
					EditorPrefs.SetBool(StateMachineConstants.PREF_VISIBLE, true);
					EditorPrefs.SetInt(StateMachineConstants.PREF_INSTANCE, _window.StateMachine.GetInstanceID());
				}

			}
			if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode) {
				// Restore window state after play mode
				if (EditorPrefs.GetBool(StateMachineConstants.PREF_VISIBLE)) {
					if (EditorPrefs.GetInt(StateMachineConstants.PREF_INSTANCE) != 0) {
						OpenStateMachine(EditorPrefs.GetInt(StateMachineConstants.PREF_INSTANCE), 0);
					} else {
						ShowWindow();
					}
				}
			}
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
			statePosition.Set(statePosition.x - StateMachineConstants.STATE_WIDTH / 2f - CanvasPosition.x, 
							  statePosition.y - StateMachineConstants.STATE_HEIGHT / 2f - CanvasPosition.y);
			_stateMachineEditor.AddState(statePosition);
		}
		#endregion
		
		#region Drag and Drop
		private Vector2 _canvasPosition = new Vector2(0, 0);
		public Vector2 CanvasPosition {
			get { return _canvasPosition; }
			set { _canvasPosition.Set(Mathf.Clamp(value.x, -(StateMachineConstants.CANVAS_WIDTH - Screen.width), 0),
									  Mathf.Clamp(value.y, -(StateMachineConstants.CANVAS_HEIGHT - Screen.height), 0)); }
		}

		private void HandleCanvasDrag(Event e) {
			if (e.type == EventType.MouseDrag && e.button == 2) {
				CanvasPosition += e.delta;
				Repaint();
			}
		}
		#endregion
		
		#region Undo/Redo
		public static void AddEventHandlers() {
			if (_window != null)
				Undo.undoRedoPerformed += _window.Repaint;
			
			EditorPrefs.DeleteKey(StateMachineConstants.PREF_VISIBLE);
			EditorPrefs.DeleteKey(StateMachineConstants.PREF_INSTANCE);
		}

		public static void RemoveEventHandlers() {
			if (_window != null)
				Undo.undoRedoPerformed -= _window.Repaint;
		}
		#endregion
	}

}
