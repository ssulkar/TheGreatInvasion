using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMovement : MonoBehaviour {

	public Transform[] positions;
	public Transform sightPosition;
	public Transform rearSightPosition;
	public float speed;
	public float idleTime;
	public float sight;
	public float chaseSpeed;
	public float chaseCoolDown;

	private Rigidbody2D rb;
	private int currentPosition;


	private bool hitPlayer = false;
	private bool foundPlayer = false;


	//coroutine checks
	private bool patrolling = true;
	private bool fighting = false;
	private bool shooting = false;

	private float shiftRate = 3f;
	private float shiftCooldown = 0;


	//melee
	public GameObject meleeAttack;
	private float meleeRate = 1.5f;
	private float meleeCooldown;
	//shooting
	public GameObject shootAttack;
	private float shootRate = 2f;


	//animations
	Animator anim;


	//private bool idle = false;

	//patrol
	private bool scriptedPatrol;


	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		currentPosition = 0;
		anim = GetComponent<Animator> ();

		if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
			scriptedPatrol = true;
		} else {
			scriptedPatrol = false;
		}
		StartCoroutine ("Patrol");
	}

	void Update(){

		//transform.parent.position = transform.position - transform.localPosition;
		RaycastHit2D front = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, sight,  ~(1 << LayerMask.NameToLayer("Enemy")));
		RaycastHit2D back = Physics2D.Raycast (rearSightPosition.position, transform.localScale.x * Vector2.left, sight,  ~(1 << LayerMask.NameToLayer("Enemy")));

		if (patrolling) {
			if (front.collider != null && front.collider.CompareTag ("Player")) {
				patrolling = false;
				StopCoroutine ("Patrol");
			}
			shiftCooldown = Time.time + shiftRate;
		}

		else if(!patrolling){
			if (front.collider != null && front.collider.CompareTag ("Player")) {
				if (front.distance > 0.25f) {
					if (fighting == true){
						fighting = false;
						StopCoroutine ("Fight");
					}
					if (shooting == false) {
						shooting = true;
						StartCoroutine ("Shoot");
					}
					//transform.position = Vector2.MoveTowards (transform.position, new Vector2 (front.collider.transform.position.x, transform.position.y), chaseSpeed);
				} else if(front.distance < 0.25f) {
					if (fighting != true) {
						fighting = true;
						StartCoroutine ("Fight");
					}
				}
				shiftCooldown = Time.time + shiftRate;
			}
			else if (back.collider != null && back.collider.CompareTag ("Player")) {
				if (fighting == true){
					fighting = false;
					StopCoroutine ("Fight");
				}
				transform.localScale = new Vector3 (transform.localScale.x * -1,1,1);
				shiftCooldown = Time.time + shiftRate;
			}
			else {
				if (shiftCooldown < Time.time) {
					shiftCooldown = Time.time + shiftRate;
					patrolling = true;
					if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
						scriptedPatrol = true;
					} else {
						scriptedPatrol = false;
					}
					StartCoroutine ("Patrol");
				} else {
					handleAnimations ("idle");
				}
			}
		}
	}

	void handleAnimations(string name){
		if ("fighting".Equals (name)) {
			anim.SetBool ("shooting", false);
			anim.SetBool("patrolling", false);
			anim.SetBool ("idle", false);
			anim.SetBool ("fighting", true);
		}
		else if ("idle".Equals (name)) {
			anim.SetBool ("shooting", false);
			anim.SetBool("patrolling", false);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", true);
		}
		else if ("patrolling".Equals (name)) {
			anim.SetBool ("shooting", false);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", false);
			anim.SetBool("patrolling", true);
		} else if ("shooting".Equals (name)) {
			anim.SetBool("patrolling", false);
			anim.SetBool ("fighting", false);
			anim.SetBool ("idle", false);
			anim.SetBool ("shooting", true);
		}
	}

	IEnumerator Fight() {
		fighting = true;
		fighting = true;
		if (shooting == true) {
			shooting = false;
			StopCoroutine ("Shoot");
		}

		while(true) {
			anim.Play ("ShooterIdle");
			yield return new WaitForSeconds(.25f);
			anim.Play ("ShooterAttack");
			GameObject swing = null;


			if (transform.localScale.x > 0) {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
			swing.transform.SetParent (sightPosition);
			//yield return new WaitForSeconds(Random.Range(meleeRate, meleeRate+0.5f));


			yield return new WaitForSeconds(Random.Range(meleeRate, meleeRate+0.5f));
			handleAnimations ("fighting");
		}
	}
	/*IEnumerator Fight() {
		fighting = true;
		if (shooting == true) {
			shooting = false;
			StopCoroutine ("Shoot");
		}

		while(true) {
			//handleAnimations ("idle");
			yield return new WaitForSeconds (meleeRate);
			handleAnimations ("fighting");
		
			GameObject swing = null;
			if (transform.localScale.x > 0) {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else {
				swing = Instantiate (meleeAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
			swing.transform.SetParent (sightPosition);
			//yield return null;
		}
	}*/

	IEnumerator Shoot() {
		shooting = true;
		if (fighting == true) {
			fighting = false;
			StopCoroutine ("Fight");
		}

		while(true) {
			anim.Play ("ShooterIdle");
			yield return new WaitForSeconds (shootRate/2);
			//handleAnimations ("fighting");
			anim.Play ("ShooterAttack");
			GameObject bullet = null;
			if (transform.localScale.x > 0) {
				bullet = Instantiate (shootAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else {
				bullet = Instantiate (shootAttack, sightPosition.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}

			yield return new WaitForSeconds (shootRate/2);
			//bullet.transform.SetParent (sightPosition);
			//yield return null;
		}
	}


	IEnumerator Patrol() {
		//int currentPosition = 0;
		patrolling = true;

		if (fighting == true) {
			fighting = false;
			StopCoroutine ("Fight");
		}
		if (shooting == true) {
			shooting = false;
			StopCoroutine ("Shoot");
		}

		handleAnimations ("patrolling");

		while (true) {
			if (transform.position.x <= positions [0].position.x && transform.position.x >= positions [1].position.x) {
				scriptedPatrol = true;
			} else {
				scriptedPatrol = false;
			}
			if (scriptedPatrol) {
				if (currentPosition >= positions.Length) {
					currentPosition = 0;
				}
				if (transform.position.x == positions [currentPosition].position.x) {
					currentPosition++;
					handleAnimations ("idle");
					yield return new WaitForSeconds (idleTime);
					handleAnimations ("patrolling");
				}
				if (currentPosition >= positions.Length) {
					currentPosition = 0;
				}

				transform.position = Vector2.MoveTowards (transform.position, new Vector2 (positions [currentPosition].position.x, transform.position.y), speed);

				if (transform.position.x > positions [currentPosition].position.x) {
					transform.localScale = new Vector3 (-1, 1, 1);
				} else if (transform.position.x < positions [currentPosition].position.x) {
					transform.localScale = new Vector3 (1, 1, 1);
				}
				yield return null;
			} else {
				RaycastHit2D front = Physics2D.Raycast (sightPosition.position, transform.localScale.x * Vector2.right, .5f, ~(1 << LayerMask.NameToLayer("Enemy")));
				RaycastHit2D frontDown = Physics2D.Raycast (sightPosition.position, new Vector2 (0, -1), 1.5f, ~(1 << LayerMask.NameToLayer("Enemy")));

				if (front.collider == null && frontDown.collider != null && frontDown.collider.IsTouchingLayers (LayerMask.GetMask ("Touchable"))) {
					if (transform.localScale.x > 0) {
						transform.position = Vector2.MoveTowards (transform.position, new Vector2 (transform.position.x + 20, transform.position.y), speed);
					} else {
						transform.position = Vector2.MoveTowards (transform.position, new Vector2 (transform.position.x - 20, transform.position.y), speed);
					}
				} else {
					handleAnimations ("idle");
					yield return new WaitForSeconds (idleTime);
					handleAnimations ("patrolling");
					transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
				}
				yield return null;
			}
		}
		yield return null;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, transform.position + transform.localScale.x * Vector3.right * sight);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (patrolling && (other.gameObject.layer == LayerMask.NameToLayer("Bullet") 
			|| other.gameObject.layer == LayerMask.NameToLayer("Secondary") || other.CompareTag("Player"))) {
			shiftCooldown = Time.time + shiftRate;
			patrolling = false;
			StopCoroutine ("Patrol");

			Vector3 dir = other.transform.position - transform.position;
			if ((transform.localScale.x > 0 && dir.x < 0) || (transform.localScale.x < 0 && dir.x > 0)) {
				transform.localScale = new Vector3 (transform.localScale.x * -1,1,1);
			}
		}
	}
	/*
	IEnumerator OnTriggerEnter2D(Collider2D other){

		if (other.CompareTag ("Player")) {
			hitPlayer = true;
			yield return new WaitForSeconds (chaseCoolDown);
			hitPlayer = false;
		}

		if (other.CompareTag("Bullet") && !foundPlayer) {

			if (patrolling == true) {
				patrolling = false;
				StopCoroutine ("Patrol");
				Vector3 dir = other.transform.position - transform.position;
				if (dir.x > 0) {
					currentPosition = 0;
				} else if (dir.x < 0) {
					currentPosition = 1;
				}
			}

		}
	}
	*/
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.gameObject.tag == "Enemy") {
			Collider2D[] colliders = GetComponents<Collider2D> ();
			Collider2D[] otherColliders = collision.gameObject.GetComponents<Collider2D> ();
			foreach (Collider2D c1 in colliders) {
				foreach (Collider2D c2 in otherColliders) {
					Physics2D.IgnoreCollision (c1, c2);
				}
			}
		}
	}


	void switchPosition(){
		if (currentPosition == 0) {
			currentPosition = 1;
		} else {
			currentPosition = 0;
		}
	}



}
