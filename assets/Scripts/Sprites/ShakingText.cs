using System.Collections;
using UnityEngine;

public class ShakingText : BaseText {

	float TimeScale = 2f;

	public override void Startstuff () {
		PiecesMaster.TextDamagesActing++;
		StartCoroutine (Shake ());
	}

	IEnumerator Shake () {
		for (int i = 0; i < TextLength; i++) {
			StartCoroutine (Shake (i, 0.01f));
			//yield return new WaitForSeconds(0.35f);
			yield return 0;
		}
	}

	IEnumerator Shake (int n, float y) {
		
		float t = 0.0f;
		while (t < 1.0f) {
			t += TimeScale * TimeMaster.GeneralTiming * Time.deltaTime; // * (Time.timeScale / scale); ///
			Vector3 startingPos = transform.GetChild (n).transform.position;
			transform.GetChild (n).transform.position = Vector3.Lerp (startingPos, startingPos + new Vector3 (0f, y, 0f), t);
			yield return 0;
		}
		t = 0.0f;
		while (t < 1.0f) {
			t += TimeScale * TimeMaster.GeneralTiming * Time.deltaTime; // * (Time.timeScale / scale); ///
			Vector3 startingPos = transform.GetChild (n).transform.position;
			transform.GetChild (n).transform.position = Vector3.Lerp (startingPos, startingPos + new Vector3 (0f, -y, 0f), t);
			yield return 0;
		}

		yield return TimeMaster.WaitSeconds (0.1f);

		PiecesMaster.TextDamagesActing--;
		Destroy (this.gameObject);

	}

}