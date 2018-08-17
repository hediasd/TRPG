﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningMaster : MonoBehaviour {

	[HideInInspector]
	public GameboardMaster GameboardMaster;
	public Gameboard Gameboard;
	public Monster OnTurn;
	public Point OnTurnPoint;
	public List<Monster> Allies, Enemies;

	public void Feed (GameboardMaster board, Monster on, List<Monster> a, List<Monster> e) {
		GameboardMaster = board;
		Gameboard = GameboardMaster.Gameboard;
		OnTurn = on;
		OnTurnPoint = new Point (on);
		Allies = a;
		Enemies = e;
	}

	class SpellCandidate {

		int EnemyDamage;
		int FriendFire;

	}

	int TargetTeamDamageDealt (List<Damage> Damages, int Team, bool ExceptGivenTeam = false) {
		int Total = 0;
		foreach (Damage d in Damages) {
			int TargetsTeam = d.TargetMonster.Team;
			// if (all enemy teams) or (only my team)
			if ((ExceptGivenTeam && TargetsTeam != Team) || (!ExceptGivenTeam && TargetsTeam == Team)) {
				Total += d.FinalDamage;
			}
		}
		return Total;
	}

	PieceSpell ChooseSpell (Monster ThinkingMonster, List<Point> ReachablePoints, InfluenceMap InfluenceMap) {

		UberDebug.LogChannel ("ID " + ThinkingMonster.ID, ThinkingMonster.Name + " choosing a spell");
		Point Here = new Point (ThinkingMonster.MonsterPoint);
		int[, ] GroundMap = Gameboard.GetLayer (LAYER.GROUND);
		int MyTeam = ThinkingMonster.Team, BestScore = 0;
		Spell ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		// For each castable spell available
		foreach (Spell CandidateSpell in ThinkingMonster.Spells_) {
			// Evaluate the result score when casting from+to any available point
			List<LinkedPoint> BSC = Algorithms.BlurredSpellCastRange (Here, GroundMap, CandidateSpell, ThinkingMonster.AvailableMovementPoints);

			foreach (LinkedPoint BlurredPoint in BSC) {

				// Checks if the blurred point is reachable
				if (!ReachablePoints.Contains ((Point) BlurredPoint.Parents[0])) {
					continue;
				}

				int SpellScore = EvaluateSpellScore (ThinkingMonster, CandidateSpell, BlurredPoint);

				if (SpellScore > BestScore) {
					//Debug.Log(ThisTotalDamage);
					BestScore = SpellScore;
					ChosenSpell = CandidateSpell;
					CastFrom = BlurredPoint.Parents[0];
					CastTo = BlurredPoint;
				}

			}

		}

		if (ChosenSpell == null) {
			UberDebug.LogChannel ("ID " + ThinkingMonster.ID, ThinkingMonster.Name + " chooses no spell");
			return null;
		}

		UberDebug.LogChannel ("ID " + ThinkingMonster.ID, ThinkingMonster.Name + " chooses spell " + ChosenSpell.Name);
		return new PieceSpell (ThinkingMonster, ChosenSpell, CastFrom, CastTo);

	}

	int EvaluateSpellScore (Monster ThinkingMonster, Spell CandidateSpell, Point CastFrom) {

		int Score = 0;

		List<Damage> DamageSimulations = Gameboard.SpellPerformance (ThinkingMonster, CandidateSpell, CastFrom);

		// Enemy damage dealt
		int A1 = TargetTeamDamageDealt (DamageSimulations, ThinkingMonster.Team, ExceptGivenTeam : true);
		// Friendly fire
		int A2 = TargetTeamDamageDealt (DamageSimulations, ThinkingMonster.Team);

		// Killed enemies
		int B1;
		// Killed allies;
		int B2;

		return (A1 - A2);

	}

	PieceMove PathMaker (Monster ThinkingMonster, Point Goal, Map WalkableMap) {

		Point Here = new Point (ThinkingMonster.MonsterPoint);
		//BattleMaster.Log("["+ThinkingMonster.Name+"] aims for ["+NearestMonster.Name+"]");

		Point BestOption = Here;
		int MinimumRecordedDistance = 9999;
		List<Point> UnnocupiedReachablePoints = Algorithms.ReachableUnnocupiedCells (Here, 2, WalkableMap); //Unnocupied
		//OverlaysMaster.CleanUp();
		//OverlaysMaster.SpawnSpellCells(reachables, 1);
		foreach (Point ReachablePoint in UnnocupiedReachablePoints) {
			if (Gameboard.MonsterAt (ReachablePoint) == null) {
				if (Point.Distance (ReachablePoint, Goal) < MinimumRecordedDistance) {
					MinimumRecordedDistance = Point.Distance (ReachablePoint, Goal);
					BestOption = ReachablePoint;
				} else if (Point.Distance (ReachablePoint, Goal) == MinimumRecordedDistance && Point.Distance (Here, ReachablePoint) < Point.Distance (Here, Goal)) {
					BestOption = ReachablePoint;
				}
			}
		}

		List<Point> PointPath = Algorithms.Path (Here, BestOption, WalkableMap);

		return new PieceMove (ThinkingMonster, Here, BestOption, PointPath);

	}

	public Deque<BoardAction> Thinking (Monster ThinkingMonster) {

		Deque<BoardAction> Actions = new Deque<BoardAction> ();

		//Create a map of influence and feed it with board info
		//Consider where you may walk or not
		//Consider the influence of enemies	

		// TODO: Where to walk considers weighted medium range of spells

		InfluenceMap InfluenceMap = new InfluenceMap (ThinkingMonster, Gameboard);
		InfluenceMap.ConsiderWalkableSpaces (Gameboard.GetLayer (LAYER.GROUND));
		InfluenceMap.ConsiderMonsters (Allies, Enemies); // Make customizable behaviors, liking get near enemies or allies for example
		//InfluenceMap.ConsiderCastableSpells(ThinkingMonster, Allies, Enemies, Gameboard.GetLayer(LAYER.GROUND));

		Map WalkableMap = Gameboard.WalkableMap ();
		Dictionary<Point, List<Point>> Paths = Algorithms.PathTracer (ThinkingMonster.MonsterPoint, ThinkingMonster.AvailableMovementPoints, WalkableMap);
		PieceSpell ChosenSpell = ChooseSpell (ThinkingMonster, new List<Point> (Paths.Keys), InfluenceMap);

		if (ChosenSpell != null) {

			//PieceMove ChosenMovementPath = PathMaker(ThinkingMonster, ChosenSpell.to, WalkableMap);
			PieceMove ChosenMovementPath = new PieceMove (ThinkingMonster, ThinkingMonster.MonsterPoint, ChosenSpell.CastedFrom, Paths[ChosenSpell.CastedFrom]);
			Actions.Enqueue (ChosenMovementPath);
			Actions.Enqueue (ChosenSpell);

		} else {

			List<Point> ReachablePoints = Algorithms.ReachableUnnocupiedCells (ThinkingMonster.MonsterPoint, ThinkingMonster.AvailableMovementPoints, WalkableMap);
			//" "+ReachablePoints.Count+ " "+ThinkingMonster.MovementPoints());
			ReachablePoints.Sort ((a, b) => InfluenceMap[b].CompareTo (InfluenceMap[a]));
			//PieceMove ChosenMovementPathh = PathMaker(ThinkingMonster, ReachablePoints[0], WalkableMap);

			//TODO: What if PM = 0
			if (ReachablePoints.Count > 0) {
				PieceMove ChosenMovementPath = new PieceMove (ThinkingMonster, ThinkingMonster.MonsterPoint, ReachablePoints[0], Paths[ReachablePoints[0]]);
				////////Debug.Log(ThinkingMonster.Name + " to " + ChosenMovementPath.to);
				//Debug.Log(ChosenMovementPath);
				Actions.Enqueue (ChosenMovementPath);
			}

		}

		WriteMaster.WriteUp ("InfluenceMap", InfluenceMap.ToString ());

		//Actions.Enqueue(new PieceMove(OnTurn, here));
		//PiecesMaster.WalkTo(OnTurn, next);//next

		return Actions;

	}

}