using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator animator;
	private SpriteRenderer spriteRenderer;

	public enum State {
		Idle,
		Walking,
		Attacking
	}

	public State state = State.Idle;

	public int direction = -1;
	public bool directionPressed = false;

	void Start () {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ReadInput();

		UpdateState();
	}

	private void ReadInput() {
		if (Input.GetAxisRaw("Horizontal") != 0) {
			direction = (int) Input.GetAxisRaw("Horizontal") * 2;
			directionPressed = true;
		} else if (Input.GetAxisRaw("Vertical") != 0) {
			direction = (int) Input.GetAxisRaw("Vertical");
			directionPressed = true;
		} else {
			directionPressed = false;
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

		PlayAnimation();
	}

	private void PlayAnimation() {
		spriteRenderer.flipX = direction == -2;
		switch (state) {
			case State.Idle:
				animator.Play("Idle" + getDirectionName(direction));
				break;
			case State.Walking:
				animator.Play("Walk" + getDirectionName(direction));
				break;
		}
	}

	private string getDirectionName(int direction) {
		switch (direction) {
		case 1:
			return "Up";
		case -1:
			return "Down";
		default:
			return "Side";
		}
	}
}
