using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterHolder : MonoBehaviour {

	public Color MonsterColorA, MonsterColorB;
	public int Team;
	public MonsterInstance HeldMonster;

	void LateUpdate(){
		MonsterColorA = HeldMonster.PaletteA;
		MonsterColorB = HeldMonster.PaletteB;
		Team = HeldMonster.Team;
	}
	
}