using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeGrow : MonoBehaviour {

	int max = 100;
	int i = 0;

	void Update () {
		if(i < max){
			i += 4;
			this.transform.localScale = new Vector3(i, max, 1);
		}
	}
}

