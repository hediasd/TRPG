using UnityEngine;

public class DataEntryException : System.Exception {

	public string Error;

	public DataEntryException (string Error) {
		this.Error = Error;
		Debug.Log (Error);
	}

}