using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Аrrow : Unit {

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		StartCoroutine ("Timer");
	}

	void Update () {
		rb.velocity = new Vector2 (input * moveSpeed, rb.velocity.y);
	}

	void OnTriggerEnter2D (Collider2D target) {
		if (target.CompareTag ("Enemy")) {
			target.GetComponent<Unit> ().SetDamage(attack, direction, attackModify);
			Destroy (gameObject);
		}
	}

	public void SetDirection (float direction) {
		flipParam = direction;
		input = direction;
	}

	public override void GetDamage () {}

	public override void SetDamage (float damage, float impulseDirection, bool[] attackModify) {}

	public override void SetStun (float direction) {}

	public override void Die () {}

	IEnumerator Timer() {
		yield return new WaitForSeconds (3f);
		Destroy (gameObject);
	}
}
