using UnityEngine;
using System.Collections.Generic;

public class CanvasMaster : MonoBehaviour {

	public GameObject boxBase;
	public GameState stackTop;
	public GameObject ShakingTextBlox, SlidingTextBlox;


	void Start()
	{
		BoxMaker boxMaker = boxBase.GetComponent<BoxMaker>();
		boxMaker.font = 0;
		boxMaker.type = boxBase.name = "Base Menu";
		boxMaker.message = name;
		boxMaker.Startup();

		//this.transform.eulerAngles = new Vector3(-38.65f, 36.24f, 0);

	}

	public void Cleanup(){

		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}

	}

	public void Updater(){
		switch (stackTop.selecter)
		{
			case E.ARROW_UPDOWN:
				if(Input.GetKeyDown("up")){
					//foreach (GameObject blox in stackTop.windows){
						stackTop.windows[0].GetComponent<BoxMaker>().UpArrow();
					//}
				}
				if(Input.GetKeyDown("down")){
					//foreach (GameObject blox in stackTop.windows){
						stackTop.windows[0].GetComponent<BoxMaker>().DownArrow();
					//}
				}				
				
			break;

			case E.ARROW:

			break;
		}
	}
	
	public void ResetArrow(){
		foreach (GameObject blox in stackTop.windows){
			while(BattleMaster.indexV > 0) blox.GetComponent<BoxMaker>().UpArrow();
			//while(BattleMaster.indexH > 0) 
		}
	}

	public void SpawnSpellName2(string SpellName){
		Debug.Log("HI");
		GameObject blox = (GameObject) Instantiate(ShakingTextBlox, new Vector3(140, -216, 0), new Quaternion());
		blox.transform.localScale = new Vector3(1, 80, 1);
		ShakingText st = blox.transform.GetComponent<ShakingText>();
		st.Text = SpellName.ToUpper();
		blox.AddComponent<SizeGrow>();
		blox.transform.SetParent(this.transform, false);
	}

	public void SpawnSpellName(string SpellName){
		List<GameObject> BloxList = new List<GameObject>();
		int LetterCount = SpellName.Length;

		for (int i = 0; i < 8; i++)
		{
			GameObject blox = (GameObject) Instantiate(SlidingTextBlox, new Vector3(((LetterCount*30 + 10) * i) - 260, -216, 0), new Quaternion());
			blox.transform.localScale = new Vector3(80, 80, 1);
			SlidingText st = blox.transform.GetComponent<SlidingText>();
			blox.transform.SetParent(this.transform, false);
			st.Startup(SpellName.ToUpper());
		}

		foreach (GameObject go in BloxList)
		{
					
		}
		
	//	blox.AddComponent<SizeGrow>();
	}

	public GameObject SummonBattleMenu () {
		GameObject blox = (GameObject) Instantiate(boxBase);
		blox.transform.SetParent(this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker>();

		boxMaker.type = blox.name = "BattleMenu";
		boxMaker.message = name;
		boxMaker.limitDown = 5;

		boxMaker.Rescale(0, 0, 5.5f, 6);
		boxMaker.TextMenu("Move \nAttack \nSpell \nItem \nInfo \nPass");

		return blox;
	}

	public GameObject SummonNameTag (string message, bool dynamic) {
		GameObject blox = (GameObject) Instantiate(boxBase);
		blox.transform.localPosition = new Vector3(-340, 168, 0);
		blox.transform.SetParent(this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker>();

		boxMaker.font = 1;
		boxMaker.type = blox.name = "NameTag";
		if(dynamic) boxMaker.wid = 2 + message.Length*0.6f; //go.name.Remove(0, 6);
		else{ boxMaker.wid = 10; }
		boxMaker.hei = 1;
		boxMaker.Startup();

		TextDialog td = blox.gameObject.AddComponent<TextDialog>();
		td.textPileup = message; //go.name.Remove(0, 6);
		boxMaker.text.transform.localPosition += new Vector3 (-8, 7, 0); //-8, 7
		boxMaker.arrow.SetActive(false);
		boxMaker.Transite(60);

		return blox;
	}

	public GameObject SummonSpellList () {
		GameObject blox = (GameObject) Instantiate(boxBase); //blox.transform.localPosition = new Vector3(-340, -144, 0);
		blox.transform.SetParent(this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker>();

		boxMaker.type = blox.name = "SpellMenu";
		boxMaker.message = name;

		string spellList = "";
		int amt = 0;

		foreach (Spell sp in BattleMaster.OnTurn.Spells_)
		{
			//if(sp.name.Length < 9)	
			spellList += sp.Name + " \n";
			amt++;
			//else{
			//	spellList += (sp.name.Remove(6, 8)) + "... \n";
			//}
		}		

		boxMaker.limitDown = amt-1;
		
		boxMaker.Rescale(0, 0, 8, 6);
		boxMaker.TextDialog(spellList);
		boxMaker.text.transform.localPosition += new Vector3 (0, 2, 0);

		//boxMaker.Transite(370);
		return blox;
	}
	public GameObject SummonSpellData () {
		GameObject blox = (GameObject) Instantiate(boxBase);
		blox.transform.position = new Vector3(-340, -144, 0);
		blox.transform.SetParent(this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker>();
		//boxMaker.Transite(200);

		boxMaker.font = 1;
		boxMaker.type = blox.name = "SpellMenu";
		boxMaker.message = name;//go.name.Remove(0, 6);
		boxMaker.wid = 6;
		boxMaker.hei = 6;

		boxMaker.arrow.SetActive(false);
		boxMaker.Transite(60);

		return blox;
	}

	public GameObject SummonDialog(string s){
		GameObject blox = (GameObject) Instantiate(boxBase);
		blox.transform.localPosition += new Vector3(0, -32, 0);
		blox.transform.SetParent(this.transform, false);
		BoxMaker boxMaker = blox.GetComponent<BoxMaker>();

		boxMaker.type = blox.name = "DialogBubble";
		boxMaker.message = name;

		boxMaker.Rescale(0, 0, 16, 5);
		boxMaker.TextDialog(s);

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
