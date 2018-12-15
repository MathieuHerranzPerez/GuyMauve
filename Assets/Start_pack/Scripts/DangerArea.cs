using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour {

	alert enemieAlert;
	idle enemieIdle;
	List<IReaction<GameObject>> enemies = new List<IReaction<GameObject>>();
	bool check = true;
	[HideInInspector]
	public int deadEnemies = 0;
	int allEnemies = 0;

	void Start () {
		foreach (IReaction<GameObject> enemy in enemies) {
			enemieAlert += enemy.Chase;
			allEnemies += 1;
		}

		foreach (IReaction<GameObject> enemy in enemies) {
			enemieIdle += enemy.Idle;
		}
	}

	public void AddEnemie(IReaction<GameObject> newEnemie) {
		enemies.Add (newEnemie);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			if (check) {
				enemieAlert (other.gameObject);
				check = false;
			}
		}
	}
}
