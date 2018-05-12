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

	public void Cleanup(){

		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}

	}

	public void Load (GameboardMaster Gameboard, string MapName) {

		//this.MapName = MapName;
		MapName = this.MapName;
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

		TextAsset[] MapCSVs = Resources.LoadAll<TextAsset>("GameMaps/" + MapName + "/");// + MapName + "_Blocks");
		List<Sprite> TilesetSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Tilesets/"+TilesetName));

		go.transform.Find("Terrain").Find("desert day").GetComponent<MeshFilter>().mesh.RecalculateNormals();
		
		for (int i = 0; i < MapCSVs.Length; i++)
		{
			TextAsset MapCSV = MapCSVs[i];
			string[] FileName = Regex.Split(MapCSV.name, "_");
			string[] MapMatrix = Regex.Split(MapCSV.text, "\n"); 

			if(i == 0){
				Gameboard.Startup(Regex.Split(MapMatrix[0], ",").Length, MapMatrix.Length-1);
			}

			if(FileName[1].Equals("Terrain")){
				continue;
			}
			if(!FileName[1].Equals("Blocks")){
				continue;
			}

			float Height = 0;
			if(FileName.Length > 2){
				Height = float.Parse(FileName[2]);
			}
			
			for (int x = MapMatrix.Length-2; x >= 0; x--) {

				string[] rowi = Regex.Split(MapMatrix[MapMatrix.Length-2-x], ","); 
				MapWriteOutput += x + " ";

				for (int z = 0; z < rowi.Length; z++) {
					
					Point p = new Point(z, x);
					int IdAtPoint = int.Parse(rowi[z]);
					int TileTypeAtPoint = TileType(IdAtPoint);

					if(IdAtPoint >= 0){

						GameObject cellSprite = Instantiate(cellSpr, new Vector3(p.x, Height, p.z), Quaternion.identity);
						cellSprite.transform.parent = blocks.transform;
						cellSprite.GetComponentInChildren<SpriteRenderer>().sprite = TilesetSprites[IdAtPoint];//.Find(Tile => Tile.name == (""+IdAtPoint));
						if(Height != 0) cellSprite.transform.GetChild(1).gameObject.SetActive(false);
					}
					
					string MapWriteCharacter = (TileTypeAtPoint == 0 ? "_ " : "X ");
					MapWriteOutput += MapWriteCharacter;
					if(Gameboard.At(p, E.GROUND_LAYER) < TileTypeAtPoint){
						Gameboard.SetGround(TileTypeAtPoint, p);
					}

				}

				MapWriteOutput += "\n";

			}

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
