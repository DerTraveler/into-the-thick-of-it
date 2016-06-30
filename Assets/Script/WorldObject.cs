using UnityEngine;
using System.Collections;

public abstract class WorldObject : MonoBehaviour {

	public GameObject body;
	private SpriteRenderer rend;

	void Awake () {
		rend = body.GetComponent<SpriteRenderer>();
		gameObject.AddComponent<PixelPerfectPositioner>();
	}
	
	void LateUpdate () {
		if (rend.isVisible) {
			// Dynamic draw order based on y-coordinate
			rend.sortingOrder = (int) (body.transform.position.y * 64.0 * -1.0f);
		}
	}

	public Spawner spawnedBy = null;

	// Receive damage, returns true if the object was really hurt
	public abstract bool ReceiveDamage(int damage);

	protected void SendDeathNotification() {
		if (spawnedBy) {
			spawnedBy.NotifyOfDeath(this);
		}
	}

	protected void PlaySoundEffect(AudioClip clip) {
		AudioManager.instance.PlaySound(clip);
	}
}
