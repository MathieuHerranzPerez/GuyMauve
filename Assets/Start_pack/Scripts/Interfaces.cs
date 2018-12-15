using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReaction <T> {
	void Chase(T player);
	void Idle ();
}

public delegate void alert (GameObject player);

public delegate void idle ();


