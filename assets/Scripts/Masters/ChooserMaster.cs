using UnityEngine;
using System.Collections;

public class ChooserMaster : MonoBehaviour {

	public GameObject pieces;
	PiecesMaster PiecesMaster;

	void Start()
	{
		PiecesMaster = pieces.GetComponent<PiecesMaster>();		
	}

	/*	public GameObject GetSelectedMon(){
			GameObject selected = PiecesMaster.GetAt(new Point(transform.gameObject));
			foreach (GameObject go in BattleMaster.Allies)
			{
				if(go == selected) return go;
			}
			foreach (GameObject go in BattleMaster.Enemies)
			{
				if(go == selected) return go;
			}
			return null;
		}
	*/

	public void Move(float x, float y, float z){
		transform.position += new Vector3(x, y, z);
	}

}
