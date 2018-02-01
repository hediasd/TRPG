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


	PieceSpell ChooseSpell(Monster ThinkingMonster, List<Point> Paths, InfluenceMap InfluenceMap){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		int[,] GroundMap = Gameboard.GetLayer(E.GROUND_LAYER);
		int MyTeam = ThinkingMonster.Team, BestDamage = 0;
		Spell ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		//Foreach castable spell available
		foreach (Spell CandidateSpell in ThinkingMonster.Spells_)
		{
			//Evaluate result when casting at any available point
			List<LinkedPoint> BSC = Algorithms.BlurredSpellCastRange(Here, GroundMap, CandidateSpell, ThinkingMonster.MovementPoints());
			foreach (LinkedPoint BlurredPoint in BSC)
			{
				//Point p = (Point) BlurredPoint;
				if(!Paths.Contains((Point) BlurredPoint.Parents[0])){
					continue;
				}
				List<Damage> DamageSimulations = Gameboard.SimulateSpellPerformance(ThinkingMonster, CandidateSpell, BlurredPoint);
				
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
					CastFrom = BlurredPoint.Parents[0];
					CastTo = BlurredPoint;
				}

			}

		}

		if(ChosenSpell == null) return null;
		return new PieceSpell(ThinkingMonster, ChosenSpell, CastFrom, CastTo);

	}

	PieceMove PathMaker(Monster ThinkingMonster, Point Goal, Map WalkableMap){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		//BattleMaster.Log("["+ThinkingMonster.Name+"] aims for ["+NearestMonster.Name+"]");

		Point BestOption = Here;
		int MinimumRecordedDistance = 9999;
		List<Point> UnnocupiedReachablePoints = Algorithms.ReachableUnnocupiedCells(Here, 2, WalkableMap);//Unnocupied
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

		List<Point> PointPath = Algorithms.Path(Here, BestOption, WalkableMap);

		return new PieceMove(ThinkingMonster, Here, BestOption, PointPath);

	}

	public Deque<BoardAction> Thinking(Monster ThinkingMonster){

		Deque<BoardAction> Actions = new Deque<BoardAction>();

		//Create a map of influence and feed it with board info
		//Consider where you may walk or not
		//Consider the influence of enemies	
		InfluenceMap InfluenceMap = new InfluenceMap(ThinkingMonster, Gameboard);
		InfluenceMap.ConsiderWalkables(Gameboard.GetLayer(E.GROUND_LAYER));
		InfluenceMap.ConsiderMonsters(Allies, Enemies);
		InfluenceMap.ConsiderCastableSpells(ThinkingMonster, Allies, Enemies, Gameboard.GetLayer(E.GROUND_LAYER));

		Map WalkableMap = Gameboard.WalkableMap();
		Dictionary<Point, List<Point>> Paths = Algorithms.PathTracer(ThinkingMonster.MonsterPoint, ThinkingMonster.MovementPoints(), WalkableMap);
		PieceSpell ChosenSpell = ChooseSpell(ThinkingMonster, new List<Point>(Paths.Keys), InfluenceMap);
		
		if(ChosenSpell != null){
			//PieceMove ChosenMovementPath = PathMaker(ThinkingMonster, ChosenSpell.to, WalkableMap);
			PieceMove ChosenMovementPath = new PieceMove(ThinkingMonster, ThinkingMonster.MonsterPoint, ChosenSpell.fr, Paths[ChosenSpell.fr]);
			Actions.Enqueue(ChosenMovementPath);
			Actions.Enqueue(ChosenSpell);
		}else{
			List<Point> ReachablePoints = Algorithms.ReachableUnnocupiedCells(ThinkingMonster.MonsterPoint, ThinkingMonster.MovementPoints(), WalkableMap);
			Debug.Log(" "+ReachablePoints.Count+ " "+ThinkingMonster.MovementPoints());
			ReachablePoints.Sort((a,b) => InfluenceMap[b].CompareTo(InfluenceMap[a]));
			//PieceMove ChosenMovementPathh = PathMaker(ThinkingMonster, ReachablePoints[0], WalkableMap);
			//TODO: What if PM = 0
			if(ReachablePoints.Count > 0){
				PieceMove ChosenMovementPath = new PieceMove(ThinkingMonster, ThinkingMonster.MonsterPoint, ReachablePoints[0], Paths[ReachablePoints[0]]);
				//Debug.Log(ChosenMovementPath);
				Actions.Enqueue(ChosenMovementPath);
			}
		}

		

		WriteMaster.WriteUp("InfluenceMap", InfluenceMap.ToString());

		//}
		//Actions.Enqueue(new PieceMove(OnTurn, here));
		//PiecesMaster.WalkTo(OnTurn, next);//next

		return Actions;

	}




}
