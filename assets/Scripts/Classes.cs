using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Classes : MonoBehaviour {
}

public class GameState {

	public List<GameObject> windows;
	public int state, selecter;

	public GameState(int st, int se = 0){
		windows = new List<GameObject>();
		state = st;
		selecter = se;
	}

}

[System.Serializable]
public class DamageSegment {
	public int Element, Value;
	public DamageSegment(int e, int d){
		Element = e;
		Value = d;
	}
}

public class E {
/* USED AS ARRAY POSITIONS */
//Monster Stats
	public const int HPA = 0, HPM = 1, POW = 2, MGT = 3, END = 4, RES = 5, LUK = 6, SPD = 7, MOV = 8;
//Gameboard Layers
	public const int GROUND_LAYER = 0, MONSTER_LAYER = 1; 
/* GENERAL */
//Elements
	public const int NONE = 0, DARKNESS = 1, EARTH = 2, FIRE = 3, ICE = 4, LIGHT = 5, STEEL = 6, THUNDER = 7, WATER = 8, WIND = 9, WOOD = 10;
//Menu, Turn and UI
	public const int ENEMY_TURN = 10, BATTLE_MENU = 11, MOVE = 12, ATTACK = 13, SPELL = 14, ITEM = 15;
	public const int CHOOSER = 30, ARROW = 31, ARROW_UPDOWN = 32;
//Geometry
	public const int SQUARE = 110, CONE = 111, CIRCLE = 112, LINE = 113, TRILINE = 114, HORIZONTALLINE = 115, VERTICALLINE = 116, CROSS = 117;
	public const int SELF = 100, ALLIES = 101, ENEMIES = 102, BOTH = 103, ALL = 104;
//Triggers and Events
	public const int ON_TURN_START = 200, ON_TURN_END = 201;
//Animations
	public const int SIMPLESINGLE = 700, WAVES = 701;
//Properties
	public const int HEAL = 800, DAMAGE = 801, POISON = 802, UNHEALABLE = 803,
	TERRAINSPAWN = 810, TERRAINREMOVAL = 811;
//States
	public const int TURN_WHEEL = 900, POP1 = 901, POP2 = 902, POP3 = 903, FLUSH = 904, WAIT = 910;

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



public class Status : DataObject {


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

public class BoardAction {
}

public class GlobalAction : BoardAction {
	public int Trigger;

	public GlobalAction(int t){
		Trigger = t;
	}
}

public class PieceMove : BoardAction {
	public GameObject who;
	public List<Point> pointpath;
	public Point from, to;
	public Monster mon;

	public PieceMove(Monster m, Point fr, Point gt, List<Point> PointPath){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
		from = fr;
		to = gt;
		pointpath = PointPath;
	}
	public override string ToString(){
		string s = "";
		foreach (Point p in pointpath)
		{
			s += p.ToString() + " ";
		}
		return s;
	}
}
public class PieceSpell : BoardAction {
	public GameObject who;
	public Spell sp;
	public Point fr, to;
	public Monster mon;
	public PieceSpell(Monster m, Spell ss, Point gf, Point gt){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
		sp = ss;
		fr = gf;
		to = gt;
	}
}

public class TextBubble : MonoBehaviour {

	public float timerMax = 2.0f;
	public char[] words;
	public string textPileup;
	public TextMesh textMesh;
	public int max = 25;

}
