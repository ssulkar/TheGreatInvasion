using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

	public string name;
	public float maxHealth;
	public float topSpeed;
	public float jumpHeight;
	public float momentum;
	public GameObject bullet;
	public float fireRate;

	public float maxAmmo;
	public float maxClip;
	public float reloadTime;

	[HideInInspector] public float currentHealth; 
	[HideInInspector] public float coolDown;

	[HideInInspector] public float currentAmmo;
	[HideInInspector] public float currentClip;


	public GameObject secondary;
	[HideInInspector] public SecondaryAttack secondaryAttack;

	[HideInInspector] public Animator anim;

	// Use this for initialization
	void Awake () {
		currentHealth = maxHealth;
		coolDown = 0f;
		currentAmmo = maxAmmo - maxClip;
		currentClip = maxClip;

		anim = GetComponent<Animator> ();

		if (secondary.tag == "Tripwire") {
			secondaryAttack = new TripwireAttack (secondary);
		} 
		else if (secondary.tag == "GasGrenade") {
			secondaryAttack = new GasAttack (secondary);
		}
		else if (secondary.tag == "Sword") {
			secondaryAttack = new SwordAttack (secondary);
		} 
		else if (secondary.tag == "Artillery") {
			secondaryAttack = new ArtilleryAttack (secondary);
		} 
		else {
			//secondaryAttack = new SecondaryAttack ();
			print("NO SECONDARY");
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
}
