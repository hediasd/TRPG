using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
	public GameObject CasterGameObject;
	public Spell CastedSpell;
	public Point CastedFrom, CastedTo;
	public Monster CasterMonster;
	public PieceSpell(Monster m, Spell ss, Point gf, Point gt){
		CasterGameObject = PiecesMaster.MonsterGameObject(m);
		CasterMonster = m;
		CastedSpell = ss;
		CastedFrom = gf;
		CastedTo = gt;
	}
}

public class PieceText : BoardAction {
	public GameObject who;
	public Monster mon;
	public string Text;

	public PieceText(Monster m, string words){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
		Text = words;
	}
}

public class PieceKill : BoardAction {
	public GameObject who;
	public Monster mon;
	
	public PieceKill(Monster m){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
	}
}
