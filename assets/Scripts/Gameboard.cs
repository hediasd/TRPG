using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard {

	public int[, , ] Board;
	public Dictionary<int, MonsterInstance> MonstersOnBoard;
	public Point size;

	public Gameboard Clone () {

		Gameboard g = new Gameboard ();

		g.size = new Point (size.x, size.z);
		g.Board = new int[size.x, size.z, 3];

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				for (int k = 0; k < 3; k++) {
					g.Board[i, j, k] = Board[i, j, k];
				}
			}
		}

		g.MonstersOnBoard = new Dictionary<int, MonsterInstance> ();
		foreach (int value in MonstersOnBoard.Keys) {
			g.MonstersOnBoard.Add (value, MonstersOnBoard[value]);
		}

		return g;

	}

	public void Startup (int x, int z) {

		size = new Point (x, z);
		Point.Limits = new Point (size);
		Board = new int[x, z, 3];

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				for (int k = 0; k < 3; k++) // TODO: AMOUNT OF LAYERS ?
				{
					Board[i, j, k] = 0;
				}
			}
		}

		MonstersOnBoard = new Dictionary<int, MonsterInstance> ();
		//System.Array.Copy(a, b, a.Length);
		//Debug.Log(string.Join("; ", a));
	}

	public int At (int x, int z, int layer = 0) {
		return Board[x, z, layer];
	}
	public int At (Point p, int layer = 0) {
		return Board[(int) p.x, (int) p.z, layer];
	}

	public void Cleanup () {
		if (size != null) Startup (size.x, size.z);
	}

	/*
		Deals the specified damage to each of its targets
		TODO: Return a DamageResult 
	 */
	public List<bool> DealDamage (List<Damage> DamageList) {

		List<bool> Success = new List<bool> ();
		foreach (Damage DamageInstance in DamageList) {
			MonsterInstance Target = MonstersOnBoard[DamageInstance.TargetID];
			Target.TakeDamage (DamageInstance);
			bool Killed = false;
			if (Target.Stats.HPA () == 0) Killed = Kill (Target);
			Success.Add (Killed);
		}
		return Success;

	}

	public List<MonsterInstance> GetMonsters () {
		return new List<MonsterInstance> (MonstersOnBoard.Values);
	}

	public Map GetWalkableMap () {
		int[, ] GroundMap = GetLayer (LAYER.GROUND);

		foreach (MonsterInstance Mon in MonstersOnBoard.Values) {
			GroundMap[Mon.MonsterPoint.x, Mon.MonsterPoint.z] = 1; //TODO:
		}

		Map ReturnMap = new Map (GroundMap);
		WriteMaster.WriteUp ("WalkableMap", GroundMap);

		return ReturnMap;
	}

	public void InsertMonster (MonsterInstance mon, Point at) {

		if (MonsterIDAt (at) != 0) {
			Debug.Log ("Illegal Board Add: Occupied Cell");
			throw new GameboardException ("Illegal Board Add: Occupied Cell");
		} else {
			int id = mon.ID;
			MonstersOnBoard.Add (id, mon);
			Set (id, at, LAYER.MONSTER);
		}
	}

	public bool IsWithinCastRange (MonsterInstance Caster, MonsterInstance Target, SpellInstance SimulatedSpell) {

		// How is the shape if i cast it from here
		List<Point> SpellCastShape = SimulatedSpell.CastShapePoints (Caster.MonsterPoint);

		return (SpellCastShape.Contains (Target.MonsterPoint));

	}

	public bool Kill (MonsterInstance Target) {

		return true;
	}

	public int MonsterIDAt (Point at) {
		if (!at.WithinLimits (size)) return 0;
		return Board[at.x, at.z, LAYER.MONSTER];
	}

	public MonsterInstance MonsterAt (Point at) {
		int id = MonsterIDAt (at);
		if (id == 0) {
			//Debug.Log("Illegal Board Search: Empty Cell");
			//throw new GameboardException();
			return null;
		} else {
			return MonstersOnBoard[id];
		}
	}

	public List<MonsterInstance> MonstersAt (Point Center, List<Point> Shape) {
		List<MonsterInstance> Monsters = new List<MonsterInstance> ();
		//foreach (Point p in Shape)
		//{
		//	Debug.Log(p);
		//}
		foreach (Point ShapePoint in Shape) {
			Point NewPoint = Center + ShapePoint;
			//Debug.Log("center " + Center.x + " " + Center.z + " plus " + ShapePoint.x + " " + ShapePoint.z );
			MonsterInstance MonsterAtPoint = MonsterAt (NewPoint);
			if (MonsterAtPoint != null) {
				Monsters.Add (MonsterAtPoint);
			}
		}

		return Monsters;
	}
	public Point GetMonsterPosition (MonsterInstance mon) {
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				if (mon.ID == Board[i, j, LAYER.MONSTER]) return new Point (i, j);
			}
		}
		return null;
	}

	public Deque<BoardAction> OnTurnEnter (MonsterInstance OnTurn) {
		Deque<BoardAction> Actions = new Deque<BoardAction> ();

		OnTurn.ResetMovementPoints ();

		return Actions;
	}

	public void RemoveMonster (MonsterInstance mon) {
		Point at = mon.MonsterPoint;
		if (Board[at.x, at.z, LAYER.MONSTER] != mon.ID) {
			Debug.Log ("Illegal Board Move: No/Wrong monster to remove");
			throw new GameboardException ("Illegal Board Move: No/Wrong monster to remove");
		} else {
			Set (0, at, LAYER.MONSTER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}

	public List<Damage> SpellPerformance (MonsterInstance Caster, SpellInstance SimulatedSpell, Point TargetedCell) {

		List<Damage> SimulationResult = new List<Damage> ();
		List<MonsterInstance> TargetedMonsters = new List<MonsterInstance> ();

		if (SimulatedSpell.Entry.Damage == "") {

		} else {
			//if(SimulatedSpell.Radius == 1){ //single target
			//TODO:	Debug.Log("do the radius 1");
			//}else{ //radius or multiple target
			//know shape = worth for terrains
			List<Point> SpellShape = SimulatedSpell.EffectShapePoints (Caster.MonsterPoint, TargetedCell);
			//know targets = important for choices
			TargetedMonsters.AddRange (MonstersAt (TargetedCell, SpellShape));
			//in the future, move this to outer layer perhaps
			//}
			return SimulatedSpell.DamageInstances (Caster, TargetedMonsters);
		}

		return SimulationResult;
	}

	//TODO

	public void SetGround (int Value, Point Point) {
		Set (Value, Point, LAYER.GROUND);
	}
	private void Set (int Value, Point p, int layer) {
		if (Value > 0 && layer == LAYER.MONSTER) { //moving someone
			MonstersOnBoard[Value].MonsterPoint = p;
		}
		Board[(int) p.x, (int) p.z, layer] = Value;

	}

	public int[, ] GetLayer (int level) {
		int[, ] layer = new int[size.x, size.z];

		//Copy matrix
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				layer[i, j] = Board[i, j, level];
			}
		}

		return layer;
	}

	public void WalkMonster(PieceMove Action){
		WalkMonster(Action.from, Action.to);
	}

	void WalkMonster (Point From, Point To) {
		bool EmptyFrom = Board[From.x, From.z, LAYER.MONSTER] == 0;
		bool FullTo = Board[To.x, To.z, LAYER.MONSTER] != 0;
		if (EmptyFrom || FullTo) {
			if (EmptyFrom) Debug.Log ("Illegal Board Move: Empty From");
			if (FullTo) Debug.Log ("Illegal Board Move: Full To");
			throw new GameboardException ("kkk");
		} else {
			MonsterInstance Mon = MonstersOnBoard[MonsterIDAt (From)];
			if (Point.Distance (From, To) > Mon.AvailableMovementPoints) {
				Debug.Log ("Illegal Board Move: No MP " + From + " " + To + " " + Point.Distance (From, To));
				throw new GameboardException ("Illegal Board Move: No MP " + From + " " + To + " " + Point.Distance (From, To));
			}
			Set (MonsterIDAt (From), To, LAYER.MONSTER);
			Set (0, From, LAYER.MONSTER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}

}