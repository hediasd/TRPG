using UnityEngine;
using System.Collections;

public class BaseText : MonoBehaviour {

	public GameObject dmg;
	public string Text = "";
	protected int TextLength = 0;

	public void Startup (string Texts) {
		//TODO: PlayerPrefs.DeleteAll();
		Text = Texts;
		TextLength = Text.Length;

		for (int i = 0; i < TextLength; i++)
		{
			GameObject dmgg1 = (GameObject) Instantiate (dmg, new Vector3 (0.3f + (i*0.37f - (0.37f * (TextLength / 2.0f))), 1, 0), Quaternion.Euler (0, 0, 0));
			dmgg1.GetComponent<TextMesh>().text = ""+Text[i];
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

        Startstuff();

	}

    public virtual void Startstuff(){

    }

}





