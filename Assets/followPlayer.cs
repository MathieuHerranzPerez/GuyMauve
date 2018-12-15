using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    private Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.position - player1.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player1.transform.position + offset;

    }
}
