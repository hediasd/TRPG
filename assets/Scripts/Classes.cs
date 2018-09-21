using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Classes : MonoBehaviour {
}

public class GameState {

	public List<GameObject> Windows;
	public int State, Selecter;

	public GameState(int st, int se = 0){
		Windows = new List<GameObject>();
		State = st;
		Selecter = se;
	}

}

public class DamageSegment {
	public int Element, Value;
	public DamageSegment(int d, int e){
		Element = e;
		Value = d;
	}
}
	//Gameboard Layers
public class LAYER {
	public const int GROUND = 0, MONSTER = 1; 
}
	//Monster Stats
public class STAT {
	public const int HPA = 0, HPM = 1, POW = 2, MGT = 3, END = 4, RES = 5, LUK = 6, SPD = 7, IDK2 = 8, MOV = 9;
}

public class GAMESTATE {
	public const int NONE = 0, ENEMY_TURN = 10, BATTLE_MENU = 11, MOVE = 12, ATTACK = 13, SPELL = 14, ITEM = 15;
}

public class TARGET {
	public const int SELF = 100, ALLIES = 101, ENEMIES = 102, BOTH = 103, ALL = 104;
}

public class LOCK {
	public const int TURN_WHEEL = 900, POP1 = 901, POP2 = 902, POP3 = 903, FLUSH = 904, WAIT = 910;
}



public class E {
/* USED AS ARRAY POSITIONS */

/* GENERAL */
//Menu, Turn and UI
	
	public const int CHOOSER = 30, ARROW = 31, ARROW_UPDOWN = 32;

//Triggers and Events
	public const int ON_TURN_START = 200, ON_TURN_END = 201;
//Animations
	public const int SIMPLESINGLE = 700, WAVES = 701;
//Properties
	public const int HEAL = 800, DAMAGE = 801, POISON = 802, UNHEALABLE = 803,
	TERRAINSPAWN = 810, TERRAINREMOVAL = 811;

}

public class Lock {
	public List<int> code = new List<int>();
	public Lock(int c, int d = -1, int e = -1){
		code.Add(c);	
		if(d != -1) code.Add(d);
		if(e != -1)	code.Add(e);
	}
}

[System.Serializable]
public class DataObject {
	public string Name;
}

/*
public class Property {
	public int a, b, c, d;
	public Property(int ai, int bi = 0, int ci = 0, int di = 0){
		a = ai;
		b = bi;
		c = ci;
		d = di;
	}

	public override string ToString(){
		return a + " " + b + " " + c + " " + d;
	}
} */

public class TextBubble : MonoBehaviour {

	public float timerMax = 2.0f;
	public char[] words;
	public string textPileup;
	public TextMesh textMesh;
	public int max = 25;

}
