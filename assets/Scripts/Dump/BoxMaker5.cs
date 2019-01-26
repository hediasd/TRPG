using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxMaker5 : MonoBehaviour {
	//public Text letter;

	//public Vector2 dimensions;
	public GameObject corner, line, filling, textContent, textContent2, arrowBase;
	public string type;
	static int sorts = 0;
	public string message;

	int step = 0;
	public int font = 0;
	public int limitUp, limitDown, limitLeft, limitRight;

	public GameObject topLeft, topRight, bottomLeft, bottomRight, left, right, up, down, fill, text, arrow;

	Transform panel;
	Vector3 wheres = Vector3.zero;

	public float x = 0, y = 0;
	public float wid = 1, hei = 1;

	public void Startup () {
		sorts += 10;
		panel = this.transform; //.parent.gameObject.transform;
		corner.GetComponent<SpriteRenderer> ().sortingOrder += sorts;
		line.GetComponent<SpriteRenderer> ().sortingOrder += sorts;
		filling.GetComponent<SpriteRenderer> ().sortingOrder += sorts;
		arrowBase.transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder += sorts;
	}

	public void TextMenu (string s) {
		TextMenu td = transform.gameObject.AddComponent<TextMenu> ();
		td.textPileup = s; //
		td.Startup ();
		text.transform.localPosition += new Vector3 (0, 2, 0);
	}

	public void TextDialog (string s) {
		TextDialog td = this.gameObject.AddComponent<TextDialog> ();
		td.textPileup = s; //
		td.Startup ();
		text.transform.localPosition += new Vector3 (-14, 2, 0);

		arrow.SetActive (false);
	}

	public void Rescale (float x, float y, float w, float h) {

		wid = w;
		hei = h;
		wheres = new Vector3 (x, y, 0);

		foreach (Transform child in this.transform) {
			GameObject.Destroy (child.gameObject);
		}

		topLeft = (GameObject) Instantiate (corner, wheres, Quaternion.Euler (0, 0, 0));
		topRight = (GameObject) Instantiate (corner, wheres, Quaternion.Euler (0, 0, 270));
		bottomLeft = (GameObject) Instantiate (corner, wheres, Quaternion.Euler (0, 0, 90));
		bottomRight = (GameObject) Instantiate (corner, wheres, Quaternion.Euler (0, 0, 180));

		up = (GameObject) Instantiate (line, wheres, Quaternion.Euler (0, 0, 0));
		down = (GameObject) Instantiate (line, wheres, Quaternion.Euler (0, 0, 180));
		left = (GameObject) Instantiate (line, wheres, Quaternion.Euler (0, 0, 90));
		right = (GameObject) Instantiate (line, wheres, Quaternion.Euler (0, 0, 270));

		fill = (GameObject) Instantiate (filling, wheres, Quaternion.Euler (0, 0, 0));
		arrow = (GameObject) Instantiate (arrowBase, wheres, Quaternion.Euler (0, 0, 0));

		if (font == 0) {
			text = (GameObject) Instantiate (textContent, wheres + (new Vector3 (22, (hei - 0.5f) * 16, 0)), Quaternion.Euler (0, 0, 0));
		} else {
			text = (GameObject) Instantiate (textContent2, wheres + (new Vector3 (22, (hei - 0.5f) * 16, 0)), Quaternion.Euler (0, 0, 0));
		}

		// TODO:
		//text.GetComponent<MeshRenderer> ().sortingOrder = filling.GetComponent<SpriteRenderer> ().sortingOrder + 1;

		SetNames ();
		SetPositions ();
		SetScales ();

		up.transform.SetParent (this.transform, false);
		down.transform.SetParent (this.transform, false);
		left.transform.SetParent (this.transform, false);
		right.transform.SetParent (this.transform, false);
		topLeft.transform.SetParent (this.transform, false);
		topRight.transform.SetParent (this.transform, false);
		bottomLeft.transform.SetParent (this.transform, false);
		bottomRight.transform.SetParent (this.transform, false);

		text.transform.SetParent (this.transform, false);
		arrow.transform.SetParent (this.transform, false);
		fill.transform.SetParent (this.transform, false);

		transform.localPosition = new Vector3 (-380, -144, 0);
		//Assign();
	}

	void SetNames () {
		topLeft.name = "TopLeft";
		topRight.name = "TopRight";
		bottomLeft.name = "BottomLeft";
		bottomRight.name = "BottomRight";
		up.name = "Up";
		down.name = "Down";
		left.name = "Left";
		right.name = "Right";
		fill.name = "Fill";
		text.name = "Text";
	}

	void SetPositions () {
		topLeft.transform.localPosition = wheres + (new Vector3 (0, hei * 16, 0));
		topRight.transform.localPosition = wheres + (new Vector3 (wid * 16, hei * 16, 0));
		bottomLeft.transform.localPosition = wheres + (new Vector3 (0, 0, 0));
		bottomRight.transform.localPosition = wheres + (new Vector3 (wid * 16, 0, 0));
		up.transform.localPosition = wheres + (new Vector3 (wid * 8, hei * 16, 0));
		down.transform.localPosition = wheres + (new Vector3 (wid * 8, 0, 0));
		left.transform.localPosition = wheres + (new Vector3 (0, hei * 8, 0));
		right.transform.localPosition = wheres + (new Vector3 (wid * 16, hei * 8, 0));

		fill.transform.localPosition = wheres + (new Vector3 (wid * 8, hei * 8, 0));
		arrow.transform.localPosition = wheres + (new Vector3 (10, 4 + (hei - 1) * 16, 0));
		text.transform.localPosition = wheres + (new Vector3 (22, (hei - 0.5f) * 16, 0));
	}

	void SetScales () {
		up.transform.localScale = new Vector3 (64 * wid, 32, 1);
		down.transform.localScale = new Vector3 (64 * wid, 32, 1);
		left.transform.localScale = new Vector3 (64 * hei, 32, 1);
		right.transform.localScale = new Vector3 (64 * hei, 32, 1);
		fill.transform.localScale = new Vector3 (32 * wid, 32 * hei, 1);
	}

	public void Transite (float amt) {
		StartCoroutine (Transition (amt));
	}

	IEnumerator Transition (float x) {
		Vector3 startingPos = transform.localPosition;
		float t = 0.0f;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / 0.3f); ///
			transform.localPosition = Vector3.Lerp (startingPos, startingPos + new Vector3 (x, 0, 0), t);
			yield return 0;
		}
	}

	public void UpArrow () {
		if (BattleMaster.indexV > limitUp) {
			BattleMaster.indexV--;
			arrow.transform.localPosition = new Vector3 (10, 84 + 13.8f * -BattleMaster.indexV, 0);
		}
	}
	public void DownArrow () {
		if (BattleMaster.indexV < limitDown) {
			BattleMaster.indexV++;
			arrow.transform.localPosition = new Vector3 (10, 84 + 13.8f * -BattleMaster.indexV, 0);
		}
	}

	/*
	public GameObject SummonNameTag (string message, bool dynamic) {
		GameObject blox = (GameObject) Instantiate (BaseBox);
		blox.transform.localPosition = new Vector3 (-340, 168, 0);
		blox.transform.SetParent (this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker> ();

		boxMaker.font = 1;
		boxMaker.type = blox.name = "NameTag";
		if (dynamic) boxMaker.wid = 2 + message.Length * 0.6f; //go.name.Remove(0, 6);
		else { boxMaker.wid = 10; }
		boxMaker.hei = 1;
		boxMaker.Startup ();

		TextDialog td = blox.gameObject.AddComponent<TextDialog> ();
		td.textPileup = message; //go.name.Remove(0, 6);
		boxMaker.text.transform.localPosition += new Vector3 (-8, 7, 0); //-8, 7
		boxMaker.arrow.SetActive (false);
		boxMaker.Transite (60);

		return blox;
	}

	public GameObject SummonSpellList () {
		GameObject blox = (GameObject) Instantiate (BaseBox); //blox.transform.localPosition = new Vector3(-340, -144, 0);
		blox.transform.SetParent (this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker> ();

		boxMaker.type = blox.name = "SpellMenu";
		boxMaker.message = name;

		string spellList = "";
		int amt = 0;

		//foreach (SpellEntry sp in BattleMaster.OnTurn.SpellsList)
		///{
		//if(sp.name.Length < 9)	
		//	spellList += sp.Name + " \n";
		//	amt++;
		//else{
		//	spellList += (sp.name.Remove(6, 8)) + "... \n";
		//}
		//}		

		boxMaker.limitDown = amt - 1;

		boxMaker.Rescale (0, 0, 8, 6);
		boxMaker.TextDialog (spellList);
		boxMaker.text.transform.localPosition += new Vector3 (0, 2, 0);

		//boxMaker.Transite(370);
		return blox;
	}

	public GameObject SummonSpellData () {
		GameObject blox = (GameObject) Instantiate (BaseBox);
		blox.transform.position = new Vector3 (-340, -144, 0);
		blox.transform.SetParent (this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker> ();
		//boxMaker.Transite(200);

		boxMaker.font = 1;
		boxMaker.type = blox.name = "SpellMenu";
		boxMaker.message = name; //go.name.Remove(0, 6);
		boxMaker.wid = 6;
		boxMaker.hei = 6;

		boxMaker.arrow.SetActive (false);
		boxMaker.Transite (60);

		return blox;
	}

	public GameObject SummonDialog (string s) {
		GameObject blox = (GameObject) Instantiate (BaseBox);
		blox.transform.localPosition += new Vector3 (0, -32, 0);
		blox.transform.SetParent (this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker> ();

		boxMaker.type = blox.name = "DialogBubble";
		boxMaker.message = name;

		boxMaker.Rescale (0, 0, 16, 5);
		boxMaker.TextDialog (s);

		return blox;
	}

	/* 
		public GameObject SummonBattleMenu2 () {
			GameObject blox = (GameObject) Instantiate(boxBase, Vector3.zero, Quaternion.Euler (0, 0, 0));
			blox.transform.SetParent(this.transform, false);
			BoxMaker boxMaker = blox.GetComponent<BoxMaker>();
			//boxMaker.Transite(200);

			boxMaker.font = 0;
			boxMaker.type = blox.name = "BattleMenu";
			boxMaker.message = name;//go.name.Remove(0, 6);
			//boxMaker.wid = 5.5f;
			//boxMaker.hei = 6;

			boxMaker.limitDown = 6;

			boxMaker.Startup();
			boxMaker.Rescale(0, 0, 5.5f, 6);
			blox.transform.localPosition = new Vector3(-340, -144, 0);
			boxMaker.Assign();

			TextMenu td = blox.AddComponent<TextMenu>();
			td.textPileup = "Move \nAttack \nSpell \nItem \nInfo \nPass"; //
			td.Startup();
			
			boxMaker.text.transform.localPosition += new Vector3 (0, 2, 0);


			return blox;
		}

	*/

}