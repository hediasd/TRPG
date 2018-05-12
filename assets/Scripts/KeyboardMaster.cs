using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMaster : MonoBehaviour {

	string input = "";

	void Start () {
	}

	void Update () {
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			Camera.main.orthographicSize += 0.25f;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			Camera.main.orthographicSize -= 0.25f;
		}

		
	}
}
