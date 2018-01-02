using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningMaster : MonoBehaviour {
	
	[HideInInspector]
	public GameboardMaster GameBoard;
	public Monster OnTurn;
	public Point onTurnPoint;
	public List<Monster> Allies, Enemies;

	public void Feed(GameboardMaster board, Monster on, List<Monster> a, List<Monster> e) {
		GameBoard = board;
		OnTurn = on;
		onTurnPoint = new Point(on);
		Allies = a;
		Enemies = e;
	}

	class SpellCandidate{

		int EnemyDamage;
		int FriendFire;

	}

	float[,] InfluenceMap(){
		float[,] FinalMap = new float[GameBoard.size.x, GameBoard.size.z];

		float MaxDepth = 5;

		foreach (Monster mon in GameBoard.MonstersOnBoard.Values)
		{
			if(mon.Team == 0){
				List<Point> MonInfluences = Environment.GetReachableUnnocupiedCells(mon.MonsterPoint, 5);
				foreach (Point InfluencePoint in MonInfluences)
				{
					FinalMap[InfluencePoint.x, InfluencePoint.z] += (MaxDepth - InfluencePoint.Depth) * 0.12f;
					//FinalMap[InfluencePoint.x, InfluencePoint.z] += 1 * (Mathf.Pow(0.6f, InfluencePoint.Depth));
					//FinalMap[InfluencePoint.x, InfluencePoint.z] += MaxInfluence - (MaxInfluence * (InfluencePoint.Depth / MaxDepth) * (InfluencePoint.Depth / MaxDepth));
				}
			}
		}
		foreach (Monster mon in GameBoard.MonstersOnBoard.Values)
		{
			FinalMap[mon.MonsterPoint.x, mon.MonsterPoint.z] = 0;
		}
	
		return FinalMap;
	}

	List<LinkedPoint> BlurredSpellCastRange(Point PointPivot, Spell SimulatedSpell, int BlurAmount){
		List<LinkedPoint> BlurredCastShape = new List<LinkedPoint>();

		//CastShape with Pivot
		List<Point> CastShape = SimulatedSpell.CastShapePoints(PointPivot);
		int[,] GroundMap = GameBoard.GetLayer(E.GROUND_LAYER);
		List<Point> Circle = Shape.GetShape(E.CIRCLE, BlurAmount);
		
		foreach (Point CastShapePoint in CastShape)
		{
			foreach (Point CirclePoint in Circle)
			{
				// Blurred point from a cast point
				LinkedPoint BlurredCastShapePoint = new LinkedPoint(CastShapePoint + CirclePoint);
				// If the cell is not obstructed, go ahead
				if(BlurredCastShapePoint.WithinLimits() && GroundMap[BlurredCastShapePoint.x, BlurredCastShapePoint.z] != 1){
					// If new, add to the list and add parent shape
					// Else only add parent shape to existing point
					int Index = BlurredCastShape.IndexOf(BlurredCastShapePoint);
					if(Index >= 0){ // Exists
						BlurredCastShape[0].Parents.Add(CastShapePoint);
					}else{
						BlurredCastShapePoint.Parents.Add(CastShapePoint);
						BlurredCastShape.Add(BlurredCastShapePoint);
					}
				}
			}
		}

		return BlurredCastShape;
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

	PieceSpell ChooseSpell(Monster ThinkingMonster){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		int MyTeam = ThinkingMonster.Team;
		int BestDamage = 0;
		Spell ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		//Foreach castable spell available
		foreach (Spell CandidateSpell in ThinkingMonster.Spells_)
		{
			//Evaluate result when casting at any available point
			List<LinkedPoint> BSC = BlurredSpellCastRange(Here, CandidateSpell, 2);
			foreach (LinkedPoint BlurredPoint in BSC)
			{

				List<Damage> DamageSimulations = ThinkingMonster.SimulateSpellPerformance(CandidateSpell, BlurredPoint);
				
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

	PieceMove PathMaker(Monster ThinkingMonster, Point Goal){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		//BattleMaster.Log("["+ThinkingMonster.Name+"] aims for ["+NearestMonster.Name+"]");

		Point BestOption = Here;
		int MinimumRecordedDistance = 999;
		List<Point> UnnocupiedReachablePoints = Environment.GetReachableUnnocupiedCells(Here, 2);//Unnocupied
		//OverlaysMaster.CleanUp();
		//OverlaysMaster.SpawnSpellCells(reachables, 1);
		foreach (Point ReachablePoint in UnnocupiedReachablePoints)
		{
			if(!Environment.IsOccupied(ReachablePoint)){
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
		InfluenceMap InfluenceMap = new InfluenceMap(GameBoard.size);
		InfluenceMap.ConsiderMonsters(GameBoard.GetMonsters);
		
		//Find nearest enemy, find nearest free cell, move

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		PieceSpell ChosenSpell = ChooseSpell(ThinkingMonster);
		
		if(ChosenSpell != null){
			PieceMove ChosenMovementPath = PathMaker(ThinkingMonster, ChosenSpell.to);
			Actions.Enqueue(ChosenMovementPath);
			Actions.Enqueue(ChosenSpell);
		}else{

		}

		//}
		//Actions.Enqueue(new PieceMove(OnTurn, here));
		//PiecesMaster.WalkTo(OnTurn, next);//next

		return Actions;

	}

}
