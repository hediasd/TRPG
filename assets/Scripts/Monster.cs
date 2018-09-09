using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

[System.Serializable]
public class Monster : DataObject {

	//public GameObject cellMon;
	[System.NonSerialized]
	public int ID;
	public string Texture;

	[System.NonSerialized]
	public List<Spell> SpellsList; // = new List<Spell>();
	[System.NonSerialized]
	public List<Spell> PassivesList;
	[System.NonSerialized]
	public List<Status> StatusesList; // = new List<Status>();

	//public List<string> SpellNames;// = new List<string>();
	public List<string> StatusNames; // = new List<string>();

	public string Spells, Passives, Stats, PaletteA, PaletteB;

	[System.NonSerialized]
	public Color PaletteA_, PaletteB_;
	[System.NonSerialized]
	public Stats StatsList; //TODO: 9

	//public int HPA_, HPM_, LVL_, POW_, MGT_, END_, RES_, LUK_, SPD_, MOV_; //original
	//public int HPA, HPM, POW, MGT, END, RES, LUK, SPD, MOV;

	[System.NonSerialized]
	public Point MonsterPoint;
	[System.NonSerialized]
	public bool Alive;
	[System.NonSerialized]
	public int Team, AvailableMovementPoints,
	lastDamage, turnDamage, lastTurnDamage, totalDamageTaken;
	[System.NonSerialized]
	public string lastSpellCast, lastElement;
	[System.NonSerialized]
	public List<Spell> SpellsCast; //= new List<Spell>();

	public Monster () {
		SpellsList = new List<Spell> ();
		//SpellNames = new List<string>();
		SpellsCast = new List<Spell> ();
		StatusesList = new List<Status> ();
		StatusNames = new List<string> ();
		StatsList = new Stats (new int[10]);
	}

	public Monster Copy () {

		Monster mon = (Monster) this.MemberwiseClone ();

		mon.SpellsList = new List<Spell> (SpellsList);
		mon.StatusesList = new List<Status> (StatusesList);
		//mon.SpellNames = new List<string>(SpellNames);
		mon.StatusNames = new List<string> (StatusNames);
		mon.SpellsCast = new List<Spell> ();

		mon.PaletteA_ = new Color (PaletteA_.r, PaletteA_.g, PaletteA_.b, PaletteA_.a);
		mon.PaletteB_ = new Color (PaletteB_.r, PaletteB_.g, PaletteB_.b, PaletteB_.a);

		mon.StatsList = StatsList.Copy ();
		//Array.Copy(STATS_, mon.STATS_, 9);

		mon.StatsList.Increase (STAT.MOV, 3);
		mon.AvailableMovementPoints = MovementPoints ();

		return mon;

	}

	public void AddSpell (Spell sp) {
		SpellsList.Add (sp);
		//SpellNames.Add(sp.name);
	}
	public void AddStatus (Status st) {
		StatusesList.Add (st);
		StatusNames.Add (st.Name);
	}

	public bool Die () {
		Alive = false;
		return true;
	}

	/*
		public Damage PhysicalAttack(){
			Damage d = new Damage();

			return d;
		}

		public Damage CastSpell(Spell sp){
			//Damage d = new Damage();


			return d;
		}
	*/
	public int MovementPoints () {
		return StatsList[STAT.MOV];
	}
	public void ResetMovementPoints () {
		StatsList.ResetValue (STAT.MOV);
	}
	public bool TakeDamage (Damage TakenDamage) {
		StatsList.Decrease (STAT.HPA, TakenDamage.FinalDamage);
		return true;

	}

}