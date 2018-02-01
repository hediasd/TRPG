using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class BattleMaster : MonoBehaviour {

	public static List<List<Monster>> Teams = new List<List<Monster>>();
	//public static List<GameObject> Allies = new List<GameObject>();
	//public static List<GameObject> Enemies = new List<GameObject>();
	public static List<Monster> Allmons = new List<Monster>();
	public static List<Monster> UpTurn = new List<Monster>();
	public static List<Monster> HadTurn = new List<Monster>();

	//[HideInInspector]
	public GameObject canvas, chooser, overlays, pieces; 
	[HideInInspector]
	public static Monster Selected, OnTurn;
	public Grimoire Grimoire;

	CanvasMaster CanvasMaster;
	ChooserMaster ChooserMaster;
	public GameboardMaster GameboardMaster;
	OverlaysMaster OverlaysMaster;
	public PiecesMaster PiecesMaster;
	PlanningMaster PlanningMaster;
	TimeMaster TimeMaster;

	public Stack<GameState> States = new Stack<GameState>();
	public Deque<BoardAction> GameboardActions = new Deque<BoardAction>();
	public Deque<BoardAction> PieceActions = new Deque<BoardAction>();
	public static bool Acting = false;

	public static string log;

	public static List<Lock> Locks = new List<Lock>();
	public static List<Lock> Unlocks = new List<Lock>();
	
	public static int indexV, indexH;
	int IDs = 0;
	int TurnNumber = 0;
	//public static bool OnHang = false;

	void OnApplicationQuit()
	{
		WriteMaster.WriteUp("Logh", log);
	}
	public static void Log(string s, bool JumpLine = true){ // TODO: AutoLog with pieceactions 
		log += s;
		if(JumpLine) log += "\n";
	}

	public void SpawnMonster(Monster MonsterSample, Point Position, int TeamNumber){
		IDs += 1;
		Monster SpawnedMonster = MonsterSample.Copy();
		SpawnedMonster.ID = IDs;
		SpawnedMonster.Team = TeamNumber;
		Teams[TeamNumber].Add(SpawnedMonster);

		GameboardMaster.InsertMonster(SpawnedMonster, Position);
		PiecesMaster.SpawnMonsterPiece(SpawnedMonster, Position);
	}

	List<int> Randoms(){
		List<int> nums = new List<int>();
		for (int i = 0; i < Grimoire.Monsters.Count; i++)
		{
			nums.Add(i);
		}
		nums.Sort((x, y) => Random.Range(-1, 2));
		return nums;
	}
	List<int> Rands;
	int w = -1;
	int RandomGr(){
		if(Rands == null){
			Rands = Randoms();
		}
		w += 1;
		return Rands[w];
	}

	void Start () {

		Teams.Add(new List<Monster>());
		Teams.Add(new List<Monster>());

		TimeMaster = GetComponent<TimeMaster>();
		GameboardMaster = GetComponent<GameboardMaster>();

		MapLoader MapLoader = GetComponent<MapLoader>();
		MapLoader.Load();

		Grimoire = GetComponent<Grimoire>();
		CanvasMaster = canvas.GetComponent<CanvasMaster>();
		ChooserMaster = chooser.GetComponent<ChooserMaster>();
		OverlaysMaster = overlays.GetComponent<OverlaysMaster>();
		PiecesMaster = pieces.GetComponent<PiecesMaster>();	
		PlanningMaster = GetComponent<PlanningMaster>();	

		Random.InitState(System.DateTime.Now.Millisecond); //(int) System.DateTime.Now.Millisecond * 77); //
		//Grimoire.Monsters[RandomGr()]
		SpawnMonster(Grimoire.GetMonster("Ice Knight"), new Point(9, 3), 0);
		SpawnMonster(Grimoire.GetMonster("Yeti"), new Point(11, 1), 0);
		SpawnMonster(Grimoire.GetMonster("Yeti"), new Point(11, 3), 0);

		SpawnMonster(Grimoire.GetMonster("Prophet of Wicked"), new Point(10, 13), 1);
		SpawnMonster(Grimoire.GetMonster("Yatagarasu"), new Point(10, 11), 1);
		SpawnMonster(Grimoire.GetMonster("Yatagarasu"), new Point(11, 12), 1);

		Allmons.AddRange(Teams[0]);
		Allmons.AddRange(Teams[1]);
		UpTurn.AddRange(Teams[0]);
		UpTurn.AddRange(Teams[1]);
		UpTurn.Sort((p1, p2) => p1.ID.CompareTo(p2.ID));
		//UpTurn.Sort((p1, p2) => p2.GetComponent<Monster>().SPD_ - p1.GetComponent<Monster>().SPD_);

		Selected = OnTurn = Teams[0][0];
		TurnWheel();
		//StatePush(new GameState(E.BATTLE_MENU, E.ARROW_UPDOWN), false);
		//GameObject blox = CanvasMaster.SummonBattleMenu();

		//Debug.Log(Environment.GameBoard.toString());

	}

	void TurnWheel(){

		// If no candidates for next turn, refill list of candidates
		if(UpTurn.Count == 0){
			UpTurn.AddRange(HadTurn);
			HadTurn.Clear();
		}

		//next candidate on the line
		OnTurn = UpTurn[0]; 

		TurnNumber += 1;
		BattleMaster.Log("--- TURN "+TurnNumber+" - ["+OnTurn.Name+"]"+"'s turn - ["+OnTurn.StatList.HPA()+"/"+OnTurn.StatList.HPM()+" HP]");

		if(Teams[0].Contains(OnTurn)) {
			StatePush(new GameState(E.BATTLE_MENU, E.ARROW_UPDOWN), false);
			GameObject blox = CanvasMaster.SummonBattleMenu();
			States.Peek().windows.Add(blox);
		}

		UpTurn.Remove(OnTurn);
		HadTurn.Add(OnTurn);

		// Starts turn
		//Actions.Enqueue(new GlobalAction(E.ON_TURN_START));
		//Actions = PlanningMaster.Thinking(MonsterOnTurn);

	}

	// Turn Machine
	void Update () {

		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			Camera.main.orthographicSize += 0.25f;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			Camera.main.orthographicSize -= 0.25f;
		}

		
		if(GameboardActions.Count > 0 || Acting){
			if(Acting ){ ///|| PiecesMaster.Actors > 0
				//
			}else{
				Acting = true;
				ProcessBoardAction();
			}
		}else if(Locks.Count > 0){
			//
		}else{
			Unlocker();
			/*if(Teams[0].Contains(OnTurn)){
				CanvasMaster.Updater();
				PiecesMaster.Updater();

				switch(States.Peek().state){
					case E.BATTLE_MENU:
						BattleMenuUpdate();
						break;
					case E.MOVE:
						MoveUpdate();
						break;
					case E.SPELL:
						SpellUpdate();
						break;
					case E.ITEM:
						ItemUpdate();
						break;
				}
				
			}else{ //Enemy*/
			StartCoroutine(Think());
				//EnemyTurn();
			//}
		}

	}


	void ProcessBoardAction(){

		try
		{
			if(GameboardActions.Peek() is GlobalAction){
				GlobalAction Action = (GlobalAction) GameboardActions.Dequeue();
				switch(Action.Trigger){
					case(E.ON_TURN_START):

					break;
					case(E.ON_TURN_END):

					break;

				}
			}else if(GameboardActions.Peek() is PieceMove){
				// TODO: This is awful
				// TODO: Add logs when Gameboard does stuff
				PieceMove Action = (PieceMove) GameboardActions.Dequeue();
				GameboardMaster.WalkMonster(Action.from, Action.to);

				// TODO: SPLIT INTO TWO PROCESSERS
				// WILL BE ABLE TO DO LOTS OF THINKING WHILE PIECES MOVE AND SPELLS GO
				PiecesMaster.WalkTo(Action.who, Action.to, Action.pointpath);
				BattleMaster.Log("%%% walks from "+Action.from+" to "+Action.to);

			}else if(GameboardActions.Peek() is PieceSpell){

				PieceSpell Action = (PieceSpell) GameboardActions.Dequeue();
				List<Damage> DamagesList = GameboardMaster.SimulateSpellPerformance(Action.mon, Action.sp, Action.to);
				//	Board
				CanvasMaster.SpawnSpellName(Action.sp.Name);
				PiecesMaster.SpawnDamage(DamagesList);
				PiecesMaster.SpawnAnimation(Action); //

				List<bool> Results = GameboardMaster.DealDamage(DamagesList);

				for (int i = 0; i < Results.Count; i++)
				{ // TODO: Move to gameboard master
					if(Results[i]){
						Monster DeadMonster = DamagesList[i].TargetMonster;
						Allmons.Remove(DeadMonster);
						UpTurn.Remove(DeadMonster);
						HadTurn.Remove(DeadMonster);
						Teams[DeadMonster.Team].Remove(DeadMonster);
						GameboardMaster.RemoveMonster(DeadMonster);
						Destroy(PiecesMaster.MonsterGameObject(DamagesList[i].TargetMonster));

					}
				}

			//	PiecesMaster.SpawnTerrain(Grimoire.Terrains[0], ps.to);
				BattleMaster.Log("%%% casts ["+Action.sp.Name+"]");
				BattleMaster.Log("%%% spell ["+Action.sp.Name+"] damages ", JumpLine: false);

				for (int i = 0; i < DamagesList.Count; i++)
				{
					string TargetName = DamagesList[i].TargetMonster.Name;
					int TotalDamage = DamagesList[i].FinalDamage;
					string Endline = (i<DamagesList.Count-1) ? ", " : "\n";
					BattleMaster.Log("["+TargetName+"] for "+TotalDamage+Endline, false);
				}

				//Acting = false;
				//SfxEffect("Bright", );
			}else{
				
				GameboardActions.Dequeue();
				Debug.Log("Unknown Board Action Unprocessed");

			}	
		}
		catch (GameboardException)
		{
			Acting = false;
			Debug.Log("Gameboard Threwup");
			GameboardActions.Clear(); //throwup
			Locks.Clear();
		}
			
	}

	void ProcessPieceAction(){

		if(PieceActions.Peek() is GlobalAction){
			GlobalAction Action = (GlobalAction) PieceActions.Dequeue();
			switch(Action.Trigger){
				case(E.ON_TURN_START):

				break;
				case(E.ON_TURN_END):

				break;

			}
		}else if(PieceActions.Peek() is PieceMove){

			PieceMove Action = (PieceMove) PieceActions.Dequeue();
			PiecesMaster.WalkTo(Action.who, Action.to, Action.pointpath);

		}else if(PieceActions.Peek() is PieceSpell){

			PieceSpell Action = (PieceSpell) PieceActions.Dequeue();
			//	Board
			//CanvasMaster.SpawnSpellName(Action.sp.Name);
			//PiecesMaster.SpawnDamage(DamagesList);
			//PiecesMaster.SpawnAnimation(Action); //

		}else{
			
			PieceActions.Dequeue();
			Debug.Log("Unknown Piece Action Unprocessed");

		}
	}


	void BattleMenuUpdate(){

		switch (Input.inputString){
			case "c":
				GameState newState;
				switch(indexV){
					case 0: //Move
						newState = new GameState(E.MOVE, E.CHOOSER);
						Point p = new Point(OnTurn);
						//Point t = new Point(OnTurn.transform.position.x+1, OnTurn.transform.position.z);
						//Spell sp = new Spell(); -r -g `$(File):$(Line)`
						//sp.effectArea = E.CIRCLE;
						//newState.windows.Add(OverlaysMaster.SpawnMoveCells(sp.Shape(p, t, 3), 2));
						newState.windows.Add(OverlaysMaster.SpawnMoveCells(p, 3));
						StatePush(newState, true);
					break;

					case 1: //Attack
						//
					break;

					case 2: //Spell
						newState = new GameState(E.SPELL, E.ARROW_UPDOWN);
						GameObject spellList = CanvasMaster.SummonSpellList(); //Arrow at first
						GameObject spellData = CanvasMaster.SummonSpellData();
						GameObject nameTag = CanvasMaster.SummonNameTag(OnTurn.Name, false);
						newState.windows.Add(spellList);
						newState.windows.Add(spellData);
						newState.windows.Add(nameTag);
						StatePush(newState, true);
					break;

					case 3: //Item
						newState = new GameState(E.ITEM);
						GameObject dialogBubble = CanvasMaster.SummonDialog("QUAL ERA O JOGO SURPRESA ?"); //Arrow at first
						newState.windows.Add(dialogBubble);
						StatePush(newState, true);
					break;
				}
			break;

			case "x":				
			break;
			case "z":
			break;

		}
	}

	void MoveUpdate(){
		switch (Input.inputString){
			case "c":
				OverlaysMaster.CleanUp();
				StatePop(2); // TODO: THIS IS AWFUL
				int id = Environment.GameBoard.MonsterIDAt(new Point(OnTurn));
				Monster mon = Environment.GameBoard.MonstersOnBoard[id];
				Debug.Log("fix this");
				GameboardActions.Enqueue(new PieceMove(mon, Environment.GameBoard.MonsterPosition(mon), new Point(chooser), null));
				TurnWheel();
			break;
			case "x":
				StatePop(1);
			break;
			case "z":
				//
			break;
		}
	}

	void SpellUpdate(){
		switch (Input.inputString){
			case "c":
			break;
			case "x":
				StatePop(1);
			break;
		}
	}

	void ItemUpdate(){
		switch (Input.inputString){
			case "c":
			//piecesMaster.WalkTo(selected, chooser);
			//overlaysMaster.CleanUp("MovementCells");
			//StatePop(1);
			break;
			case "x":
				StatePop(1);
			break;
		}
	}

	void StatePush(GameState gs, bool hideMenu){
		if(States.Count > 0){
			if(hideMenu){
				foreach (GameObject go in States.Peek().windows){
					go.SetActive(false);
				}
			}
		}
		States.Push(gs);
		OnStateEnter(States.Peek());
	}

	void StatePop(int times){
		for (int i = 0; i < times; i++){
			foreach (GameObject go in States.Pop().windows){
				Destroy(go);
			}
		}
		if(States.Count > 0){
			foreach (GameObject go in States.Peek().windows){
					go.SetActive(true);
			}
			OnStateEnter(States.Peek());
		}
	}

	void OnStateEnter(GameState gs){
		if(indexV != 0 || indexH != 0) CanvasMaster.ResetArrow();
		CanvasMaster.stackTop = PiecesMaster.stackTop = States.Peek();
	}


	public static void AddLock(Lock ck){
		Locks.Add(ck);
	}
	public static void ReleaseLock(Lock ck){
		Unlocks.Add(ck);
		Locks.Remove(ck);
	}
	public void Unlocker(){
		foreach (Lock ck in Unlocks)
		{
			foreach (int c in ck.code)
			{
				switch (c){
					case E.FLUSH:
						StatePop(States.Count);
					break;
					case E.POP1:
						StatePop(1);
					break;
					case E.POP2:
						StatePop(2);
					break;
					case E.POP3:
						StatePop(3);
					break;
					case E.TURN_WHEEL:
						TurnWheel();
					break;
					case E.WAIT:
						TimeMaster.WaitSeconds(0.5f);
					break;
				}
			}	
		}
		Unlocks.Clear();
	}

	IEnumerator Think() {
		StatePush(new GameState(E.ENEMY_TURN, E.NONE), true);
		Lock ck = new Lock(E.WAIT, E.FLUSH, E.TURN_WHEEL);
		AddLock(ck);

		Monster MonsterOnTurn = OnTurn;

		int type = 0;

		while(Acting){ //Prevents wrong data
			yield return 0;
		}

		int i, j;
		
		if(MonsterOnTurn.Team == 0){
			i = 0; j = 1;
		}else{
			i = 1; j = 0;
		}

		PlanningMaster.Feed(GameboardMaster, MonsterOnTurn, Teams[i], Teams[j]);
		switch (type)
		{
			case 0:
				GameboardActions = PlanningMaster.Thinking(MonsterOnTurn);
			break;
			case 1:

			break;
		}
		
		ReleaseLock(ck);
		yield return 0;

	}
	



}
