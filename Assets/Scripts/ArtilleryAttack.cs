using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryAttack : SecondaryAttack {

	GameObject artillery;
	private float attackRate = 10f;
	private float coolDown;
	// Use this for initialization

	public ArtilleryAttack(GameObject prefab) {
		artillery = prefab;
	}

	void Start () {
		coolDown = 0;
	}

	// Update is called once per frame
	void Update () {

	}

	public override void attack(Transform t, bool facingRight) {
		if (coolDown < Time.time) {
			coolDown = Time.time + attackRate;
			GameObject ami = Instantiate (Resources.Load("Pigeon"), t.position, Quaternion.identity) as GameObject;
			GameObject artyShell = Instantiate (artillery, t.position, Quaternion.identity);
			artyShell.transform.SetParent (t);
		}
	}
}
