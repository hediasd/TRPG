using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard {

	public int[, , ] Board;
	public Dictionary<int, Monster> MonstersOnBoard;
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

		g.MonstersOnBoard = new Dictionary<int, Monster> ();
		foreach (int value in MonstersOnBoard.Keys) {
			g.MonstersOnBoard.Add (value, MonstersOnBoard[value]);
		}

		return g;

	}

	public void Startup (int x, int z) {

		size = new Point (x, z);
		Point.Limits = size;
		Board = new int[x, z, 3];

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				for (int k = 0; k < 3; k++) // TODO: AMOUNT OF LAYERS ?
				{
					Board[i, j, k] = 0;
				}
			}
		}

		MonstersOnBoard = new Dictionary<int, Monster> ();
		//System.Array.Copy(a, b, a.Length);
		//Debug.Log(string.Join("; ", a));
	}

	public void Cleanup () {
		if (size != null) Startup (size.x, size.z);
	}

	public List<Damage> SpellPerformance (Monster Caster, Spell SimulatedSpell, Point TargetedCell) {

		List<Damage> SimulationResult = new List<Damage> ();
		List<Monster> TargetedMonsters = new List<Monster> ();

		if (SimulatedSpell.Damage == "") {

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

	public Deque<BoardAction> OnTurnEnter (Monster OnTurn) {
		Deque<BoardAction> Actions = new Deque<BoardAction> ();

		OnTurn.ResetMovementPoints ();

		return Actions;
	}

	public List<bool> DealDamage (List<Damage> DamageList) {
		List<bool> Success = new List<bool> ();
		foreach (Damage DamageInstance in DamageList) {
			Monster Target = MonstersOnBoard[DamageInstance.TargetID];
			Target.TakeDamage (DamageInstance);
			bool Killed = false;
			if (Target.StatsList.HPA () == 0) Killed = Kill (Target);
			Success.Add (Killed);
		}
		return Success;
	}
	public bool Kill (Monster Target) {

		return true;
	}

	public void InsertMonster (Monster mon, Point at) {

		if (MonsterIDAt (at) != 0) {
			Debug.Log ("Illegal Board Add: Occupied Cell");
			throw new GameboardException ();
		} else {
			int id = mon.ID;
			MonstersOnBoard.Add (id, mon);
			Set (id, at, LAYER.MONSTER);
		}
	}
	public void WalkMonster (Point From, Point To) {
		bool EmptyFrom = Board[From.x, From.z, LAYER.MONSTER] == 0;
		bool FullTo = Board[To.x, To.z, LAYER.MONSTER] != 0;
		if (EmptyFrom || FullTo) {
			if (EmptyFrom) Debug.Log ("Illegal Board Move: Empty From");
			if (FullTo) Debug.Log ("Illegal Board Move: Full To");
			throw new GameboardException ();
		} else {
			Monster Mon = MonstersOnBoard[MonsterIDAt (From)];
			if (Point.Distance (From, To) > Mon.AvailableMovementPoints) {
				Debug.Log ("Illegal Board Move: No MP " + From + " " + To + " " + Point.Distance (From, To));
				throw new GameboardException ();
			}
			Set (MonsterIDAt (From), To, LAYER.MONSTER);
			Set (0, From, LAYER.MONSTER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}
	public void RemoveMonster (Monster mon) {
		Point at = mon.MonsterPoint;
		if (Board[at.x, at.z, LAYER.MONSTER] != mon.ID) {
			Debug.Log ("Illegal Board Move: No/Wrong monster to remove");
			throw new GameboardException ();
		} else {
			Set (0, at, LAYER.MONSTER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}

	public List<Monster> GetMonsters () {
		return new List<Monster> (MonstersOnBoard.Values);
	}

	public int MonsterIDAt (Point at) {
		if (!at.WithinLimits (size)) return 0;
		return Board[at.x, at.z, LAYER.MONSTER];
	}
	public Monster MonsterAt (Point at) {
		int id = MonsterIDAt (at);
		if (id == 0) {
			//Debug.Log("Illegal Board Search: Empty Cell");
			//throw new GameboardException();
			return null;
		} else {
			return MonstersOnBoard[id];
		}
	}
	public List<Monster> MonstersAt (Point Center, List<Point> Shape) {
		List<Monster> Monsters = new List<Monster> ();
		//foreach (Point p in Shape)
		//{
		//	Debug.Log(p);
		//}
		foreach (Point ShapePoint in Shape) {
			Point NewPoint = Center + ShapePoint;
			//Debug.Log("center " + Center.x + " " + Center.z + " plus " + ShapePoint.x + " " + ShapePoint.z );
			Monster MonsterAtPoint = MonsterAt (NewPoint);
			if (MonsterAtPoint != null) {
				Monsters.Add (MonsterAtPoint);
			}
		}

		return Monsters;
	}
	public Point MonsterPosition (Monster mon) {
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				if (mon.ID == Board[i, j, LAYER.MONSTER]) return new Point (i, j);
			}
		}
		return null;
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

	public int At (int x, int z, int layer = 0) {
		return Board[x, z, layer];
	}
	public int At (Point p, int layer = 0) {
		return Board[(int) p.x, (int) p.z, layer];
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

	public Map WalkableMap () {
		int[, ] GroundMap = GetLayer (LAYER.GROUND);

		foreach (Monster Mon in MonstersOnBoard.Values) {
			GroundMap[Mon.MonsterPoint.x, Mon.MonsterPoint.z] = 1; //TODO:
		}

		Map ReturnMap = new Map (GroundMap);
		WriteMaster.WriteUp ("WalkableMap", GroundMap);

		return ReturnMap;
	}

}