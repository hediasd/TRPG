﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardMaster : MonoBehaviour{

	public int[,,] Board;
	public Dictionary<int, Monster> MonstersOnBoard;
	public Point size;

	public void Startup(int x, int z){

		size = new Point(x, z);
		Point.Limits = size;
		Board = new int[x, z, 3];

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.z; j++)
			{
				for (int k = 0; k < 3; k++) // TODO: AMOUNT OF LAYERS ?
				{
					Board[i, j, k] = 0;
				}
			}
		}

		MonstersOnBoard = new Dictionary<int, Monster>();
		//System.Array.Copy(a, b, a.Length);
		//Debug.Log(string.Join("; ", a));
	}

	public void Cleanup(){
		if(size != null) Startup(size.x, size.z);
	}

	public List<Damage> SimulateSpellPerformance(Monster Caster, Spell SimulatedSpell, Point TargetedCell){
		
		List<Damage> SimulationResult = new List<Damage>();
		List<Monster> TargetedMonsters = new List<Monster>();

		if(SimulatedSpell.Damage == ""){
			
		}else{
			//if(SimulatedSpell.Radius == 1){ //single target
			//TODO:	Debug.Log("do the radius 1");
			//}else{ //radius or multiple target
				//know shape = worth for terrains
				List<Point> SpellShape = SimulatedSpell.EffectShapePoints(Caster.MonsterPoint, TargetedCell);
				//know targets = important for choices
				TargetedMonsters.AddRange(MonstersAt(TargetedCell, SpellShape));
				//in the future, move this to outer layer perhaps
			//}
			return SimulatedSpell.DamageInstances(Caster, TargetedMonsters);
		}

		return SimulationResult;
	}

	public Deque<BoardAction> OnTurnEnter(Monster OnTurn){
		Deque<BoardAction> Actions = new Deque<BoardAction>();

		OnTurn.ResetMovementPoints();

		return Actions;
	}

	public List<bool> DealDamage(List<Damage> DamageList){
		List<bool> Success = new List<bool>();
		foreach (Damage DamageInstance in DamageList)
		{
			Monster Target = MonstersOnBoard[DamageInstance.TargetID];
			Target.TakeDamage(DamageInstance);
			bool Killed = false;
			if(Target.StatList.HPA() == 0) Killed = Kill(Target);
			Success.Add(Killed);
		}
		return Success;
	}
	public bool Kill(Monster Target){

		return true;
	}

	public void InsertMonster(Monster mon, Point at){
		
		if(MonsterIDAt(at) != 0){
			Debug.Log("Illegal Board Add: Occupied Cell");
			throw new GameboardException();
		}
		else{
			int id = mon.ID;
			MonstersOnBoard.Add(id, mon);
			Set(id, at, E.MONSTER_LAYER);
		}
	}
	public void WalkMonster(Point From, Point To){
		bool EmptyFrom = Board[From.x, From.z, E.MONSTER_LAYER] == 0;
		bool FullTo = Board[To.x, To.z, E.MONSTER_LAYER] != 0;
		if(EmptyFrom || FullTo){
			if(EmptyFrom) Debug.Log("Illegal Board Move: Empty From");
			if(FullTo) Debug.Log("Illegal Board Move: Full To");			
			throw new GameboardException();
		}
		else{
			Monster Mon = MonstersOnBoard[MonsterIDAt(From)];
			if(Point.Distance(From, To) > Mon.MovementPoints()){
				Debug.Log("Illegal Board Move: No MP "+From+" "+To+" "+Point.Distance(From, To));			
				throw new GameboardException();
			}
			Set(MonsterIDAt(From), To, E.MONSTER_LAYER);
			Set(0, From, E.MONSTER_LAYER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}	
	public void RemoveMonster(Monster mon){
		Point at = mon.MonsterPoint;
		if(Board[at.x, at.z, E.MONSTER_LAYER] != mon.ID){
			Debug.Log("Illegal Board Move: No/Wrong monster to remove");
			throw new GameboardException();
		}
		else{
			Set(0, at, E.MONSTER_LAYER);
			//Debug.Log("gone to " + to.x + " " + to.z);
		}
	}

	public List<Monster> GetMonsters(){
		return new List<Monster>(MonstersOnBoard.Values);
	}

	public int MonsterIDAt(Point at){
		if(!at.WithinLimits(size)) return 0;
		return Board[at.x, at.z, E.MONSTER_LAYER];
	}
	public Monster MonsterAt(Point at){
		int id = MonsterIDAt(at);
		if(id == 0){
			//Debug.Log("Illegal Board Search: Empty Cell");
			//throw new GameboardException();
			return null;
		}else{
			return MonstersOnBoard[id];
		}
	}
	public List<Monster> MonstersAt(Point Center, List<Point> Shape){
		List<Monster> Monsters = new List<Monster>();
		//foreach (Point p in Shape)
		//{
		//	Debug.Log(p);
		//}
		foreach (Point ShapePoint in Shape){
			Point NewPoint = Center + ShapePoint;
			//Debug.Log("center " + Center.x + " " + Center.z + " plus " + ShapePoint.x + " " + ShapePoint.z );
			Monster MonsterAtPoint = MonsterAt(NewPoint);
			if(MonsterAtPoint != null){
				Monsters.Add(MonsterAtPoint);
			}	
		}
		
		return Monsters;
	}
	public Point MonsterPosition(Monster mon){
		for (int i = 0; i < size.x; i++){
			for (int j = 0; j < size.z; j++){
				if(mon.ID == Board[i, j, E.MONSTER_LAYER]) return new Point(i, j);
			}
		}
		return null;
	}
	//TODO


	public void SetGround(int Value, Point Point){
		Set(Value, Point, E.GROUND_LAYER);
	}
	private void Set(int Value, Point p, int layer){
		if(Value > 0 && layer == E.MONSTER_LAYER){ //moving someone
			MonstersOnBoard[Value].MonsterPoint = p;
		}
		Board[(int)p.x, (int)p.z, layer] = Value;
	}
	
	public int At(int x, int z, int layer = 0){
		return Board[x, z, layer];
	}
	public int At(Point p, int layer = 0){
		return Board[(int)p.x, (int)p.z, layer];
	}

	public int[,] GetLayer(int level){
		int[,] layer = new int[size.x, size.z];

		//Copy matrix
		for (int i = 0; i < size.x; i++){
			for (int j = 0; j < size.z; j++){
				layer[i, j] = Board[i, j, level];
			}
		}

		return layer;
	}
	public Map WalkableMap(){
		int[,] IntMap = GetLayer(E.GROUND_LAYER);

		foreach (Monster Mon in MonstersOnBoard.Values)
		{
			IntMap[Mon.MonsterPoint.x, Mon.MonsterPoint.z] = 1; //TODO:
		}

		Map ReturnMap = new Map(IntMap);
		WriteMaster.WriteUp("WalkableMap", IntMap);

		return ReturnMap;
	}


	/*
	public string toString(){
		string s = "";
		for (int k = 0; k < 3; k++){
			s += " [";
			for (int j = 0; j < size.z; j++){
				for (int i = 0; i < size.x; i++){
					s += " " + Board[i, j, k];
				}
			}
			s += " ] \n";
		}
		
		return s;
	}
	public int GetAt(Point p){
		for (int c = 0; c < monPieces.transform.childCount; c++){
			Vector3 pos = monPieces.transform.GetChild(c).position;
			if(pos.x == p.x && pos.z == p.z){
				return monPieces.transform.GetChild(c).gameObject;
			}			
		}
		return null;
	}*/

	


}