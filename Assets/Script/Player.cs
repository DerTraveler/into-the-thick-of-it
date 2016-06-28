using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 1.0f;

	private Animator animator;
	private SpriteRenderer spriteRenderer;

	public enum State {
		Idle,
		Walking,
		Attacking
	}

	[SerializeField]
	private State state = State.Idle;
	[SerializeField]
	private Vector2 moveDirection = new Vector2(0, 0);
	[SerializeField]
	private Vector2 faceDirection = new Vector2(0, -1);
	[SerializeField]
	private bool directionPressed = false;

	void Start () {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ReadInput();

		UpdateState();

		PlayAnimation();

		UpdateTransform();
	}

	private void ReadInput() {
		moveDirection.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		directionPressed = moveDirection.sqrMagnitude > float.Epsilon;

		if (Mathf.Approximately(moveDirection.sqrMagnitude, 1.0f)) {
			faceDirection.Set(moveDirection.x, moveDirection.y);
		}
	}

	private void UpdateState() {
		switch(state) {
			case State.Idle:
				if (directionPressed) {
					state = State.Walking;
				}
				break;
			case State.Walking:
				if (!directionPressed) {
					state = State.Idle;
				}
				break;
		}
	}

	private void PlayAnimation() {
		// Flipping on X axis
		spriteRenderer.flipX = Mathf.Approximately(faceDirection.x, -1.0f);

		switch (state) {
			case State.Idle:
				animator.Play("Idle" + getDirectionName());
				break;
			case State.Walking:
				animator.Play("Walk" + getDirectionName());
				break;
		}
	}

	private string getDirectionName() {
		if (faceDirection.y > 0) {
			return "Up";
		} else if (faceDirection.y < 0) {
			return "Down";
		} else {
			return "Side";
		}
	}

	private void UpdateTransform() {
		if (state == State.Walking) {
			Vector3 newPos = transform.position + (Vector3)(moveDirection * speed * Time.deltaTime);
			transform.position = newPos;
		}
	}
}
