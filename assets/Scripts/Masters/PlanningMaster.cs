using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningMaster : MonoBehaviour {
	
	[HideInInspector]
	public GameboardMaster Gameboard;
	public Monster OnTurn;
	public Point OnTurnPoint;
	public List<Monster> Allies, Enemies;

	public void Feed(GameboardMaster board, Monster on, List<Monster> a, List<Monster> e) {
		Gameboard = board;
		OnTurn = on;
		OnTurnPoint = new Point(on);
		Allies = a;
		Enemies = e;
	}

	class SpellCandidate{

		int EnemyDamage;
		int FriendFire;

	}
	
	int TeamDamageDealt(List<Damage> Damages, int Team, bool ExceptTeam = false){
		int Total = 0;
		foreach (Damage d in Damages)
		{
			int TargetsTeam = d.TargetMonster.Team;
			// if (all enemy teams) or (only my team)
			if((ExceptTeam && TargetsTeam != Team) || (!ExceptTeam && TargetsTeam == Team)){
					Total += d.FinalDamage;
			}		
		}
		return Total;
	}

	PieceSpell ChooseSpell(Monster ThinkingMonster, InfluenceMap InfluenceMap){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		int[,] GroundMap = Gameboard.GetLayer(E.GROUND_LAYER);
		int MyTeam = ThinkingMonster.Team, BestDamage = 0;
		Spell ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		//Foreach castable spell available
		foreach (Spell CandidateSpell in ThinkingMonster.Spells_)
		{
			//Evaluate result when casting at any available point
			List<LinkedPoint> BSC = Algorithms.BlurredSpellCastRange(Here, GroundMap, CandidateSpell, 2);
			foreach (LinkedPoint BlurredPoint in BSC)
			{

				List<Damage> DamageSimulations = SimulateSpellPerformance(ThinkingMonster, CandidateSpell, BlurredPoint);
				
				// Enemy damage dealt
				int A1 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team, ExceptTeam: true);
				// Friendly fire
				int A2 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team);
				// Killed enemies
				int B1;
				// Killed allies;
				int B2;
				




				if(A1 > BestDamage){
					//Debug.Log(ThisTotalDamage);
					BestDamage = A1;
					ChosenSpell = CandidateSpell;
					//TODO: CAST FROM
					CastTo = BlurredPoint;
				}

			}

		}

		if(ChosenSpell == null) return null;
		return new PieceSpell(ThinkingMonster, ChosenSpell, CastFrom, CastTo);

	}

	PieceMove PathMaker(Monster ThinkingMonster, Point Goal, int[,] Map){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		//BattleMaster.Log("["+ThinkingMonster.Name+"] aims for ["+NearestMonster.Name+"]");

		Point BestOption = Here;
		int MinimumRecordedDistance = 9999;
		List<Point> UnnocupiedReachablePoints = Algorithms.ReachableUnnocupiedCells(Here, 2, Map);//Unnocupied
		//OverlaysMaster.CleanUp();
		//OverlaysMaster.SpawnSpellCells(reachables, 1);
		foreach (Point ReachablePoint in UnnocupiedReachablePoints)
		{
			if(Gameboard.MonsterAt(ReachablePoint) == null){
				if(Point.Distance(ReachablePoint, Goal) < MinimumRecordedDistance){
					MinimumRecordedDistance = Point.Distance(ReachablePoint, Goal);
					BestOption = ReachablePoint;
				}
				else if(Point.Distance(ReachablePoint, Goal) == MinimumRecordedDistance && Point.Distance(Here, ReachablePoint) < Point.Distance(Here, Goal)){
					BestOption = ReachablePoint;
				}
			}
		}

		return new PieceMove(ThinkingMonster, Here, BestOption);

	}

	public Deque<PieceAction> Thinking(Monster ThinkingMonster){

		Deque<PieceAction> Actions = new Deque<PieceAction>();

		//Create a map of influence and feed it with board info
		//Consider where you may walk or not
		//Consider the influence of enemies	
		InfluenceMap InfluenceMap = new InfluenceMap(ThinkingMonster, Gameboard.size);
		InfluenceMap.ConsiderWalkables(Gameboard.GetLayer(E.GROUND_LAYER));
		InfluenceMap.ConsiderMonsters(Allies, Enemies);

		int[,] Map = Gameboard.WalkableMap();
		
		PieceSpell ChosenSpell = ChooseSpell(ThinkingMonster, InfluenceMap);
		
		if(ChosenSpell != null){
			Debug.Log("chosen sp");
			PieceMove ChosenMovementPath = PathMaker(ThinkingMonster, ChosenSpell.to, Map);
			Actions.Enqueue(ChosenMovementPath);
			Actions.Enqueue(ChosenSpell);
		}else{
			Debug.Log("not chosen sp");
			List<Point> ReachablePoints = Algorithms.ReachableUnnocupiedCells(ThinkingMonster.MonsterPoint, 2, Map);
			ReachablePoints.Sort((a,b) => InfluenceMap[b].CompareTo(InfluenceMap[a]));
			PieceMove ChosenMovementPath = PathMaker(ThinkingMonster, ReachablePoints[0], Map);
			Debug.Log("from " + ThinkingMonster.MonsterPoint + " to " + ReachablePoints[0]);
			Actions.Enqueue(ChosenMovementPath);
		}

		WriteMaster.WriteUp("InfluenceMap", InfluenceMap.ToString());

		//}
		//Actions.Enqueue(new PieceMove(OnTurn, here));
		//PiecesMaster.WalkTo(OnTurn, next);//next

		return Actions;

	}


	public List<Damage> SimulateSpellPerformance(Monster Caster, Spell SimulatedSpell, Point TargetedCell){
		List<Damage> SimulationResult = new List<Damage>();
		List<Monster> TargetedMonsters = new List<Monster>();

		if(SimulatedSpell.DamageSegments.Count == 0){
			
		}else{
			//if(SimulatedSpell.Radius == 1){ //single target
			//TODO:	Debug.Log("do the radius 1");
			//}else{ //radius or multiple target
				//know shape = worth for terrains
				List<Point> SpellShape = SimulatedSpell.EffectShapePoints(Caster.MonsterPoint, TargetedCell);
				//know targets = important for choices
				TargetedMonsters.AddRange(Gameboard.MonstersAt(TargetedCell, SpellShape));
				//in the future, move this to outer layer perhaps
			//}
			return SimulatedSpell.DamageInstances(Caster, TargetedMonsters);
		}

		return SimulationResult;
	}
	

}
