using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MapLoader : MonoBehaviour {

	GameboardMaster GameBoard; 
	BattleMaster BattleMaster;

	public void Load () {

		string MapWriteOutput = "";

		TextAsset textAsset = (TextAsset) Resources.Load("GameMaps/" + "snowy2" + "_Blocks", typeof(TextAsset)); 
		string[] whole_text = Regex.Split(textAsset.text, "\n"); 

		BattleMaster = GetComponent<BattleMaster>();
		BattleMaster.GameBoard.Startup(Regex.Split(whole_text[0], ",").Length, whole_text.Length-1);
		GameBoard = BattleMaster.GameBoard;

		for (int x = whole_text.Length-2; x >= 0; x--) {
			string[] rowi = Regex.Split(whole_text[whole_text.Length-2-x], ","); 
			MapWriteOutput += x + " ";
			for (int z = 0; z < rowi.Length; z++) {
				Point keyp = new Point(z, x); 
				string key = keyp.x + " " + keyp.z; 
				//Debug.Log(key);
				if (int.Parse(rowi[z]) > 0) {
					//firstTable.Add(key, 1); 
					MapWriteOutput += "X ";
					GameBoard.SetGround(1, keyp);
				}else {
					//firstTable.Add(key, 0); 
					MapWriteOutput += "_ ";
					GameBoard.SetGround(0, keyp);
				}
			}
			MapWriteOutput += "\n";
		}

		WriteMaster.WriteUp("MapOutput", MapWriteOutput);

	}
	
}
