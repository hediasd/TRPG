using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRegularBox : MonoBehaviour {

	[SerializeField]
	GameObject Box;

	void Start () {
		Box = transform.Find ("Box").gameObject;
	}

	public void Rescale (Point Size) {
		Box.transform.localScale = new Vector3 (Size.x, Size.z, 1);
	}

}