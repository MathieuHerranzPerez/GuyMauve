using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour {

	Unit controller;

	//переменная для определения направления персонажа вправо/влево
	[HideInInspector]
	public bool isFacingRight = true;

	void Start () {
		controller = GetComponent<Unit> ();
	}
	
	void Update () {
		
		if(controller.flipParam > 0 && !isFacingRight)
			//отражаем персонажа вправо
			FlipObject();
		//обратная ситуация. отражаем персонажа влево
		else if (controller.flipParam < 0 && isFacingRight)
			FlipObject();
	}

	private void FlipObject()
	{
		//меняем направление движения персонажа
		isFacingRight = !isFacingRight;
		//получаем размеры персонажа
		Vector3 theScale = transform.localScale;
		//зеркально отражаем персонажа по оси Х
		theScale.x *= -1;
		controller.direction *= -1;
		//задаем новый размер персонажа, равный старому, но зеркально отраженный
		transform.localScale = theScale;
	}

}
