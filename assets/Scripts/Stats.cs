using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

	int[,] InnerStats;
	const int AbsoluteValue = 0, BattleStartValue = 1, BattleActualValue = 2;

	public Stats(int[] Values){
		InnerStats = new int[10, 3]; 

		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				InnerStats[i, j] = Values[i];
			}
		}
	}

	public int this[int n]{
        get
        {
			return InnerStats[n, BattleActualValue];
        }
    }

	public int HPA(){
		return InnerStats[E.HPA, BattleActualValue];
	}
	public int HPM(){
		return InnerStats[E.HPA, BattleStartValue];
	}

	public void Increase(int Index, int Amount){
		InnerStats[Index, BattleActualValue] += Amount;
	}
	public void Decrease(int Index, int Amount){
		InnerStats[Index, BattleActualValue] -= Amount;
	}
	public void ResetValue(int Index){
		InnerStats[Index, BattleActualValue] = InnerStats[Index, BattleStartValue];
	}

	/*
	public bool Decrease(int Amount){
		BattleActualValue = ((BattleActualValue - Amount) < 0) ? 0 : BattleActualValue - Amount;
		return true;
	}*/

	public Stats Copy(){
		int[] CopiedInts = new int[10];
		for (int i = 0; i < 10; i++)
		{
				CopiedInts[i] = InnerStats[i, 1];
		}
		return new Stats(CopiedInts);
	}

	
	
}
