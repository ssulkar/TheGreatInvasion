using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : SecondaryAttack {

	GameObject sword;
	private float attackRate = 1f;
	private float coolDown;
	// Use this for initialization

	public SwordAttack(GameObject prefab) {
		sword = prefab;
	}

	void Start () {
		coolDown = 0;
	}

	// Update is called once per frame
	void Update () {

	}

	public override void attack(Transform t, bool facingRight) {
		if (coolDown < Time.time) {
			GameObject swing = null;
			coolDown = Time.time + attackRate;
			if (facingRight) {
				swing = Instantiate (sword, t.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else if (!facingRight) {
				swing = Instantiate (sword, t.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
			swing.transform.SetParent (t);
		}
	}
}
