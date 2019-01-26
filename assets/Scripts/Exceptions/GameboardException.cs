using System.Collections.Generic;
using UnityEngine;

public class GameboardException : System.Exception {

	public string Error;

	public GameboardException (string Error) {
		this.Error = Error;
		Debug.Log (Error);
	}

}