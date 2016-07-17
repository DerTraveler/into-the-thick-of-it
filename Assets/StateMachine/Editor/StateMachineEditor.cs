/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;

namespace StateMachine.Editor {

    [CustomEditor(typeof(StateMachineInEditor))]
    public class StateMachineEditor : UnityEditor.Editor {

        #region Static Editor Getter
        static UnityEditor.Editor _editor;

        public static StateMachineEditor GetEditor(StateMachineInEditor stateMachine) {
            UnityEditor.Editor.CreateCachedEditor(stateMachine, typeof(StateMachineEditor), ref _editor);
            return _editor as StateMachineEditor;
        }
        #endregion

        public StateInEditor AddState(Vector2 position) {
            var stateMachine = target as StateMachineInEditor;

            Undo.RecordObject(stateMachine, StateMachineConstants.UndoCommands.CREATE_STATE);
            StateInEditor newState = stateMachine.AddState(position);
            newState.hideFlags = HideFlags.HideInHierarchy;
            Undo.RegisterCreatedObjectUndo(newState, StateMachineConstants.UndoCommands.CREATE_STATE);

            AssetDatabase.AddObjectToAsset(newState, stateMachine);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(stateMachine));

            EditorUtility.SetDirty(stateMachine);

            return newState;
        }

        public void RemoveState(StateInEditor state) {
            var stateMachine = target as StateMachineInEditor;

            Undo.RecordObject(stateMachine, StateMachineConstants.UndoCommands.DELETE_STATE);
            stateMachine.RemoveState(state);

            Undo.DestroyObjectImmediate(state);

            EditorUtility.SetDirty(stateMachine);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

    }

}
