using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap {

	float[, , ] Map;
	Gameboard Gameboard;
	int LayerAmount = 8;
	Point size;
	MonsterInstance ThinkingMonster;
	List<MonsterInstance> Allies, Enemies;

	private class LAYER {
		public const int GROUND = 0,
			MONSTER = 1;
	}

	public InfluenceMap (MonsterInstance ThinkingMonster, List<MonsterInstance> Allies, List<MonsterInstance> Enemies, Gameboard Gameboard) {

		this.ThinkingMonster = ThinkingMonster;
		this.Gameboard = Gameboard;
		size = Gameboard.size;
		Map = new float[size.x, size.z, LayerAmount];
		this.Allies = Allies;
		this.Enemies = Enemies;

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				for (int k = 0; k < LayerAmount; k++) // TODO: AMOUNT OF LAYERS ?
				{
					Map[i, j, k] = 1;
				}
			}
		}

	}

	public float this [Point p] {
		get {
			float Sum = 1;
			for (int i = 0; i < LayerAmount; i++) {
				Sum *= Map[p.x, p.z, i];
			}
			return Sum;
		}
	}

	private class ActivityPoint {
		float DamageDealt, LifeRestored, EnemiesKilled,
		AlteredStates, TerrainChanges,
		ConsumedMovementPoints, ConsumedManaPoints, ConsumedActionPoints;
		List<BoardAction> Activities;
	}

	public void ConsiderWalkableSpaces () {

		int[, ] Walkable = Gameboard.GetLayer (LAYER.GROUND);

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				// If walkable writes 1, otherwise 0
				Map[i, j, LAYER.GROUND] = (Walkable[i, j] == 0 ? 1 : 0);
			}
		}

	}

	public void ConsiderMonsters (List<MonsterInstance> Allies, List<MonsterInstance> Enemies) {

		// For every cell
		/*
		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.z; j++) {
				foreach (Monster EnemyMonster in Enemies) {
					int MonsterDistance = Point.Distance (EnemyMonster.MonsterPoint, new Point (i, j));
					if (Map[i, j, LAYER.MONSTER] == 1.0f) {
						Map[i, j, LAYER.MONSTER] += 1.0f;
					} else {
						Map[i, j, LAYER.MONSTER] = Map[i, j, LAYER.MONSTER] * 1.08f * Mathf.Pow (0.94f, MonsterDistance);
					}
				}
			}
		}
		*/

		// No chance to move to an occupied cell
		foreach (MonsterInstance AllyMonster in Allies) {
			Map[AllyMonster.MonsterPoint.x, AllyMonster.MonsterPoint.z, LAYER.MONSTER] = -1;
		}
		foreach (MonsterInstance EnemyMonster in Enemies) {
			Map[EnemyMonster.MonsterPoint.x, EnemyMonster.MonsterPoint.z, LAYER.MONSTER] = -1;
		}

		Map[ThinkingMonster.MonsterPoint.x, ThinkingMonster.MonsterPoint.z, LAYER.MONSTER] = 0;

	}

	public void ConsiderSpellRanges () {

		List<SpellInstance> ConsideredSpells = ThinkingMonster.SpellsList;

		// For each enemy
		foreach (MonsterInstance EnemyMonster in Enemies) {
			// Checks every reverse perspective cast range
			foreach (SpellInstance CandidateSpell in ConsideredSpells) {

				List<Point> CastShapePoints = CandidateSpell.CastShapePoints (EnemyMonster.MonsterPoint);

				for (int i = 0; i < size.x; i++) {
					for (int j = 0; j < size.z; j++) {

						int MonsterDistance = Point.Distance (EnemyMonster.MonsterPoint, new Point (i, j));
						if (Map[i, j, LAYER.MONSTER] == 1.0f) {
							Map[i, j, LAYER.MONSTER] += 1.0f;
						} else {
							Map[i, j, LAYER.MONSTER] = Map[i, j, LAYER.MONSTER] * 1.08f * Mathf.Pow (0.94f, MonsterDistance);
						}
					}
				}

			}
		}

		// No chance to move to an occupied cell
		foreach (MonsterInstance AllyMonster in Allies) {
			Map[AllyMonster.MonsterPoint.x, AllyMonster.MonsterPoint.z, LAYER.MONSTER] = -1;
		}
		foreach (MonsterInstance EnemyMonster in Enemies) {
			Map[EnemyMonster.MonsterPoint.x, EnemyMonster.MonsterPoint.z, LAYER.MONSTER] = -1;
		}

	}

	public void ConsiderCastableSpells (MonsterInstance ThinkingMonster, List<MonsterInstance> Allies, List<MonsterInstance> Enemies, int[, ] Walkable) {

		Point Here = new Point (ThinkingMonster.MonsterPoint);
		int MyTeam = ThinkingMonster.Team, BestDamage = 0;
		SpellInstance ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		//Foreach castable spell available
		foreach (SpellInstance CandidateSpell in ThinkingMonster.SpellsList) {
			//Evaluate result when casting at any available point
			List<LinkedPoint> BSC = Algorithms.BlurredSpellCastRange (Here, Walkable, CandidateSpell, 2);
			foreach (LinkedPoint BlurredPoint in BSC) {

				List<Damage> DamageSimulations = Gameboard.SpellPerformance (ThinkingMonster, CandidateSpell, BlurredPoint);

				// Enemy damage dealt
				//int A1 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team, ExceptTeam: true);
				// Friendly fire
				//int A2 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team);
				// Killed enemies
				int B1 = 0;
				// Killed allies;
				int B2;

				if (B1 > BestDamage) {
					//Debug.Log(ThisTotalDamage);
					BestDamage = B1;
					ChosenSpell = CandidateSpell;
					//TODO: CAST FROM
					CastTo = BlurredPoint;
				}

			}

		}

		//	if(ChosenSpell == null) return null;
		//return new PieceSpell(ThinkingMonster, ChosenSpell, CastFrom, CastTo);

	}

	public override string ToString () {
		string map = "";
		for (int i = size.z - 1; i >= 0; i--) {
			for (int j = 0; j < size.x; j++) {
				string s;
				if (this [new Point (j, i)] < 0) {
					s = "XXX";
				} else {
					s = this [new Point (j, i)].ToString ("0.0");
					if (s.Equals ("0.0")) s = "___";
				}
				map += s;
				map += " ";
			}
			map += "\n";
		}
		return map;
	}

}