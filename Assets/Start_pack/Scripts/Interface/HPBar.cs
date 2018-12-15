using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

	public Unit unit;
	Image image;
	float maxHP;
	Transform unitTransform;
	Vector3 scale;

	void Start () {
		image = GetComponent<Image> ();
		maxHP = unit.health;
		unitTransform = unit.transform.GetComponent<Transform> ();
		scale = transform.localScale;
	}
	
	void Update () {

		if (Mathf.Sign(scale.x) != Mathf.Sign(unitTransform.localScale.x)) {
			scale.x *= -1f;
			transform.localScale = scale;
		}

		image.fillAmount = unit.health / maxHP;
	}
}
