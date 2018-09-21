using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterHolder : MonoBehaviour {

	public Color MonsterColorA, MonsterColorB;
	public int Team;
	public MonsterInstance HeldMonster;

	void LateUpdate(){
		MonsterColorA = HeldMonster.PaletteA_;
		MonsterColorB = HeldMonster.PaletteB_;
		Team = HeldMonster.Team;
	}
	
}