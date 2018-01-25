using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

	int[,] InnerMap;
	public readonly Point size;

	public Map(Point p){
		size = new Point(p);
		InnerMap = new int[size.x, size.z];
	}
	public Map(int[,] IntMap){
		size = new Point(IntMap.GetLength(0), IntMap.GetLength(1));
		InnerMap = new int[size.x, size.z];
		
		for (int i = 0; i < IntMap.GetLength(0); i++)
		{
			for (int j = 0; j < IntMap.GetLength(1); j++)
			{
				InnerMap[i, j] = IntMap[i, j];	
			}
		}
	}

	public int this[Point p]
    {
        get
        {
			return InnerMap[p.x, p.z];
        }
		set
		{
			InnerMap[p.x, p.z] = value;
		}
    }

}
