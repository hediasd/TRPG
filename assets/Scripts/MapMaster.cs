using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Tiled2Unity;

public class MapMaster : MonoBehaviour {

	GameboardMaster Gameboard; 
	BattleMaster BattleMaster;

	public string MapName;
	//public GameObject cellSpr;

	public void Cleanup(){

		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
		}

	}

	public void Load (Gameboard Gameboard, string MapName) {

		//this.MapName = MapName;
		MapName = this.MapName;
		UnityEngine.Object prefab = Resources.Load(MapName);
		Debug.Log("Loading map: " + prefab.name);

		GameObject MapFloor = (GameObject) Instantiate(prefab);
		MapFloor.transform.Translate(0, -0.062f, MapFloor.GetComponent<TiledMap>().NumTilesHigh);
		MapFloor.transform.Rotate(90, 0, 0);
		MapFloor.transform.parent = this.transform;

		GameObject blocks = (GameObject) Instantiate(new GameObject(), parent: MapFloor.transform);
		blocks.name = "Blocksss";

		string TilesetName = MapFloor.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterials[0].name;
		string MapWriteOutput = "";

		TextAsset[] MapCSVs = Resources.LoadAll<TextAsset>("GameMaps/" + MapName + "/");// + MapName + "_Blocks");
		List<Sprite> TilesetSprites = new List<Sprite>(Resources.LoadAll<Sprite>("Tilesets/"+TilesetName));

		//go.transform.Find("Terrain").Find("desert day").GetComponent<MeshFilter>().mesh.RecalculateNormals();

		for (int i = 0; i < MapCSVs.Length; i++)
		{
			TextAsset MapCSV = MapCSVs[i];
			string[] FileName = Utility.ChewUp(MapCSV.name);//Regex.Split(MapCSV.name, /([^_]+)/g);
			string[] MapMatrix = Regex.Split(MapCSV.text, "\n"); 

			if(i == 0){
				Gameboard.Startup(Regex.Split(MapMatrix[0], ",").Length, MapMatrix.Length-1);
			}

			string FileMiddleName = FileName[1];

			if(FileMiddleName.Equals("Terrain")){
				continue;
			}
			//if(!FileName[1].Equals("Blocks")){
			//	continue;
			//}
			switch(FileMiddleName){
				case "Terrain":
				//
				continue;
				case "Blocks":
				
				break;
				case "NorthFaces":
				
				break;
				case "SouthFaces":
				
				break;
				case "EastFaces":
				
				break;
				case "WestFaces":
				
				break;
				
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
						GameObject cellSprite = Instantiate(ResourcesMaster.GetCellSprite(), new Vector3(p.x, Height, p.z), Quaternion.identity);
						cellSprite.transform.parent = blocks.transform;
						cellSprite.GetComponentInChildren<SpriteRenderer>().sprite = TilesetSprites[IdAtPoint];//.Find(Tile => Tile.name == (""+IdAtPoint));
						if(Height != 0) cellSprite.transform.GetChild(1).gameObject.SetActive(false);
					}
					
					string MapWriteCharacter = (TileTypeAtPoint == 0 ? "_ " : "X ");
					MapWriteOutput += MapWriteCharacter;
					if(Gameboard.At(p, LAYER.GROUND) < TileTypeAtPoint){
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
