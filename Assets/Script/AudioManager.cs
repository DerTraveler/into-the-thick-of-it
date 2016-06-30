using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;

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
		sfxSource.clip = clip;
		sfxSource.Play();
	}

}
