using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour {

	public Player player;

	public float maxStamina = 100.0f;
	public float staminaRegeneration = 40.0f;

	public float distancePerJump = 0.15f;
	public float staminaPerJump = 10.0f;


	private Animator animator;

	public enum State {
		Idle,
		Following,
		Hurt
	}

	[SerializeField]
	private State state = State.Idle;
	[SerializeField]
	private string currentAnimation = "Idle";
	[SerializeField]
	private float currentStamina;

	[SerializeField]
	private bool jumpTrigger = false;
	[SerializeField]
	private bool jumping = false;
	[SerializeField]
	private Vector2 jumpOrigin;
	[SerializeField]
	private Vector2 jumpDirection = new Vector2(1, 0);

	[SerializeField]
	private bool lookToLeft = false;

	float distanceFromPlayer {
		get { return (player.transform.position - transform.position).magnitude; }
	}

	Vector2 playerDirection {
		get { return (player.transform.position - transform.position); }
	}

	float animationTime {
		get { return animator.GetCurrentAnimatorStateInfo(0).normalizedTime; }
	}


	void Start () {
		animator = GetComponent<Animator>();
		currentStamina = maxStamina;
	}

	// Update is called once per frame
	void FixedUpdate () {
		UpdateState();

		PlayAnimation();

		UpdateTransform();
	}

	private void UpdateState() {
		switch(state) {
		case State.Idle:
			currentStamina = Mathf.Min(currentStamina + staminaRegeneration * Time.deltaTime, maxStamina);
			if (Mathf.Approximately(currentStamina, maxStamina)) {
				state = State.Following;
			}
			break;
		case State.Following:
			if (jumping && animationTime > 1.0f) {
				jumping = false;
			} else if (jumpTrigger) {
				jumpTrigger = false;
				jumping = true;
			} else if (!jumping) {
				if (currentStamina >= staminaPerJump) {
					Jump(playerDirection);
				} else {
					state = State.Idle;
				}
			}
			break;
		}

	}

	private void Jump(Vector2 direction) {
		jumpTrigger = true;

		jumpOrigin = transform.position;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
			jumpDirection.Set(Mathf.Sign(direction.x) * 1.0f, 0f);
			lookToLeft = jumpDirection.x < 0;
		} else {
			jumpDirection.Set(0f, Mathf.Sign(direction.y) * 1.0f);
		}

		currentStamina -= (staminaPerJump + Random.Range(-3, 3));
	}

	private void PlayAnimation() {
		FlipIfNecessary();

		switch(state) {
		case State.Idle:
			animator.Play("Idle");
			break;
		case State.Hurt:
			animator.Play("Hurt");
			break;
		default:
			if (jumpTrigger) {
				animator.Play("Jump", 0, 0);
			}
			break;
		}
	}

	private void FlipIfNecessary() {
		Vector3 scale = transform.localScale;
		float scaleX = Mathf.Abs(scale.x);
		scale.Set(lookToLeft ? -scaleX: scaleX, scale.y, scale.z);
		transform.localScale = scale;
	}

	private void UpdateTransform() {
		if (jumping) {
			Vector2 newPos = jumpOrigin + Mathf.Lerp(0, distancePerJump, animationTime) * jumpDirection;
			transform.position = newPos;
		}
	}

	public void DealDamage() {
		jumpTrigger = false;
		jumping = false;
		state = State.Hurt;
	}
}
