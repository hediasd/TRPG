using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

[System.Serializable]
public class MonsterEntry : DataObject {

	public string Texture;
	[System.NonSerialized]
	public bool Started = false;
	//TODO: Started

	[System.NonSerialized]
	public List<SpellEntry> SpellsList; // = new List<Spell>();
	[System.NonSerialized]
	public List<SpellEntry> PassivesList;

	//public List<string> SpellNames;// = new List<string>();
	public List<string> StatusNames; // = new List<string>();

	public string Spells, Passives, Stats, PaletteA, PaletteB;

	[System.NonSerialized]
	public Color PaletteA_, PaletteB_;
	[System.NonSerialized]
	public Stats StatsList; //TODO: 9

	//public int HPA_, HPM_, LVL_, POW_, MGT_, END_, RES_, LUK_, SPD_, MOV_; //original
	//public int HPA, HPM, POW, MGT, END, RES, LUK, SPD, MOV;

	public MonsterEntry () {
		SpellsList = new List<SpellEntry> ();
		PassivesList = new List<SpellEntry> ();
		StatsList = new Stats (new int[10]);
	}

	public void Startup () {

	}

	public MonsterInstance Instantiate () {

		//MonsterEntry thisclone = this.MemberwiseClone ();
		//MonsterInstance mon = (MonsterInstance) ( this.MemberwiseClone());
		MonsterInstance mon = new MonsterInstance (this);

		Debug.Log (mon.Name + " " + mon.Texture);

		return mon;

	}

}