/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

/// <summary>
/// Fixes position so it always keeps inside the given boundaries.
/// </summary>
public class KeepInBounds : MonoBehaviour {

    public Rect positionBoundary = new Rect(-4.75f, -4.75f, 9.5f, 9.3f);

    void LateUpdate() {
        transform.position = ReturnInsideBounds(positionBoundary, transform.position);
    }

    static Vector3 ReturnInsideBounds(Rect rect, Vector3 point) {
        return new Vector3(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax), point.z);
    }

}
