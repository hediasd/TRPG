using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

	int[, ] InnerStats;

	// Absolute = natural stats, start?, actual is the value right now, after equips buffs debuffs
	const int BattleAbsoluteValue = 0,
		BattleStartValue = 1,
		BattleCurrentValue = 2;

	public Stats (int[] Values) {
		InnerStats = new int[10, 3];

		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 3; j++) {
				InnerStats[i, j] = Values[i];
			}
		}
	}

	public int this [int n] {
		get {
			return InnerStats[n, BattleCurrentValue];
		}
	}

	public int HPA () {
		return InnerStats[STAT.HPA, BattleCurrentValue];
	}
	public int HPM () {
		return InnerStats[STAT.HPA, BattleStartValue];
	}

	public int AbsoluteValue (int Index) {
		return InnerStats[Index, BattleAbsoluteValue];
	}
	public int StartValue (int Index) {
		return InnerStats[Index, BattleAbsoluteValue];
	}
	public int CurrentValue (int Index) {
		return InnerStats[Index, BattleAbsoluteValue];
	}

	public void Increase (int Index, int Amount) {
		InnerStats[Index, BattleCurrentValue] += Amount;
	}
	public void Decrease (int Index, int Amount) {
		InnerStats[Index, BattleCurrentValue] -= Amount;
	}
	public void ResetValue (int Index) {
		InnerStats[Index, BattleCurrentValue] = InnerStats[Index, BattleStartValue];
	}

	/*
	public bool Decrease(int Amount){
		BattleActualValue = ((BattleActualValue - Amount) < 0) ? 0 : BattleActualValue - Amount;
		return true;
	}*/

	public Stats Copy () {
		int[] CopiedInts = new int[10];
		for (int i = 0; i < 10; i++) {
			CopiedInts[i] = InnerStats[i, 1];
		}
		return new Stats (CopiedInts);
	}

}