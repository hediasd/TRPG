using UnityEngine; 
using System.Collections.Generic; 

public class Keys : MonoBehaviour {

	public GameObject pieces; 
	public GameObject chooser; 
	public GameObject logic;
	public GameObject camera1; 
	public GameObject cells1;
	public GameObject cells2;
	public GameObject spawn;
	public int aux;
	public GameObject canvas;
	public GameObject box;

	GameObject selected;
	Grimoire grimoire;
	PiecesMaster pieceMaster;
	GameObject blox;

	Point ch;
	int i = -1; 

	void Start(){
		grimoire = logic.GetComponent<Grimoire>();
		pieceMaster = pieces.GetComponent<PiecesMaster>();
	}
	
	void Updaterdsd(){

		int ax = 0;
		int bz = 0;
		float y = 0;

		if (Input.GetKeyDown("d")){
			if(selected == null){
				/*
				}*/
			}else{
				ch = new Point(chooser.transform.position.x, chooser.transform.position.z);
				/*
				Point se = new Point(selected.transform.position.x, selected.transform.position.z);
				selected.GetComponent<Piece>().Walk(Environment.GetPath(se.x, se.z, ch.x, ch.z));
				selected = null;
				cells1.GetComponent<BlueCells>().Feed(Environment.visited);
				*/
			}

			//GameObject bomb = (GameObject) Instantiate (spawn, new Vector3(chooser.transform.position.x, 0.0f, chooser.transform.position.z), Quaternion.Euler (0, 0, 0));
			//bomb.transform.SetParent (this.transform, false);

			ch = new Point(chooser.transform.position.x, chooser.transform.position.z);
			GameObject go = null;//pieceMaster.GetAt(ch);
			if(go != null){
				Destroy(blox);
				blox = (GameObject) Instantiate(box, Vector3.zero, Quaternion.Euler (0, 0, 0));
				blox.transform.SetParent(canvas.transform, false);
				blox.GetComponent<BoxMaker>().message = go.name.Remove(0, 6);
				blox.transform.localPosition = new Vector3(-600, 160, 0);
				blox.GetComponent<BoxMaker>().Transite(222);
			}else{
				for (int i = 0; i < 6; i++){
					Random.seed *= Random.seed/2;
					Monster m = grimoire.Monsters[Random.Range( 0, grimoire.Monsters.Count)]; //
					pieceMaster.SpawnMonsterPiece(m, new Point(ch.x + Random.Range(-2, 2), ch.z + Random.Range(-2, 2)));
				}
			}
						
			
		}

		if (Input.GetKeyDown("left")){
			ax--;
		}
		if (Input.GetKeyDown("right")){
			ax++;
		}
		if (Input.GetKeyDown("up")){
			bz++;
		}
		if (Input.GetKeyDown("down")){
			bz--;
		}

		if(ax+bz != 0){
			if(chooser.transform.GetChild(0).transform.position.y != 0) 
				y = 0.5f - chooser.transform.GetChild(0).transform.position.y;
			for (int c = 0; c < pieces.transform.childCount; c++)
			{
				if(pieces.transform.GetChild(c).transform.position.x == chooser.transform.position.x){
					if(pieces.transform.GetChild(c).transform.position.z == chooser.transform.position.z){
						int radius = 3;
						//Environment.table
					}
				}
			}
			camera1.GetComponent<CameraMove1>().goTo(chooser);
		}

		chooser.transform.position += new Vector3(ax, y, bz);


		
     	//Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax );





	}

	void uUpdate () {

		int j = -1; 
		int k = -1;

		if (Input.GetKeyDown("left")){
			i--;
			j++;
			if(i<0) i = pieces.transform.childCount - 1;
		}

		if (Input.GetKeyDown("right")){
			i++;
			j++;
			if(i > pieces.transform.childCount - 1) i = 0;
		} 

		if (Input.GetKeyDown("1")){
			k = 1;
//			logic.GetComponent<Environment>().Think(pieces.transform.GetChild(0).gameObject);
		}
		if (Input.GetKeyDown("2"))
		k = 2; 
		if (Input.GetKeyDown("3"))
		k = 3; 
		if (Input.GetKeyDown("4"))
		k = 4; 
		if (Input.GetKeyDown("5"))
		k = 5; 
		if (Input.GetKeyDown("6"))
		k = 6; 
		if (Input.GetKeyDown("7"))
		k = 7; 

		if (j != -1) {
			try {
				chooser.transform.position = pieces.transform.GetChild(i).gameObject.transform.position;
				camera1.GetComponent<CameraMove1> ().target = pieces.transform.GetChild(i).gameObject.transform.GetChild(0); 
				camera1.GetComponent<CameraMove1> ().goOn(); 
			}
			catch (System.Exception) {
			}
		}else if(k != -1){
				Random.seed = (int) System.DateTime.Now.Ticks; 

				//moves[1,0] = 1f; moves[1,1] = 0f;
				//moves[2,0] = 0f; moves[2,1] = 1f;
				//moves[3,0] = 1f; moves[3,1] = 0f;

				//pieces.transform.GetChild(k).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteAnim>().Walk(1.0f,0.0f);
				//camera1.GetComponent < CameraMove1 > ().goOn(); 
			
		}



		}

	
}
