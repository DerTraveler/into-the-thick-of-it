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
        EditorWindowState _windowState;

        public StateMachineInEditor StateMachine {
            get { return _windowState.stateMachine; }
            set {
                _windowState.stateMachine = value;
                _stateMachineEditor = StateMachineEditor.GetEditor(value);
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
                    foreach (StateInEditor s in StateMachine.States) {
                        DrawState(s);
                    }
                }
                GUI.EndGroup();

                HandleSelection(currentEvent);
                HandleStateDrag(currentEvent);
                HandleContextMenu(currentEvent);
                HandleCanvasDrag(currentEvent);
            }
        }

        void DrawState(StateInEditor state) {
            GUILayout.BeginArea(_dragged == null || _dragged != state ? state.DrawRect : _dragRect, 
                                state.name, 
                                SelectedStateId == state.GetInstanceID() ? _skin.customStyles[1] : _skin.customStyles[0]);
            {

            }
            GUILayout.EndArea();
        }

        StateInEditor ClickedState(Vector2 mousePos) {
            foreach (StateInEditor state in StateMachine.States) {
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
            _windowState = ScriptableObject.CreateInstance<EditorWindowState>();
            AddEventHandlers();
            _skin = Resources.Load<GUISkin>(StateMachineConstants.SKIN_PATH);
        }

        void OnDestroy() {
            RemoveEventHandlers();
            Resources.UnloadAsset(_skin);
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OpenStateMachine(int instanceID, int line) {
            if (EditorUtility.InstanceIDToObject(instanceID) as StateMachine != null) {
                ShowWindow();
                Window.StateMachine = AssetDatabase.LoadAssetAtPath<StateMachineInEditor>(AssetDatabase.GetAssetPath(instanceID));
                Window.Repaint();
                return true;
            }
            return false;
        }

        // Added to EditorApplication.playmodeStateChanged callback
        public static void PlaymodePersistence() {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
                // Close Window state before play mode
                // TODO: Proper Window Serialization and Restoration
                if (_window != null)
                    _window.Close();
            }
        }
        #endregion

        #region Selection
        int SelectedStateId {
            get { return _windowState.selectedStateId; }
            set { _windowState.selectedStateId = value; }
        }

        void HandleSelection(Event ev) {
            if (ev.type == EventType.MouseDown && ev.button == 0) {
                Vector2 mousePos = ev.mousePosition;
                StateInEditor clickedState = ClickedState(mousePos);

                if (clickedState != null && clickedState.GetInstanceID() != SelectedStateId) {
                    SelectState(clickedState);
                    ev.Use();
                } else if (clickedState == null && SelectedStateId != StateMachineConstants.NOTHING_SELECTED) {
                    Deselect();
                    ev.Use();
                }
            }
        }

        void SelectState(StateInEditor state) {
            Undo.RecordObject(_windowState, "Select State");
            SelectedStateId = state.GetInstanceID();
            Selection.activeObject = state;
        }

        void Deselect() {
            Undo.RecordObject(_windowState, "Deselect State");
            SelectedStateId = StateMachineConstants.NOTHING_SELECTED;
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
        #region Canvas Drag
        Vector2 _canvasPosition = new Vector2(0, 0);

        public Vector2 CanvasPosition {
            get { return _canvasPosition; }
            set {
                _canvasPosition.Set(Mathf.Clamp(value.x, -(StateMachineConstants.CANVAS_WIDTH - Screen.width), 0),
                                    Mathf.Clamp(value.y, -(StateMachineConstants.CANVAS_HEIGHT - Screen.height), 0));
            }
        }

        void HandleCanvasDrag(Event ev) {
            if (ev.type == EventType.MouseDrag && ev.button == 2) {
                CanvasPosition += ev.delta;
                Repaint();
                ev.Use();
            }
        }
        #endregion

        #region Stage Drag
        private StateInEditor _dragged;
        private Rect _dragRect;

        void HandleStateDrag(Event ev) {
            if (ev.type == EventType.MouseDrag && ev.button == 0) {
                Vector2 mousePos = ev.mousePosition;
                StateInEditor clickedState = ClickedState(mousePos);

                if (_dragged != null || clickedState != null && clickedState.GetInstanceID() == SelectedStateId) {
                    if (_dragged == null) {
                        _dragged = clickedState;
                        _dragRect = clickedState.DrawRect;
                    }

                    _dragRect.position += ev.delta;
                    Repaint();
                    ev.Use();
                }
            }

            if (ev.type == EventType.MouseUp && _dragged != null) {
                StateEditor stateEditor = StateEditor.GetEditor(_dragged);
                stateEditor.UpdatePosition(_dragRect.position);
                _dragged = null;
                ev.Use();
            }
        }
        #endregion
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
