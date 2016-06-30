using UnityEngine;
using System.Collections;

/// <summary>
/// Fixes position so it always keeps inside the given boundaries.
/// </summary>
public class KeepInBounds : MonoBehaviour {

	public Rect positionBoundary = new Rect(-4.75f, -4.75f, 9.5f, 9.3f);

	void LateUpdate () {
		transform.position = ReturnInsideBounds(positionBoundary, transform.position);
	}

	private Vector3 ReturnInsideBounds(Rect rect, Vector3 point) {
		return new Vector3(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax), point.z);
	}

}
