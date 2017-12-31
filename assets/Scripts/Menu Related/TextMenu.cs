using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class TextMenu : TextBubble {

	public void Startup () {
		textMesh = transform.Find("Text").gameObject.GetComponent<TextMesh>();
		StartCoroutine(Transition());
	}

    IEnumerator Transition() {

		//textPileup =
		textMesh.text = textPileup;
		yield return 0;

    }
	
}
