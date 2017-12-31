using UnityEngine;
using System.Collections;

public class MoveBack : MonoBehaviour {

	public Vector3 mov;

	void Start () {
		StartCoroutine(Mover());
	}
	
	IEnumerator Mover () {
		Vector3 sp = transform.localPosition;
		Vector3 st = sp + new Vector3(mov.x/7, 0, mov.z/7);
        float t = 0.0f;
		while(true){         
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / 0.3f); ///
                transform.localPosition = Vector3.Lerp(sp, st, t);
                yield return 0;
            }
			t = 0;
			while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / 0.6f); ///
                transform.localPosition = Vector3.Lerp(st, sp, t);
                yield return 0;
            }
			t = 0;			
        }
	}
}
