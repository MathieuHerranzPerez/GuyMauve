using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_zombie : Unit, IReaction<GameObject> {

	//Атакуемые слои
	public LayerMask attackCollision;
	//Область агра
	DangerArea start;

	//Ссылка на игрока
	GameObject target;

	public float runDistance = 7f;
	public float runSpeed = 5f;
	float zombySpeed;
	public float bornDelay = 0f;

	//Сила толчка во время получения урона
	public float impulsePower = 3;

	//Местоположения относительно игрока
	float targetRange = 0f;
	float targetDirection =0f;

	bool idle = true;

	GameObject hpBar;

	void Awake() {
		start = GetComponentInParent<DangerArea> ();
		start.AddEnemie (this);
	}

	void Start () {
		hpBar = transform.Find ("HPBar").gameObject;
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {

		if (!idle) {
			
			if (alive && !stunned) {

				//Определение местоположения игрока
				targetRange = Mathf.Abs (transform.position.x - target.transform.position.x);
				targetDirection = Mathf.Sign (transform.position.x - target.transform.position.x);
				flipParam = input;

				if (attackCheck) {
					if (targetRange < (attackRange - 0.3f) && ((targetDirection < 0f && direction > 0f) || (targetDirection > 0f && direction < 0f))) {
						input = 0f;
						GetDamage ();
					} else {
						if (targetRange < runDistance) {
							zombySpeed = runSpeed;
						} else
							zombySpeed = moveSpeed;

						input = -targetDirection;
					}
				}
			} else {
				Impulse ();
			}
			rb.velocity = new Vector2 (input * zombySpeed, rb.velocity.y);
			anim.SetFloat ("speed", Mathf.Abs (input * zombySpeed));
		}
	}

	//Нанести урон
	public override void GetDamage () {
		if (attackCheck) {
			attackCheck = false;
			anim.SetTrigger ("attack");
		}
	}

	//Сбросить чек атаки
	public IEnumerator ResetAttackCheck () {
		yield return new WaitForSeconds (attackSpeed);
		attackCheck = true;
	}

	//Построить луч атаки
	public void CreateAttackVector() {
		Vector2 targetVector = new Vector2 (direction, 0);
		Vector2 rayOrigin = new Vector2 (transform.position.x, transform.position.y + 0.7f);

		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, targetVector, attackRange, attackCollision);

		if (hit) {
			hit.transform.GetComponent<Unit> ().SetDamage (attack, direction, attackModify);
		}
	}

	//Получить урон
	public override void SetDamage (float damage, float impulseDirection, bool[] attackModify) {
		SetStun (impulseDirection);
		health = 0f;
		Die ();
	}

	//Получить стан
	public override void SetStun (float direction) {
		stunned = true;
		input = direction;
	}

	//Сбросить чек стана
	public void ResetStunCheck () {
		stunned = false;
		input = 0f;
	}

	//Умереть
	public override void Die () {
		Destroy (hpBar);
		anim.SetTrigger ("die");
		alive = false;
		gameObject.layer = 2;
		gameObject.tag = "Puddle";
	}

	//Начать преследование
	public void Chase (GameObject player) {
		target = player;
		StartCoroutine ("TimeToBorn");
	}

	public void StartChase() {
		gameObject.layer = 9;
		idle = false;
		hpBar.SetActive (true);
	}

	//Остановить преследование
	public void Idle () {
	}

	//Задержка перед воскрешением
	IEnumerator TimeToBorn() {
		yield return new WaitForSeconds (bornDelay);
		StartChase();
	}

	void Impulse () {
		zombySpeed = Mathf.Sqrt(Time.deltaTime) * impulsePower;
	}
}
