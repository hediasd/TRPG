using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

	//[System.NonSerialized]
	public int AbsoluteValue = 1;
	public int BattleStartValue = 1;
	public int BattleActualValue = 1;

	public Stat(int st){
		if(st < 0) st = 0;
		AbsoluteValue = BattleStartValue = BattleActualValue = st;
	}
	public Stat(Stat st){
		try{
			if(st.BattleStartValue < 0) st.BattleStartValue = 0;
			AbsoluteValue = BattleStartValue = BattleActualValue = st.BattleStartValue;
		}catch(Exception){
			AbsoluteValue = BattleStartValue = BattleActualValue = 0;
		}
	}
	public bool Decrease(int Amount){
		BattleActualValue = ((BattleActualValue - Amount) < 0) ? 0 : BattleActualValue - Amount;
		return true;
	}
	
}
