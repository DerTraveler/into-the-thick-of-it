/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;

namespace StateMachine.Editor {

    [CustomEditor(typeof(StateInEditor))]
    public class StateEditor : UnityEditor.Editor {

        #region Static Editor Getter
        static UnityEditor.Editor _editor;

        public static StateEditor GetEditor(StateInEditor state) {
            UnityEditor.Editor.CreateCachedEditor(state, typeof(StateEditor), ref _editor);
            return _editor as StateEditor;
        }
        #endregion

        public void UpdatePosition(Vector2 position) {
            var state = target as StateInEditor;

            Undo.RecordObject(state, StateMachineConstants.UndoCommands.UPDATE_STATE_POSITION);
            state.Position = position;

            EditorUtility.SetDirty(state);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

    }

}
