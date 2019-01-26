using System.Collections.Generic;
using UnityEngine;

public class CanvasMaster : MonoBehaviour {

	[SerializeField]
	GameObject BaseBox, Box2, Box3;
	public GameObject boxBase;
	public GameState stackTop;
	public GameObject ShakingTextBlox, SlidingTextBlox;

	void Start () {

		BaseBox = (GameObject) Resources.Load ("Prefabs/BaseBox", typeof (GameObject));
		//this.transform.eulerAngles = new Vector3(-38.65f, 36.24f, 0);

	}

	public void Cleanup () {

		foreach (Transform child in this.transform) {
			GameObject.Destroy (child.gameObject);
		}

	}

	UIRegularBox InstantiateBox (Point Where, Point Size) {

		GameObject Box = (GameObject) Instantiate (BaseBox, new Vector3 (Where.x, Where.z, 0), Quaternion.identity);
		Box.transform.SetParent (this.transform, false);

		UIRegularBox RegularBox = Box.AddComponent<UIRegularBox> ();
		RegularBox.Rescale (Size);

		return RegularBox;

	}

	public void Updater () {
		switch (stackTop.Selecter) {
			case E.ARROW_UPDOWN:
				if (Input.GetKeyDown ("up")) {
					//foreach (GameObject blox in stackTop.windows){
					//	stackTop.Windows[0].GetComponent<BoxMaker>().UpArrow();
					//}
				}
				if (Input.GetKeyDown ("down")) {
					//foreach (GameObject blox in stackTop.windows){
					//	stackTop.Windows[0].GetComponent<BoxMaker>().DownArrow();
					//}
				}

				break;

			case E.ARROW:

				break;
		}
	}

	public void ResetArrow () {
		foreach (GameObject blox in stackTop.Windows) {
			//while(BattleMaster.indexV > 0) blox.GetComponent<BoxMaker>().UpArrow();

			////while(BattleMaster.indexH > 0) 
		}
	}

	public void SpawnSpellName2 (string SpellName) {
		Debug.Log ("HI");
		GameObject blox = (GameObject) Instantiate (ShakingTextBlox, new Vector3 (140, -216, 0), new Quaternion ());
		blox.transform.localScale = new Vector3 (1, 80, 1);
		ShakingText st = blox.transform.GetComponent<ShakingText> ();
		st.Text = SpellName.ToUpper ();
		blox.AddComponent<SizeGrow> ();
		blox.transform.SetParent (this.transform, false);
	}

	public void SpawnSpellName (string SpellName) {
		List<GameObject> BloxList = new List<GameObject> ();
		int LetterCount = SpellName.Length;

		for (int i = 0; i < 8; i++) {
			GameObject blox = (GameObject) Instantiate (SlidingTextBlox, new Vector3 (((LetterCount * 30 + 10) * i) - 260, -216, 0), new Quaternion ());
			blox.transform.localScale = new Vector3 (80, 80, 1);
			SlidingText st = blox.transform.GetComponent<SlidingText> ();
			blox.transform.SetParent (this.transform, false);
			st.Startup (SpellName.ToUpper ());
		}

		foreach (GameObject go in BloxList) {

		}

		//	blox.AddComponent<SizeGrow>();
	}

	public void SummonBattleMenu () {

		Point Where = new Point (-280, -90);
		Point Size = new Point (14, 16);

		InstantiateBox (Where, Size);

		//return Blox;

		//BoxMaker boxMaker = blox.GetComponent<BoxMaker> ();

		//boxMaker.type = blox.name = "BattleMenu";
		//boxMaker.message = name;
		//boxMaker.limitDown = 5;

		//boxMaker.Rescale (0, 0, 5.5f, 6);
		//boxMaker.TextMenu ("Move \nAttack \nSpell \nItem \nInfo \nPass");

		//return Blox;

	}

}