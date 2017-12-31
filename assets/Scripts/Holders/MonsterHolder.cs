using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterHolder : MonoBehaviour {

	public Color MonsterColorA, MonsterColorB;
	public int Team;
	public Monster Monster;

	void LateUpdate(){
		MonsterColorA = Monster.PaletteA_;
		MonsterColorB = Monster.PaletteB_;
		Team = Monster.Team;
	}
	
}