using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public int x, y, z;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(x, y, z) * 2 * Time.deltaTime);
	}
}
