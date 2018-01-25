using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingText : MonoBehaviour {

	public GameObject dmg;
	public string text = "";
	int count = 0;
	float scale = 0.15f;

	void Start () {
		//TODO: PlayerPrefs.DeleteAll();
		
		count = text.Length;

		for (int i = 0; i < count; i++)
		{
			GameObject dmgg1 = (GameObject) Instantiate (dmg, new Vector3 (0.3f + (i*0.37f - (0.37f * (count / 2.0f))), 1, 0), Quaternion.Euler (0, 0, 0));
			dmgg1.GetComponent<TextMesh>().text = ""+text[i];
			dmgg1.GetComponent<TextMesh>().color = Color.black;
			dmgg1.GetComponent<MeshRenderer>().sortingLayerName  = "UI";
			dmgg1.GetComponent<MeshRenderer>().sortingOrder = 4;
			
			float depth = 0.02f;
			float space = 0.05f;

			GameObject dmgg2 = (GameObject) Instantiate (dmgg1, new Vector3(-space, 0.0f, depth), Quaternion.Euler (0, 0, 0));
			GameObject dmgg3 = (GameObject) Instantiate (dmgg1, new Vector3(space, 0.0f, depth), Quaternion.Euler (0, 0, 0));
			GameObject dmgg4 = (GameObject) Instantiate (dmgg1, new Vector3(0.0f, -space, depth), Quaternion.Euler (0, 0, 0));
			GameObject dmgg5 = (GameObject) Instantiate (dmgg1, new Vector3(0.0f, space, depth), Quaternion.Euler (0, 0, 0));
			
			//dmgg2.transform.localPosition = ;
			//dmgg2.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			//dmgg2.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

			dmgg2.transform.SetParent (dmgg1.transform, false);
			dmgg3.transform.SetParent (dmgg1.transform, false);
			dmgg4.transform.SetParent (dmgg1.transform, false);
			dmgg5.transform.SetParent (dmgg1.transform, false);

			dmgg1.transform.SetParent (this.transform, false);
			dmgg1.GetComponent<TextMesh>().color = Color.white;
			dmgg1.GetComponent<MeshRenderer>().sortingOrder = 5;
		}

		StartCoroutine(Slide());
	}

	IEnumerator Slide(){
		for (int i = 0; i < count; i++)
		{
			StartCoroutine(Slide(i, 0.01f));
			//yield return new WaitForSeconds(0.35f);
			yield return 0;
		}     
    }

	IEnumerator Slide(int n, float y){      
		int i = 0;
		//while(i < 1){
			float t = 0.0f;
			while (t < 1.0f)
			{
				t += Time.deltaTime * (Time.timeScale / scale); ///
				Vector3 startingPos = transform.GetChild(n).transform.position;
				transform.GetChild(n).transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(0f, y, 0f), t);
				yield return 0;
			}

			TimeMaster.WaitSeconds(1f);
			i++;
		//}
		Destroy(this.gameObject);

	}     


}