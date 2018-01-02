using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap {

	int[,,] Map;
	int LayerAmount = 8;
	Point size;

	int WalkableLayer = 0, MonsterLayer = 1;

	public InfluenceMap(Point BoardSize){

		size = BoardSize;
		Map = new int[size.x, size.z, LayerAmount];

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

	public void ConsiderMonsters(List<Monster> MonsterList){
		
	}

}
