using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingText : BaseText {


	float TimeScale = 0.02f;
	float WalkAmount = 1;

	public override void Startstuff(){
		StartCoroutine(Slide());
	}


	//IEnumerator Slide(){
		//for (int i = 0; i < TextLength; i++)
		//{
	//		StartCoroutine(Slide());
			//yield return new WaitForSeconds(0.35f);
	//		yield return 0;
		//}     
    //}

	IEnumerator Slide(){  
		
		float t = 0.0f;
		while (t < 1.0f)
		{
			t += TimeScale * TimeMaster.GeneralTiming * Time.deltaTime; ///
			for (int i = 0; i < TextLength; i++)
			{
				Vector3 startingPos = transform.GetChild(i).transform.localPosition;
				transform.GetChild(i).transform.localPosition = Vector3.Lerp(startingPos, startingPos + new Vector3(WalkAmount, 0f, 0f), t);
			}
			yield return 0;
		}

		//TimeMaster.WaitSeconds(1f);
		//}
		Destroy(this.gameObject);

	}


}