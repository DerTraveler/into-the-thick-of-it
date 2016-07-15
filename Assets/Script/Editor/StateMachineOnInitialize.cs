/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace StateMachine.Editor {

	[InitializeOnLoad]
	public static class StateMachineOnInitialize {

		static StateMachineOnInitialize() {
			EditorApplication.playmodeStateChanged += StateMachineEditorWindow.PlaymodePersistence;
		}
	}

}
