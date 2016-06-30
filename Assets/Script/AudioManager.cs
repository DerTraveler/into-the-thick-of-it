/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource[] sfxSources;

	public static AudioManager instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(AudioClip clip) {
		AudioSource freeSource = null;

		foreach (AudioSource src in sfxSources) {
			if (!src.isPlaying) {
				freeSource = src;
			}
			if (src.clip == clip && src.isPlaying) {
				return;
			}
		}
		if (freeSource != null) {
			freeSource.clip = clip;
			freeSource.Play();	
		}
	}

}
