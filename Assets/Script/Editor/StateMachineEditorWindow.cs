/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;

namespace StateMachine.Editor {

    public class StateMachineEditorWindow : EditorWindow {
        #region Getter/Setter
        StateMachine _stateMachine;

        public StateMachine StateMachine {
            get { return _stateMachine; }
            set {
                _stateMachine = value;
                _stateMachineEditor = StateMachineEditor.GetEditor(value);
                _stateMachineEditor.ReadStatesIntoEditor();
            }
        }

        StateMachineEditor _stateMachineEditor;

        GUISkin _skin;
        #endregion

        void OnGUI() {
            if (StateMachine != null) {
                Event currentEvent = Event.current;

                GUI.BeginGroup(new Rect(CanvasPosition.x, CanvasPosition.y, StateMachineConstants.CANVAS_WIDTH, StateMachineConstants.CANVAS_HEIGHT));
                {
                    foreach (StateInEditor s in _stateMachineEditor.states) {
                        DrawState(s);
                    }
                }
                GUI.EndGroup();

                HandleSelection(currentEvent);
                HandleContextMenu(currentEvent);
                HandleCanvasDrag(currentEvent);
            }
        }

        void DrawState(StateInEditor state) {
            GUILayout.BeginArea(state.DrawRect, state.Name, _skin.window);
            {

            }
            GUILayout.EndArea();
        }

        StateInEditor ClickedState(Vector2 mousePos) {
            foreach (StateInEditor state in _stateMachineEditor.states) {
                if (state.DrawRect.Contains(mousePos - _canvasPosition))
                    return state;
            }
            return null;
        }

        #region Window Lifecycle
        static StateMachineEditorWindow _window;

        public static StateMachineEditorWindow Window {
            get {
                EnsureWindow();
                return _window;
            }
        }

        static void EnsureWindow() {
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

        #region Selection
        void HandleSelection(Event ev) {
            if (ev.type == EventType.MouseDown && ev.button == 0) {
                Vector2 mousePos = ev.mousePosition;
                StateInEditor clickedState = ClickedState(mousePos);

                if (clickedState != null)
                    SelectState(clickedState);
                else
                    Deselect();

                ev.Use();
            }
        }

        void SelectState(StateInEditor state) {
            Selection.activeObject = state;
        }

        void Deselect() {
            Selection.activeObject = StateMachine;
        }
        #endregion

        #region Context Menu
        void HandleContextMenu(Event ev) {
            if (ev.type == EventType.ContextClick) {
                Vector2 mousePos = ev.mousePosition;
                StateInEditor clickedState = ClickedState(mousePos);

                var contextMenu = new GenericMenu();
                if (clickedState != null) {
                    SelectState(clickedState);
                    AddStateContextMenu(contextMenu, clickedState);
                } else {
                    Deselect();
                    AddBackgroundContextMenu(contextMenu, mousePos);
                }
                contextMenu.ShowAsContext();

                ev.Use();
            }
        }

        void AddBackgroundContextMenu(GenericMenu contextMenu, Vector2 mousePos) {
            contextMenu.AddItem(new GUIContent("Create State"), false, CreateState, mousePos);
        }

        void AddStateContextMenu(GenericMenu contextMenu, StateInEditor state) {
            contextMenu.AddItem(new GUIContent("Delete State"), false, RemoveState, state);
        }

        void CreateState(object argument) {
            var statePosition = (Vector2) argument;
            statePosition.Set(statePosition.x - StateMachineConstants.STATE_WIDTH / 2f - CanvasPosition.x,
                              statePosition.y - StateMachineConstants.STATE_HEIGHT / 2f - CanvasPosition.y);
            StateInEditor created = _stateMachineEditor.AddState(statePosition);
            SelectState(created);
        }

        void RemoveState(object argument) {
            Deselect();
            var state = (StateInEditor) argument;
            _stateMachineEditor.RemoveState(state);
        }
        #endregion

        #region Drag and Drop
        Vector2 _canvasPosition = new Vector2(0, 0);

        public Vector2 CanvasPosition {
            get { return _canvasPosition; }
            set {
                _canvasPosition.Set(Mathf.Clamp(value.x, -(StateMachineConstants.CANVAS_WIDTH - Screen.width), 0),
                                    Mathf.Clamp(value.y, -(StateMachineConstants.CANVAS_HEIGHT - Screen.height), 0));
            }
        }

        void HandleCanvasDrag(Event e) {
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
