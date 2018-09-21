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
	public MonsterInstance mon;

	public PieceMove(MonsterInstance m, Point fr, Point gt, List<Point> PointPath){
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
	public SpellEntry CastedSpell;
	public Point CastedFrom, CastedTo;
	public MonsterInstance CasterMonster;
	public PieceSpell(MonsterInstance m, SpellEntry ss, Point gf, Point gt){
		CasterGameObject = PiecesMaster.MonsterGameObject(m);
		CasterMonster = m;
		CastedSpell = ss;
		CastedFrom = gf;
		CastedTo = gt;
	}
}

public class PieceText : BoardAction {
	public GameObject who;
	public MonsterInstance mon;
	public string Text;

	public PieceText(MonsterInstance m, string words){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
		Text = words;
	}
}

public class PieceKill : BoardAction {
	public GameObject who;
	public MonsterInstance mon;
	
	public PieceKill(MonsterInstance m){
		who = PiecesMaster.MonsterGameObject(m);
		mon = m;
	}
}
