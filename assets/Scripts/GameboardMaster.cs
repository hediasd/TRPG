using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardMaster : MonoBehaviour {

	public Gameboard Gameboard;

	void Start () {
		Gameboard = new Gameboard();
	}

}
