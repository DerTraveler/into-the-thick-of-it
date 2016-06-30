using UnityEngine;
using System.Collections;

/// <summary>
/// Fixes position to an always pixel perfect position.
/// </summary>
public class PixelPerfectPositioner : MonoBehaviour {

	private readonly float unitPerPixel = 1.0f / 64.0f;

	void LateUpdate () {
		// Round position to next pixel perfect position
		transform.position = RoundToPixelPerfect(transform.position);
	}

	private Vector2 RoundToPixelPerfect(Vector2 pos) {
		return new Vector2(Mathf.Round(pos.x / unitPerPixel) * unitPerPixel, Mathf.Round(pos.y / unitPerPixel) * unitPerPixel);
	}

}
