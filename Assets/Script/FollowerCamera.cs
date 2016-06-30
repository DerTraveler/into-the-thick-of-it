/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;
using System.Collections;

public class FollowerCamera : MonoBehaviour {

	public Transform target;

	public Vector2 viewportOffset = new Vector2(0.5f, 0.45f);
	public Vector2 viewportSize = new Vector2(0.6f, 0.5f);

	private Rect viewportRect;

	private Camera cam;

	void Awake () {
		cam = GetComponent<Camera>();
		viewportRect = new Rect(viewportOffset.x - viewportSize.x / 2.0f, viewportOffset.y - viewportSize.y / 2.0f,
								viewportSize.x, viewportSize.y);

		gameObject.AddComponent<PixelPerfectPositioner>();
	}

	void LateUpdate () {
		Vector2 positionInViewport = cam.WorldToViewportPoint(target.position);

		if (!viewportRect.Contains(positionInViewport)) {
			ViewWorldPointAtViewportPoint(target.position, ClosestPointOnBorder(viewportRect, positionInViewport));
		}
	}

	private void ViewWorldPointAtViewportPoint(Vector3 worldPoint, Vector2 viewportPoint) {
		Vector3 viewportPointInWorld = cam.ViewportToWorldPoint(viewportPoint);
		Vector3 offsetToCamera = transform.position - viewportPointInWorld;
		Vector3 result = worldPoint + offsetToCamera;
		result.z = transform.position.z;

		transform.position = result;
	}

	private Vector2 ClosestPointOnBorder(Rect rect, Vector2 point) {
		return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
	}

}
