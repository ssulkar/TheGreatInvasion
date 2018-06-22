using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D playerRb;

	//characters
	private GameObject Tommy, Jerry, Poilu, Doughboy;
	private Hero currentHero;

	//movement
	private bool facingRight;
	private bool isGrounded = false;
	private float groundCheckRadius = 0.2f;
	public LayerMask touchable;
	public Transform groundChecker;
	public float fallHeight;

	//private Animator playerAnim;

	//shooting variables
	public Transform gunBarrel;

	//healthbar
	public Slider healthBar;
	private float healRate = 5f;
	private float healCooldown;

	//invincibility
	private float invincibilityRate = .9f;
	private float invincibilityCooldown = 0f;



	//crouch
	public Collider2D upperBody;
	private bool crouched = false;
	private float crouchRate = 1f;
	private float crouchCooldown = 0;

	// Use this for initialization
	void Start () {
		playerRb = GetComponent<Rigidbody2D> ();

		Tommy = GameObject.Find ("Tommy");
		Jerry = GameObject.Find ("Jerry");
		Poilu = GameObject.Find ("Poilu");
		Doughboy = GameObject.Find ("Doughboy");

		Tommy.SetActive (true);
		Jerry.SetActive (false);
		Poilu.SetActive (false);
		Doughboy.SetActive (false);
		facingRight = true;
		currentHero = Tommy.GetComponent<Hero>();
		healthBar.value = calculateHealth ();
		healCooldown = healRate;
	}

	// Update is called once per frame
	void Update () {
		handleManualPlayerSwap ();
		handleJump ();
		handleAutoHeal ();
	}

	void handleAutoHeal (){
		if (currentHero.currentHealth < currentHero.maxHealth && healCooldown < Time.time) {
			healCooldown = Time.time + healRate * 0.25f;
			currentHero.currentHealth += 1;
		}
		/*if (GetComponent<Rigidbody2D> ().velocity != Vector2.zero) {
			healCooldown = Time.time + healRate;
		} else {
			if (currentHero.currentHealth < currentHero.maxHealth && healCooldown < Time.time) {
				currentHero.currentHealth += 1;
				healCooldown = Time.time + healRate;
			}
		}


		if (GetComponent<Rigidbody2D> ().velocity == Vector2.zero 
			&& currentHero.currentHealth < currentHero.maxHealth
			&& healCooldown < Time.time) {
			currentHero.currentHealth += 1;
			print (currentHero.currentHealth);
			healCooldown = Time.time + healRate;
		}*/
	}

	void handleAutoPlayerSwap(){
		if (Tommy.GetComponent<Hero> ().currentHealth <= 0 
			&& Jerry.GetComponent<Hero> ().currentHealth <= 0
			&& Poilu.GetComponent<Hero> ().currentHealth <= 0
			&& Doughboy.GetComponent<Hero> ().currentHealth <= 0) {
			gameOver ();
		} else if (currentHero.currentHealth <= 0) {
			if (currentHero.name.Equals("Tommy")) {
				Tommy.SetActive (false);
				Jerry.SetActive (true);
				Poilu.SetActive (false);
				Doughboy.SetActive (false);
				currentHero = Jerry.GetComponent<Hero> ();
				handleAutoPlayerSwap ();
			} else if (currentHero.name.Equals("Jerry")) {
				Tommy.SetActive (false);
				Jerry.SetActive (false);
				Poilu.SetActive (true);
				Doughboy.SetActive (false);
				currentHero = Poilu.GetComponent<Hero> ();
				handleAutoPlayerSwap ();
			} else if (currentHero.name.Equals("Poilu")) {
				Tommy.SetActive (false);
				Jerry.SetActive (false);
				Poilu.SetActive (false);
				Doughboy.SetActive (true);
				currentHero = Doughboy.GetComponent<Hero> ();
				handleAutoPlayerSwap ();
			} else if (currentHero.name.Equals("Doughboy")) {
				Tommy.SetActive (true);
				Jerry.SetActive (false);
				Poilu.SetActive (false);
				Doughboy.SetActive (false);
				currentHero = Tommy.GetComponent<Hero> ();
				handleAutoPlayerSwap ();
			}
		}
		healthBar.value = calculateHealth (); 
	}

	void handleManualPlayerSwap(){
		if (Input.GetKeyDown (KeyCode.Alpha1) && (!currentHero.name.Equals("Tommy")) && Tommy.GetComponent<Hero>().currentHealth > 0) {
			Tommy.SetActive (true);
			Jerry.SetActive (false);
			Poilu.SetActive (false);
			Doughboy.SetActive (false);
			currentHero = Tommy.GetComponent<Hero> ();
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && (!currentHero.name.Equals("Jerry")) && Jerry.GetComponent<Hero>().currentHealth > 0) {
			Tommy.SetActive (false);
			Jerry.SetActive (true);
			Poilu.SetActive (false);
			Doughboy.SetActive (false);
			currentHero = Jerry.GetComponent<Hero> ();
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && (!currentHero.name.Equals("Poilu")) && Poilu.GetComponent<Hero>().currentHealth > 0) {
			Tommy.SetActive (false);
			Jerry.SetActive (false);
			Poilu.SetActive (true);
			Doughboy.SetActive (false);
			currentHero = Poilu.GetComponent<Hero> ();
		} else if (Input.GetKeyDown (KeyCode.Alpha4) && (!currentHero.name.Equals("Doughboy")) && Doughboy.GetComponent<Hero>().currentHealth > 0) {
			Tommy.SetActive (false);
			Jerry.SetActive (false);
			Poilu.SetActive (false);
			Doughboy.SetActive (true);
			currentHero = Doughboy.GetComponent<Hero> ();
		}
		healthBar.value = calculateHealth ();
	}

	void FixedUpdate() {
		handleSpeed ();
		handleTurn ();
		//handleJump ();
		handleFire ();
		handleFallToDeath ();
		handleReload ();
		handleSecondaryAttack ();
		handleCrouch ();

		handleAnimations ();

		//anim

	}

	void handleAnimations() {
		Animator anim = currentHero.anim;
		float speed = playerRb.velocity.magnitude;
		if(Mathf.Abs(Input.GetAxis("Horizontal")) == 0){
			speed = 0;
		}
		anim.SetFloat ("speed", speed);

		anim.SetBool ("isGrounded", isGrounded);
		anim.SetFloat ("verticalSpeed", playerRb.velocity.y);
		anim.SetBool ("crouched", crouched);

		/*
		if (isGrounded && Input.GetAxis ("Jump") > 0) {
			anim.SetBool ("isGrounded", isGrounded);
			//anim.SetBool ("isGrounded", false);
		}*/

	}

	void handleSecondaryAttack() {
		if (Input.GetButtonDown ("Fire2")) {
			currentHero.secondaryAttack.attack (gunBarrel.transform, facingRight);
			if ("Doughboy".Equals (currentHero.name)) {
				Animator anim = currentHero.anim;
			    //anim.Play ("DoughboyMelee");
				anim.Play("DoughboyMelee");

			}
		}
	}

	void handleFallToDeath(){
		if (transform.position.y <= fallHeight) {
			gameOver ();
		}
	}
	/*void handleSpeed(){
		float move = Input.GetAxis ("Horizontal");
		playerRb.velocity = new Vector2 (move * currentHero.topSpeed, playerRb.velocity.y);
		if (move > 0 && !facingRight) {
			flip ();
		} else if (move < 0 && facingRight) {
			flip ();
		}
	}*/

	void handleCrouch() {
		if(crouched == true && isGrounded == true && Input.GetButton("Fire3") == false){
			RaycastHit2D upRay = Physics2D.Raycast (upperBody.transform.position, transform.localScale.y * Vector2.up, 1f,  ~((1 << LayerMask.NameToLayer ("Player")) | (1 << LayerMask.NameToLayer ("Secondary"))));
			if (upRay.collider == null) {
				crouched = false;
				upperBody.enabled = true;
				Vector3 localPos = gunBarrel.transform.localPosition;
				gunBarrel.transform.localPosition = new Vector3 (localPos.x, localPos.y+0.4f, localPos.z);
			}
		}

		else if (crouched == false && isGrounded == true && Input.GetButton ("Fire3")) {
			if (crouchCooldown < Time.time) {
				crouchCooldown = Time.time + crouchRate;
				upperBody.enabled = false;//!upperBody.enabled;
				if (playerRb.velocity.x != 0) {
					currentHero.anim.SetBool ("slide", true);
					if (facingRight) {
						playerRb.AddForce (transform.right * 10, ForceMode2D.Impulse);
					} else {
						playerRb.AddForce (transform.right * -10, ForceMode2D.Impulse);
					}
					currentHero.anim.SetBool ("slide", false); 
				}
				crouched = true;//!crouched;
				Vector3 localPos = gunBarrel.transform.localPosition;
				gunBarrel.transform.localPosition = new Vector3 (localPos.x, localPos.y-0.4f, localPos.z);
			}
		}			
	}

	void handleSpeed(){
		float horizontalAxis = Input.GetAxis("Horizontal");
		float currentSpeed = Mathf.Abs(playerRb.velocity.x);

		float topSpeed = currentHero.topSpeed;
		if (crouched == true) {
			topSpeed = topSpeed / 2;
		}
		if (currentSpeed > topSpeed) {
			playerRb.velocity = new Vector2 ((playerRb.velocity.normalized * topSpeed).x, playerRb.velocity.y);
		} else {
			playerRb.AddForce (new Vector2 (horizontalAxis * currentHero.momentum, 0));
		}
	}

	void handleTurn(){
		float move = Input.GetAxis ("Horizontal");
		if (move > 0 && !facingRight) {
			flip ();
		} else if (move < 0 && facingRight) {
			flip ();
		}
	}

	/*void handleTurn (){
		float horizontalAxis = Input.GetAxis("Horizontal");
		float currentSpeed = Mathf.Abs(playerRb.velocity.x);
		if (currentSpeed == 0) {
			if (horizontalAxis > 0 && facingRight == false) {
				flip ();
			} else if (horizontalAxis < 0 && facingRight == true) {
				flip ();
			}
		} else if (playerRb.velocity.x < 0 && facingRight == true) {
			flip ();
		} else if (playerRb.velocity.x > 0 && facingRight == false){
			flip ();
		}
	}*/

	void handleJump(){
		if(isGrounded == true && Input.GetButtonDown("Jump")){
			isGrounded = false;
			playerRb.AddForce (new Vector2(0, currentHero.jumpHeight), ForceMode2D.Impulse);
		}
		isGrounded = Physics2D.OverlapCircle (groundChecker.position, groundCheckRadius, LayerMask.GetMask("Touchable", "Enemy"));

		/*if (Input.GetButtonDown ("Jump")) {
			//GetComponent<Rigidbody2D> ().velocity = Vector2.up * currentHero.jumpHeight;
		}*/
		if (playerRb.velocity.y < 0) {
			playerRb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.deltaTime;
		} else if (playerRb.velocity.y > 0 && !Input.GetButton ("Jump")) {
			playerRb.velocity += Vector2.up * Physics2D.gravity.y * (1.5f - 1) * Time.deltaTime;
		}
	}


	void flip(){
		facingRight = !facingRight;
		Vector3 playerScale = transform.localScale;
		playerScale.x *= -1;
		transform.localScale = playerScale;
	}

	void handleReload(){
		if(Input.GetKeyDown(KeyCode.R)){
			reload ();
		}
	}

	void reload() {
		if ((currentHero.currentClip < currentHero.maxClip) && (currentHero.currentAmmo != 0)) {
			currentHero.coolDown = Time.time + currentHero.reloadTime;
			currentHero.currentAmmo += currentHero.currentClip;
			if (currentHero.currentAmmo <= currentHero.maxClip) {
				currentHero.currentClip = currentHero.currentAmmo;
				currentHero.currentAmmo = 0;
			} else {
				currentHero.currentClip = currentHero.maxClip;
				currentHero.currentAmmo -= currentHero.maxClip;
			}
		}
	}

	void handleFire ()
	{
		if (currentHero.currentClip == 0) {
			reload ();
		} else {
			if (currentHero.name.Equals ("Jerry") && currentHero.coolDown < Time.time) {
				if (Input.GetButton ("Fire1")) {
					currentHero.coolDown = Time.time + currentHero.fireRate;
					fire (currentHero.bullet);
					currentHero.currentClip -= 1f;
				}
			} else if (Input.GetButtonDown ("Fire1") && currentHero.coolDown < Time.time) {
				if (currentHero.name.Equals ("Poilu")) {
					for (int i = 1; i < 6; i++) {
						currentHero.coolDown = Time.time + currentHero.fireRate;
						fire (currentHero.bullet);
					}
					currentHero.currentClip -= 1f;
				} else {
					currentHero.coolDown = Time.time + currentHero.fireRate;
					fire (currentHero.bullet);
					currentHero.currentClip -= 1f;
				}
			}
		}
	}

	void fire(GameObject bullet) {
		GameObject liveBullet = null;
		if (facingRight) {
			liveBullet = Instantiate (bullet, gunBarrel.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		} else if (!facingRight) {
			liveBullet = Instantiate (bullet, gunBarrel.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
		}
		if (currentHero.currentHealth <= 1) {
			liveBullet.GetComponent<DamageOnContact> ().applyCriticalMultiplier ();
		}
	}


	/*public void addDamage(float damage) {
		currentHero.currentHealth -= damage;
		if (currentHero.currentHealth <= 0) {
			currentHero.currentHealth = 0;
			handleAutoPlayerSwap ();
		}
		healthBar.value = calculateHealth ();
	}*/

	public void addDamage(float damage) {
		if(invincibilityCooldown < Time.time){
			invincibilityCooldown = Time.time + invincibilityRate;
			currentHero.currentHealth -= damage;
			if (currentHero.currentHealth <= 0) {
				currentHero.currentHealth = 0;
				handleAutoPlayerSwap ();
			}
			healthBar.value = calculateHealth ();
			healCooldown = Time.time + healRate;
		}
	}


	void gameOver(){
		SceneManager.LoadScene("MainMenu");
	}


	float calculateHealth() {
		return (currentHero.currentHealth/currentHero.maxHealth);
	}

	public void refillAmmo(){
		currentHero.currentAmmo = currentHero.maxAmmo;
	}
}
