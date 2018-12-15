using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

	public float health = 0f;
	public float armor = 0f;
	public float attack = 0f;
	public float attackSpeed = 0f;
	public float attackRange = 1f;

	[HideInInspector]
	public bool invulnerability = false;
	[HideInInspector]
	public bool attackCheck = true;
	[HideInInspector]
	public bool stunned = false;
	[HideInInspector]
	public bool alive = true;
	[HideInInspector]
	public bool[] attackModify = new bool[2];

	public float moveSpeed = 2f;
	[HideInInspector]
	public float input = 0f;
	[HideInInspector]
	public Rigidbody2D rb;
	[HideInInspector]
	public Animator anim;
	[HideInInspector]
	public float direction = 1f;
	[HideInInspector]
	public float flipParam = 0f;

	public abstract void GetDamage ();

	public abstract void SetDamage (float damage, float impulseDirection, bool[] attackModify);

	public abstract void SetStun (float direction);

	public abstract void Die ();
}
