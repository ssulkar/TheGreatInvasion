using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlimpMovement : MonoBehaviour {


	public GameObject balloon;
	//public GameObject basket;

	public float thrust;

	private Rigidbody2D balloonRb;
	private Rigidbody2D basketRb;

	public float xSpeed, ySpeed;

	private bool hasDriver = false;

	// Use this for initialization
	void Start () {
		balloonRb = balloon.GetComponent<Rigidbody2D> ();
		basketRb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		handleSpeed ();
		if (hasDriver) {
			handleMovement ();
		}
	}

	void handleMovement(){
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis = Input.GetAxis("Vertical");

		if (verticalAxis > 0) {
			//balloon.GetComponent<Rigidbody2D> ().gravityScale = -3.5f;
			balloon.GetComponent<Rigidbody2D>().AddForce (new Vector2(9, thrust), ForceMode2D.Force);
		}

		if (horizontalAxis < 0) {
			//balloon.GetComponent<Rigidbody2D> ().gravityScale = -3.5f;
			//balloon.GetComponent<Rigidbody2D>().AddForce (basket.transform.right*-1*thrust*.5f, ForceMode2D.Force);
			balloon.GetComponent<Rigidbody2D>().AddForce (new Vector2(-1*thrust*.75f, thrust*.25f*0), ForceMode2D.Force);
		}

		if (horizontalAxis > 0) {
			//balloon.GetComponent<Rigidbody2D> ().gravityScale = -3.5f;
			//balloon.GetComponent<Rigidbody2D>().AddForce (basket.transform.right*thrust * .5f, ForceMode2D.Force);

			balloon.GetComponent<Rigidbody2D>().AddForce (new Vector2(1*thrust*.75f, thrust*.25f*0), ForceMode2D.Force);
		}


		/*else if(Input.GetButtonUp("Interact"))
		{
			balloon.GetComponent<Rigidbody2D> ().gravityScale = -1f;
		}*/
		//balloon.GetComponent<Rigidbody2D> ().constraints;
	}


	void handleRotation(){
	}
	void handleSpeed(){
		
		float currentXSpeed = Mathf.Abs (basketRb.velocity.x);
		float currentYSpeed = Mathf.Abs (basketRb.velocity.y);

		if (currentXSpeed > xSpeed) {
			basketRb.velocity = new Vector2((basketRb.velocity.normalized * xSpeed).x, basketRb.velocity.y);
		}
		if (currentYSpeed > ySpeed) {
			basketRb.velocity = new Vector2(basketRb.velocity.x, (basketRb.velocity.normalized * xSpeed).y);
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if(other.gameObject.tag == "Player"){
			other.transform.parent = transform;


			hasDriver = true;
			//PlayerController playerController = other.gameObject.GetComponent<PlayerController> ();

			/*
			PlayerController pc = other.gameObject.GetComponent<PlayerController> ();


			float heroTopSpeed = pc.getCurrentHero ().topSpeed;//    .getCurrentHero ().topSpeed;
			pc.onMovingPlatform = true;
			pc.currentTopSpeed = Mathf.Abs (basketRb.velocity.x) + heroTopSpeed;
			print (pc.currentTopSpeed);*/
		}

	}
	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			other.transform.parent = null;
			hasDriver = false;
			/*
			PlayerController pc = other.gameObject.GetComponent<PlayerController> ();
								float heroTopSpeed = pc.getCurrentHero ().topSpeed;
									pc.onMovingPlatform = false;
										pc.currentTopSpeed = heroTopSpeed;*/
		}
	}
}
