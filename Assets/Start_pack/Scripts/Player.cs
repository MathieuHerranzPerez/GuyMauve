using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    public GameObject character;
    public LayerMask attackCollision;
	public GameObject arrow;
    public float jumpForce;
	bool inBlock = false;
    private bool shouldJump;
    private bool canJump;


    public float rollCD = 3f;
	bool rollCheck = true;

	//Сила толчка во время получения урона
	public float defaultImpulsePower = 10f;
	float impulsePower = 10f;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	void Update () {
		
		if (attackCheck) {
			//Управление
			if (Input.GetKeyDown (KeyCode.F)) {       //Атака мечом
				GetDamage ();
			}

			if (Input.GetKeyDown (KeyCode.R)) {       //Перекат
				Roll ();
			}

			if (Input.GetKeyDown (KeyCode.B)) {       //Блок
				UseShield ();
			}

			if (Input.GetKeyDown (KeyCode.P)) {       //Атака из лука
				PullBow ();
			}
            if (canJump && Input.GetKeyDown(KeyCode.Space))
            {   
                canJump = false;
                    shouldJump = true;
            }
        }

		if (stunned || !alive) {
			Impulse ();
		} else {
			input = Input.GetAxisRaw ("Horizontal");
			flipParam = input;
		}

		rb.velocity = new Vector2 (input * moveSpeed, rb.velocity.y);
		anim.SetBool ("run", Mathf.Abs (input) > 0.1f);
	}
    private void FixedUpdate()
    {
      
        // jump
        if (shouldJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            shouldJump = false;
        }
    }
    //Атака
    public override void GetDamage () {

		if (inBlock) {
			RemoveShield ();
		}

		if (attackCheck && !stunned) {
				
			anim.SetTrigger ("attack");
			attackCheck = false;
			//Меняем скорость атаки в зависимости от заданного параметра
			anim.speed = 1 / attackSpeed;
		}
	}

	//Сбросить чек атаки
	public void ResetAttackCheck () {
		attackCheck = true;
		anim.speed = 1;
	}

	//Построить луч атаки
	public void CreateAttackVector(int attackModifier) {
		Vector2 targetVector = new Vector2 (direction, 0);
		Vector2 rayOrigin = new Vector2 (transform.position.x, transform.position.y + 0.7f);

		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, targetVector, attackRange, attackCollision);

		if (hit) {
			hit.transform.GetComponent<Unit> ().SetDamage (attack, direction, attackModify);
		}
	}

	//Получить урон
	public override void SetDamage (float damage, float impulseDirection, bool[] attackModify) {

		bool backToTheEnemy = impulseDirection == direction;

		if (inBlock) {
			if (backToTheEnemy || attackModify[1]) {
				ReduceHP (damage);
				RemoveShield ();
				SetStun (impulseDirection);
				anim.SetTrigger ("attackable");
			} else {
				SetStun (impulseDirection);
				anim.SetTrigger ("blocked");
			}
		} else if (!invulnerability) {
			ReduceHP (damage);
			SetStun (impulseDirection);
			anim.SetTrigger ("attackable");
		}
	}

	//Получить стан
	public override void SetStun (float direction) {
		input = direction;
		stunned = true;
	}

	//Сбросить чек стана
	public void ResetStunCheck () {
		input = 0f;
		moveSpeed = 5f;
		stunned = false;
		SetImpulsePower (defaultImpulsePower);
	}

	//Умереть
	public override void Die () {
		alive = false;
	}

	//Использовать блок
	void UseShield() {
		if (!invulnerability && !inBlock) {
			invulnerability = true;
			inBlock = true;
			anim.SetTrigger ("block");
		} else
			RemoveShield ();
	}

	//Завершить блок
	public void RemoveShield () {
		invulnerability = false;
		inBlock = false;
		anim.SetTrigger ("block");
	}

	//Использовать перекат
	void Roll() {

		if (inBlock) {
			RemoveShield ();
		}

		if (!stunned && rollCheck) {
			rollCheck = false;
			invulnerability = true;
			SetImpulsePower (50f);
			stunned = true;
			Physics2D.IgnoreLayerCollision (9, 8, true);
			attackCheck = false;
			anim.SetTrigger ("roll");
			input = Mathf.Sign (direction);
			StartCoroutine ("RollCD");
		}
	}

	//Завершить перекат
	public void StopRoll() {
		ResetStunCheck ();
		moveSpeed = 5f;
		Physics2D.IgnoreLayerCollision (9, 8, false);
		invulnerability = false;
	}

	//Стрельба из лука
	void PullBow () {

		if (inBlock) {
			RemoveShield ();
		}

		attackCheck = false;
		anim.SetTrigger ("pullBow");
	}

	//Выпустить стрелу
	public void CreateArrow() {
		GameObject arrowInstance = Instantiate (arrow, new Vector3 (transform.position.x, transform.position.y + 0.9f, transform.position.z), Quaternion.identity);
		Аrrow arrowScript = arrowInstance.GetComponent<Аrrow> ();
		arrowScript.SetDirection (direction);
	}

	IEnumerator RollCD () {
		yield return new WaitForSeconds (rollCD);
		rollCheck = true;
	}

	void Impulse () {
		moveSpeed = Mathf.Sqrt(Time.deltaTime) * impulsePower;
	}

	//Уменьшить ХП + проверка на "смерть"
	void ReduceHP (float damage) {
		if (health <= damage) {
			Die ();
		}
		health -= damage;
	}

	public void SetImpulsePower (float value) {
		impulsePower = value;
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        // allow jumping again
        canJump = true;
        character.transform.tag = "onFloor";
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        character.transform.tag = "Jumping";
    }
}
