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
