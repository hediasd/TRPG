using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap {

	float[,,] Map;
	GameboardMaster GameboardMaster;
	int LayerAmount = 8;
	Point size;
	Monster PivotMonster;

	public InfluenceMap(Monster Mon, GameboardMaster Gameboard){

		PivotMonster = Mon;
		GameboardMaster = Gameboard;
		size = Gameboard.size;
		Map = new float[size.x, size.z, LayerAmount];

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.z; j++)
			{
				for (int k = 0; k < LayerAmount; k++) // TODO: AMOUNT OF LAYERS ?
				{
					Map[i, j, k] = 1;
				}
			}
		}

	}

	public float this[Point p]
    {
        get
        {
			float Sum = 1;
			for (int i = 0; i < LayerAmount; i++)
			{
				Sum *= Map[p.x, p.z, i];
			}
            return Sum;
        }
    }

	private class ActivityPoint {
		float DamageDealt, LifeRestored, EnemiesKilled,
			AlteredStates, TerrainChanges,
			ConsumedMovementPoints,	ConsumedManaPoints,	ConsumedActionPoints;
		List<BoardAction> Activities;
	}

	public void ConsiderWalkables(int [,] Walkable){

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.z; j++)
			{
				Map[i, j, E.GROUND_LAYER] = (Walkable[i, j] == 0 ? 1 : 0);				
			}
		}

	}

	public void ConsiderMonsters(List<Monster> Allies, List<Monster> Enemies){

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.z; j++)
			{
				foreach (Monster EnemyMonster in Enemies)
				{
					int MonsterDistance = Point.Distance(EnemyMonster.MonsterPoint, new Point(i, j));
					if(Map[i, j, E.MONSTER_LAYER] == 1.0f){
						Map[i, j, E.MONSTER_LAYER] += 1.0f;
					}else{
						Map[i, j, E.MONSTER_LAYER] = Map[i, j, E.MONSTER_LAYER] * 1.08f * Mathf.Pow(0.94f, MonsterDistance);
					}
				}
			}
		}

		// No chance to move to an occupied cell
		foreach (Monster AllyMonster in Allies)
		{
			Map[AllyMonster.MonsterPoint.x, AllyMonster.MonsterPoint.z, E.MONSTER_LAYER] = -1;			
		}
		foreach (Monster EnemyMonster in Enemies)
		{
			Map[EnemyMonster.MonsterPoint.x, EnemyMonster.MonsterPoint.z, E.MONSTER_LAYER] = -1;
		}

	}

	public void ConsiderCastableSpells(Monster ThinkingMonster, List<Monster> Allies, List<Monster> Enemies, int[,] Walkable){

		Point Here = new Point(ThinkingMonster.MonsterPoint);
		int MyTeam = ThinkingMonster.Team, BestDamage = 0;
		Spell ChosenSpell = null;
		Point CastFrom = null, CastTo = null;

		//Foreach castable spell available
		foreach (Spell CandidateSpell in ThinkingMonster.Spells_)
		{
			//Evaluate result when casting at any available point
			List<LinkedPoint> BSC = Algorithms.BlurredSpellCastRange(Here, Walkable, CandidateSpell, 2);
			foreach (LinkedPoint BlurredPoint in BSC)
			{

				List<Damage> DamageSimulations = GameboardMaster.SimulateSpellPerformance(ThinkingMonster, CandidateSpell, BlurredPoint);
				
				// Enemy damage dealt
				//int A1 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team, ExceptTeam: true);
				// Friendly fire
				//int A2 = TeamDamageDealt(DamageSimulations, ThinkingMonster.Team);
				// Killed enemies
				int B1 = 0;
				// Killed allies;
				int B2;
				




				if(B1 > BestDamage){
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

	public override string ToString(){
		string map = "";
		for (int i = size.z-1; i >= 0 ; i--)
		{
			for (int j = 0; j < size.x; j++)
			{
				string s;
				if(this[new Point(j, i)] < 0){
					s = "XXX";
				}else{
					s = this[new Point(j, i)].ToString("0.0");
					if(s.Equals("0.0")) s = "___";
				}
				map += s;
				map += " ";
			}
			map += "\n";
		}
		return map;
	}

}
