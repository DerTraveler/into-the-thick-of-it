/* Copyright (c) 2016 Kevin Fischer
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using UnityEngine;

public abstract class State<T> {

	public virtual void Entry(T subject) { }

	public virtual void Exit(T subject) { }

	public abstract State<T> HandleInput(T subject);
	
	public abstract void Update(T subject);

}
