using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.LookAt(Camera.main.transform.position );// - new Vector3(0,1.5f,0));
		//transform.Rotate(0,180,0);
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 v = new Vector3(-Camera.main.transform.position.x, Camera.main.transform.position.y, -Camera.main.transform.position.z);
		
	}
}
