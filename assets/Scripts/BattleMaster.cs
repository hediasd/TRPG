using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BattleMaster : MonoBehaviour {

	public static List<List<Monster>> Teams = new List<List<Monster>> ();
	//public static List<GameObject> Allies = new List<GameObject>();
	//public static List<GameObject> Enemies = new List<GameObject>();
	public static List<Monster> Allmons = new List<Monster> ();
	public static List<Monster> UpTurn = new List<Monster> ();
	public static List<Monster> HadTurn = new List<Monster> ();

	//[HideInInspector]
	public GameObject canvas, chooser, overlays, pieces;
	[HideInInspector]
	public static Monster Selected, OnTurn;
	public ResourcesMaster ResourcesMaster;

	CanvasMaster CanvasMaster;
	ChooserMaster ChooserMaster;
	public GameboardMaster GameboardMaster;
	Gameboard Gameboard;
	MapMaster MapMaster;
	OverlaysMaster OverlaysMaster;
	public PiecesMaster PiecesMaster;
	PlanningMaster PlanningMaster;
	TimeMaster TimeMaster;

	public Stack<GameState> States = new Stack<GameState> ();
	public Deque<BoardAction> GameboardActions = new Deque<BoardAction> ();
	public Deque<BoardAction> PieceActions = new Deque<BoardAction> ();

	public static string log;

	public static List<Lock> Locks = new List<Lock> ();
	public static List<Lock> Unlocks = new List<Lock> ();

	public static int indexV, indexH;
	int IDs = 0;
	int TurnNumber = 0;
	//public static bool OnHang = false;

	void OnApplicationQuit () {
		WriteMaster.WriteUp ("Logh", log);
	}
	public static void Log (string s, bool JumpLine = true) { // TODO: AutoLog with pieceactions 
		log += s;
		if (JumpLine) log += "\n";
	}

	void Start () {

		TimeMaster = GetComponent<TimeMaster> ();
		GameboardMaster = GetComponent<GameboardMaster> ();
		Gameboard = GameboardMaster.Gameboard;

		ResourcesMaster = GetComponent<ResourcesMaster> ();
		CanvasMaster = GameObject.Find ("Canvas").GetComponent<CanvasMaster> ();
		ChooserMaster = chooser.GetComponent<ChooserMaster> ();
		OverlaysMaster = overlays.GetComponent<OverlaysMaster> ();
		PiecesMaster = GameObject.Find ("Pieces").GetComponent<PiecesMaster> ();
		PlanningMaster = GetComponent<PlanningMaster> ();
		MapMaster = GameObject.Find ("Map").GetComponent<MapMaster> ();

		Teams = new List<List<Monster>> ();
		Allmons = new List<Monster> ();
		UpTurn = new List<Monster> ();

		//StatePush(new GameState(E.BATTLE_MENU, E.ARROW_UPDOWN), false);
		//GameObject blox = CanvasMaster.SummonBattleMenu();
		//Debug.Log(Environment.GameBoard.toString());

		StartBattle ();

	}

	void StartBattle () {

		TurnNumber = 0;
		IDs = 0;
		PiecesMaster.AnimationsActing = 0;
		PiecesMaster.MonstersActing = 0;
		PiecesMaster.TextDamagesActing = 0;

		States.Clear ();
		GameboardActions.Clear ();
		PieceActions.Clear ();
		Locks.Clear ();
		Unlocks.Clear ();

		Teams.Clear ();
		Allmons.Clear ();
		UpTurn.Clear ();
		HadTurn.Clear ();

		Teams.Add (new List<Monster> ());
		Teams.Add (new List<Monster> ());

		CanvasMaster.Cleanup ();
		PiecesMaster.Cleanup ();
		MapMaster.Cleanup ();
		Gameboard.Cleanup ();

		MapMaster.Load (Gameboard, "desert1");

		Random.InitState (1);

		SpawnMonster (ResourcesMaster.GetMonster ("Tricky Viper"), new Point (9, 3), 0);
		SpawnMonster (ResourcesMaster.GetMonster ("Guide of Lost"), new Point (11, 1), 0);
		SpawnMonster (ResourcesMaster.GetMonster ("Tricky Viper"), new Point (11, 3), 0);

		SpawnMonster (ResourcesMaster.GetMonster ("Sandstone Golem"), new Point (10, 13), 1);
		SpawnMonster (ResourcesMaster.GetMonster ("Sandstone Golem"), new Point (10, 11), 1);
		SpawnMonster (ResourcesMaster.GetMonster ("Guide of Lost"), new Point (11, 12), 1);

		Allmons.AddRange (Teams[0]);
		Allmons.AddRange (Teams[1]);

		UpTurn.AddRange (Teams[0]);
		UpTurn.AddRange (Teams[1]);

		UpTurn.Sort ((p1, p2) => p1.ID.CompareTo (p2.ID));
		//UpTurn.Sort((p1, p2) => p2.GetComponent<Monster>().SPD_ - p1.GetComponent<Monster>().SPD_);

		Selected = OnTurn = Teams[0][0];
		TurnWheel ();

	}

	public void SpawnMonster (Monster MonsterSample, Point Position, int TeamNumber) {

		IDs += 1;
		if(MonsterSample == null) UberDebug.LogChannel("Error", "MonsterSample null");
		Monster SpawnedMonster = MonsterSample.Copy ();
		SpawnedMonster.ID = IDs;
		SpawnedMonster.Team = TeamNumber;
		Teams[TeamNumber].Add (SpawnedMonster);

		Gameboard.InsertMonster (SpawnedMonster, Position);
		PiecesMaster.SpawnMonsterPiece (SpawnedMonster, Position);

	}

	void TurnWheel () {

		// If no candidates for next turn, refill list of candidates
		if (UpTurn.Count == 0) {
			UpTurn.AddRange (HadTurn);
			HadTurn.Clear ();
		}

		//next candidate on the line
		OnTurn = UpTurn[0];

		TurnNumber += 1;

		BattleMaster.Log ("--- TURN " + TurnNumber + " - [" + OnTurn.Name + "]" + "'s turn - [" + OnTurn.StatsList.HPA () + "/" + OnTurn.StatsList.HPM () + " HP]");

		if (Teams[0].Contains (OnTurn)) {
			StatePush (new GameState (GAMESTATE.BATTLE_MENU, E.ARROW_UPDOWN), false);
			GameObject blox = CanvasMaster.SummonBattleMenu ();
			States.Peek ().Windows.Add (blox);
		}

		UpTurn.Remove (OnTurn);
		HadTurn.Add (OnTurn);

		// Starts turn
		//Actions.Enqueue(new GlobalAction(E.ON_TURN_START));
		//Actions = PlanningMaster.Thinking(MonsterOnTurn);

		if (TurnNumber == 45) StartBattle ();

	}

	// Turn Machine
	void Update () {

		if (GameboardActions.Count > 0) {
			ProcessBoardAction ();
		}

		if (PieceActions.Count > 0) { //} || Acting < 1){
			if (PiecesMaster.MonstersActing > 5) { ///|| PiecesMaster.Actors > 0
				//
			} else {
				ProcessPieceAction ();
			}
		} else if (Locks.Count > 0) {
			//
		} else {

			Unlocker ();
			StartCoroutine (Think ());

			//Unlocker();
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
			//TODO: //StartCoroutine(Think());
			//EnemyTurn();
			//}
		}

	}

	void ProcessBoardAction () {

		//while(!GameboardActions.IsEmpty()){
		try {
			if (GameboardActions.Peek () is GlobalAction) {
				GlobalAction Action = GameboardActions.Dequeue () as GlobalAction;
				switch (Action.Trigger) {
					case (E.ON_TURN_START):

						break;
					case (E.ON_TURN_END):

						break;

				}
			} else if (GameboardActions.Peek () is PieceMove) {
				// TODO: This is awful
				// TODO: Add logs when Gameboard does stuff
				PieceMove Action = GameboardActions.Dequeue () as PieceMove;
				Gameboard.WalkMonster (Action.from, Action.to);
				PieceActions.Enqueue (Action);
				BattleMaster.Log ("%%% walks from " + Action.from + " to " + Action.to);

			} else if (GameboardActions.Peek () is PieceSpell) {

				PieceSpell Action = GameboardActions.Dequeue () as PieceSpell;
				PieceActions.Enqueue (Action);

				List<Damage> DamagesList = Gameboard.SpellPerformance (Action.CasterMonster, Action.CastedSpell, Action.CastedTo);
				Utility.Each (DamagesList, i => PieceActions.Enqueue (new PieceText (i.TargetMonster, i.FinalDamage + "")));

				List<bool> KilledTargets = Gameboard.DealDamage (DamagesList);

				for (int i = 0; i < KilledTargets.Count; i++) {
					// TODO: Move to gameboard master
					if (KilledTargets[i]) {
						Monster DeadMonster = DamagesList[i].TargetMonster;
						Allmons.Remove (DeadMonster);
						UpTurn.Remove (DeadMonster);
						HadTurn.Remove (DeadMonster);
						Teams[DeadMonster.Team].Remove (DeadMonster);
						Gameboard.RemoveMonster (DeadMonster);

						PieceActions.Enqueue (new PieceKill (DeadMonster));

					}
				}

				//	PiecesMaster.SpawnTerrain(Grimoire.Terrains[0], ps.to);
				BattleMaster.Log ("%%% casts [" + Action.CastedSpell.Name + "]");
				BattleMaster.Log ("%%% spell [" + Action.CastedSpell.Name + "] damages ", JumpLine : false);

				for (int i = 0; i < DamagesList.Count; i++) {
					string TargetName = DamagesList[i].TargetMonster.Name;
					int TotalDamage = DamagesList[i].FinalDamage;
					string Endline = (i < DamagesList.Count - 1) ? ", " : "\n";
					BattleMaster.Log ("[" + TargetName + "] for " + TotalDamage + Endline, false);
				}

				//Acting = false;
				//SfxEffect("Bright", );
			} else {

				GameboardActions.Dequeue ();
				Debug.Log ("Unknown Board Action Unprocessed");

			}
		} catch (GameboardException) {
			PiecesMaster.MonstersActing = 0;
			Debug.Log ("Gameboard Threwup");
			GameboardActions.Clear (); //throwup
			Locks.Clear ();
		}
		//}
	}

	void ProcessPieceAction () {

		if (PieceActions.Peek () is PieceMove && PiecesMaster.MonstersActing < 1 && PiecesMaster.TextDamagesActing == 0) {

			PieceMove Action = (PieceMove) PieceActions.Dequeue ();
			PiecesMaster.WalkTo (Action.who, Action.to, Action.pointpath);

		} else if (PieceActions.Peek () is PieceSpell && PiecesMaster.MonstersActing < 1) {

			PieceSpell Action = (PieceSpell) PieceActions.Dequeue ();
			//CanvasMaster.SpawnSpellName(Action.sp.Name);
			PiecesMaster.SpawnAnimation (Action); //

		} else if (PieceActions.Peek () is PieceText && PiecesMaster.MonstersActing < 1) {
			TimeMaster.WaitSeconds (0.4f);
			while (PieceActions.Count > 0 && PieceActions.Peek () is PieceText) {
				PieceText Action = (PieceText) PieceActions.Dequeue ();
				PiecesMaster.SpawnDamageText (Action.who, Action.Text);
			}			

		} else if (PieceActions.Peek () is PieceKill && PiecesMaster.MonstersActing < 2) {

			PieceKill Action = (PieceKill) PieceActions.Dequeue ();
			Destroy (PiecesMaster.MonsterGameObject (Action.mon));

		} else {

			//PieceActions.Dequeue();
			//Debug.Log("Unknown Piece Action Unprocessed");

		}

	}

	void BattleMenuUpdate () {

		switch (Input.inputString) {
			case "c":
				GameState newState;
				switch (indexV) {
					case 0: //Move
						newState = new GameState (GAMESTATE.MOVE, E.CHOOSER);
						Point p = new Point (OnTurn);
						//Point t = new Point(OnTurn.transform.position.x+1, OnTurn.transform.position.z);
						//Spell sp = new Spell(); -r -g `$(File):$(Line)`
						//sp.effectArea = E.CIRCLE;
						//newState.windows.Add(OverlaysMaster.SpawnMoveCells(sp.Shape(p, t, 3), 2));
						newState.Windows.Add (OverlaysMaster.SpawnMoveCells (p, 3));
						StatePush (newState, true);
						break;

					case 1: //Attack
						//
						break;

					case 2: //Spell
						newState = new GameState (GAMESTATE.SPELL, E.ARROW_UPDOWN);
						GameObject spellList = CanvasMaster.SummonSpellList (); //Arrow at first
						GameObject spellData = CanvasMaster.SummonSpellData ();
						GameObject nameTag = CanvasMaster.SummonNameTag (OnTurn.Name, false);
						newState.Windows.Add (spellList);
						newState.Windows.Add (spellData);
						newState.Windows.Add (nameTag);
						StatePush (newState, true);
						break;

					case 3: //Item
						newState = new GameState (GAMESTATE.ITEM);
						GameObject dialogBubble = CanvasMaster.SummonDialog ("QUAL ERA O JOGO SURPRESA ?"); //Arrow at first
						newState.Windows.Add (dialogBubble);
						StatePush (newState, true);
						break;
				}
				break;

			case "x":
				break;
			case "z":
				break;

		}
	}

	void MoveUpdate () {
		switch (Input.inputString) {
			case "c":
				OverlaysMaster.CleanUp ();
				StatePop (2); // TODO: THIS IS AWFUL
				int id = Environment.GameBoard.MonsterIDAt (new Point (OnTurn));
				Monster mon = Environment.GameBoard.MonstersOnBoard[id];
				Debug.Log ("fix this");
				GameboardActions.Enqueue (new PieceMove (mon, Environment.GameBoard.MonsterPosition (mon), new Point (chooser), null));
				TurnWheel ();
				break;
			case "x":
				StatePop (1);
				break;
			case "z":
				//
				break;
		}
	}

	void SpellUpdate () {
		switch (Input.inputString) {
			case "c":
				break;
			case "x":
				StatePop (1);
				break;
		}
	}

	void ItemUpdate () {
		switch (Input.inputString) {
			case "c":
				//piecesMaster.WalkTo(selected, chooser);
				//overlaysMaster.CleanUp("MovementCells");
				//StatePop(1);
				break;
			case "x":
				StatePop (1);
				break;
		}
	}

	void StatePush (GameState gs, bool hideMenu) {
		if (States.Count > 0) {
			if (hideMenu) {
				foreach (GameObject go in States.Peek ().Windows) {
					go.SetActive (false);
				}
			}
		}
		States.Push (gs);
		OnStateEnter (States.Peek ());
	}

	void StatePop (int times) {
		for (int i = 0; i < times; i++) {
			foreach (GameObject go in States.Pop ().Windows) {
				Destroy (go);
			}
		}
		if (States.Count > 0) {
			foreach (GameObject go in States.Peek ().Windows) {
				go.SetActive (true);
			}
			OnStateEnter (States.Peek ());
		}
	}

	void OnStateEnter (GameState gs) {
		if (indexV != 0 || indexH != 0) CanvasMaster.ResetArrow ();
		CanvasMaster.stackTop = PiecesMaster.stackTop = States.Peek ();
	}

	public static void AddLock (Lock ck) {
		Locks.Add (ck);
	}
	public static void ReleaseLock (Lock ck) {
		Unlocks.Add (ck);
		Locks.Remove (ck);
	}
	public void Unlocker () {
		foreach (Lock ck in Unlocks) {
			foreach (int c in ck.code) {
				switch (c) {
					case LOCK.FLUSH:
						StatePop (States.Count);
						break;
					case LOCK.POP1:
						StatePop (1);
						break;
					case LOCK.POP2:
						StatePop (2);
						break;
					case LOCK.POP3:
						StatePop (3);
						break;
					case LOCK.TURN_WHEEL:
						TurnWheel ();
						break;
					case LOCK.WAIT:
						TimeMaster.WaitSeconds (0.5f);
						break;
				}
			}
		}
		Unlocks.Clear ();
	}

	IEnumerator Think () {

		StatePush (new GameState (GAMESTATE.ENEMY_TURN, GAMESTATE.NONE), true);
		Lock ck = new Lock (LOCK.WAIT, LOCK.FLUSH, LOCK.TURN_WHEEL);
		AddLock (ck);

		Monster MonsterOnTurn = OnTurn;

		int type = 0;
		int i, j;

		if (MonsterOnTurn.Team == 0) {
			i = 0;
			j = 1;
		} else {
			i = 1;
			j = 0;
		}

		PlanningMaster.Feed (GameboardMaster, MonsterOnTurn, Teams[i], Teams[j]);

		switch (type) {
			case 0:
				GameboardActions = (PlanningMaster.Thinking (MonsterOnTurn));
				break;
			case 1:

				break;
		}

		ReleaseLock (ck);
		yield return 0;

	}

}