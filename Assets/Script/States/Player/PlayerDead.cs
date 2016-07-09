/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace States {

	public class PlayerDead : PlayerBase {

		public override void Entry(Player subject) {
			subject.TriggerGameOver();
		}

		public override State<Player> HandleInput(Player subject) {
			if (Input.GetButtonDown("Attack")) {
				int scene = SceneManager.GetActiveScene().buildIndex; 
				SceneManager.LoadScene(scene, LoadSceneMode.Single); 
			}
			return null;
		}

		public override void Update(Player subject) { }

		public override bool ReceiveDamage(int damage) { 
			return false;
		}

	}

}

