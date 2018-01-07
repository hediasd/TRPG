using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap {

	float[,,] Map;
	int LayerAmount = 8;
	Point size;
	Monster PivotMonster;

	public InfluenceMap(Monster Mon, Point BoardSize){

		PivotMonster = Mon;
		size = BoardSize;
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
					Map[i, j, E.MONSTER_LAYER] += Mathf.Pow(0.96f, MonsterDistance);
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
