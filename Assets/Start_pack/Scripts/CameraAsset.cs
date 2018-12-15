using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAsset : MonoBehaviour {

	public GameObject target;
	Vector3 targetVector;

	void Start () {
	}
	
	void Update () {
		FindTarget (target);
	}

	void FindTarget(GameObject target) {
		targetVector = target.transform.position;
		targetVector.z -= 10f;
		targetVector.y += 3f;
		transform.position = targetVector;
	}
}
