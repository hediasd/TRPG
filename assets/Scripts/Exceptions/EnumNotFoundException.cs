using UnityEngine;

public class EnumNotFoundException : System.Exception {

	public string Error;

	public EnumNotFoundException (string Error) {
		this.Error = Error;
		Debug.Log (Error);
	}

}