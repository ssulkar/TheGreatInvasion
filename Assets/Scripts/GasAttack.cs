using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasAttack : SecondaryAttack {

	GameObject gasGrenade;
	private float attackRate = 18f;
	private float coolDown;
	// Use this for initialization

	public GasAttack(GameObject prefab) {
		gasGrenade = prefab;
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
			Vector3 v = t.position;
			if (facingRight) {
				Vector3 rayOrigin = new Vector3 (t.position.x-0.5f, t.position.y, t.position.z);
				RaycastHit2D front = Physics2D.Raycast (rayOrigin, t.localScale.x * Vector2.right, 1f, ~((1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Player"))));
				if (!(front.collider != null && front.collider.CompareTag ("Touchable"))) {
					v.x += .5f;
				}
				Instantiate (gasGrenade, v, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else if (!facingRight) {
				Vector3 rayOrigin = new Vector3 (t.position.x+0.5f, t.position.y, t.position.z);
				RaycastHit2D front = Physics2D.Raycast (rayOrigin, t.localScale.x * Vector2.left, 1f, ~((1 << LayerMask.NameToLayer ("Enemy")) | (1 << LayerMask.NameToLayer ("Player"))));
				if (!(front.collider != null && front.collider.CompareTag ("Touchable"))) {
					v.x -= .5f;
				}
				Instantiate (gasGrenade, v, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
		}
	}
}
