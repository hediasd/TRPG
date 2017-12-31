using UnityEngine;
using System.Collections.Generic;

public class OverlaysMaster : MonoBehaviour {

	public GameObject blues, yellows, reds, movementCells;
	public GameObject grid, cellStore;

	public GameObject SpawnMoveCells(Point p, int radius){
		CleanUp("MovementCells");
		
		GameObject cellsFather = (GameObject) Instantiate(movementCells, new Vector3(0, 0, 0), Quaternion.Euler (0, 0, 0));
		cellsFather.name = "MovementCells";
		cellsFather.transform.SetParent (cellStore.transform, false);
		
		BlueCells cellsComp = cellsFather.GetComponent<BlueCells>();
		cellsComp.Startup(p, radius, blues);

		return cellsFather;
	}

	public GameObject SpawnSpellCells(List<Point> shape, int radius){
		//CleanUp("MovementCells");
		
		GameObject cellsFather = (GameObject) Instantiate(movementCells, new Vector3(0, 0, 0), Quaternion.Euler (0, 0, 0));
		cellsFather.name = "SpellCells";
		cellsFather.transform.SetParent (cellStore.transform, false);
		
		BlueCells cellsComp = cellsFather.GetComponent<BlueCells>();
		//cellsComp.x = p.x;
		//cellsComp.z = p.z;
		cellsComp.radius = radius;
		cellsComp.blueCell = yellows;
		//cellsComp.Startup();
		cellsComp.Feed(shape);

		return cellsFather;
	}

	public void CleanUp(string name){
		foreach (Transform child in cellStore.transform) {
     		if(child.gameObject.name.Equals(name)) GameObject.Destroy(child.gameObject);
 		}
	}

	public void CleanUp(){
		foreach (Transform child in cellStore.transform) {
     		GameObject.Destroy(child.gameObject);
 		}
	}



}
