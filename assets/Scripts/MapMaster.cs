using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Tiled2Unity;

public class MapMaster : MonoBehaviour {

	GameboardMaster Gameboard; 
	BattleMaster BattleMaster;

	public string MapName;
	public GameObject cellSpr;

	public void Load (GameboardMaster Gameboard, string MapName) {

		this.MapName = MapName;

		UnityEngine.Object prefab = Resources.Load(MapName);
		Debug.Log(prefab.name);

		GameObject go = (GameObject) Instantiate(prefab);
		go.transform.Translate(0, -0.062f, go.GetComponent<TiledMap>().NumTilesHigh);
		go.transform.Rotate(90, 0, 0);
		go.transform.parent = this.transform;

		GameObject blocks = (GameObject) Instantiate(new GameObject(), parent: go.transform);
		blocks.name = "Blocksss";

		string TilesetName = go.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterials[0].name;
		string MapWriteOutput = "";

		TextAsset textAsset = Resources.Load<TextAsset>("GameMaps/" + MapName + "/" + MapName + "_Blocks");
		List<Sprite> TilesetSprites = new List<Sprite>(Resources.LoadAll<Sprite>("TilesetSprites/"+TilesetName+"/"));

		string[] whole_text = Regex.Split(textAsset.text, "\n"); 

		Gameboard.Startup(Regex.Split(whole_text[0], ",").Length, whole_text.Length-1);
		 
		for (int x = whole_text.Length-2; x >= 0; x--) {

			string[] rowi = Regex.Split(whole_text[whole_text.Length-2-x], ","); 
			MapWriteOutput += x + " ";

			for (int z = 0; z < rowi.Length; z++) {
				
				Point p = new Point(z, x);
				
				int IdAtPoint = int.Parse(rowi[z]);
				int TileTypeAtPoint = TileType(IdAtPoint);

				GameObject cellSprite = Instantiate(cellSpr, new Vector3(p.x, 0, p.z), Quaternion.identity);
				cellSprite.transform.parent = blocks.transform;
				cellSprite.GetComponentInChildren<SpriteRenderer>().sprite = TilesetSprites.Find(Tile => Tile.name == (""+IdAtPoint));
				//Debug.Log(cellSprite.transform.position);
				string MapWriteCharacter = (TileTypeAtPoint == 0 ? "_ " : "X ");

				MapWriteOutput += MapWriteCharacter;
				Gameboard.SetGround(TileTypeAtPoint, p);

			}

			MapWriteOutput += "\n";

		}

		WriteMaster.WriteUp("MapOutput", MapWriteOutput);

	}

	int TileType(int id){
		int offset_id = id + 12;
		if(id < 0) offset_id += 1;
		int rest = offset_id % 12;

		if(rest >= 0 && rest <= 3)  return 0;
		if(rest >= 4 && rest <= 7)  return 1;
		if(rest >= 8 && rest <= 11) return 2;
		return -1;
	}
	
}
