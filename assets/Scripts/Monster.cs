using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

[System.Serializable]
public class Monster : DataObject {
	
	//public GameObject cellMon;
	[System.NonSerialized]
	public int ID;
	public string Texture;

	[System.NonSerialized]
	public List<Spell> Spells_;// = new List<Spell>();
	[System.NonSerialized]
	public List<Spell> Passives_;
	[System.NonSerialized]
	public List<Status> Statuses_;// = new List<Status>();
	
	//public List<string> SpellNames;// = new List<string>();
	public List<string> StatusNames;// = new List<string>();
	
	public string Spells, Passives, Stats, PaletteA, PaletteB;
	
	[System.NonSerialized]
	public Color PaletteA_, PaletteB_;
	[System.NonSerialized]
	public Stat[] Stats_;  //TODO: 9

	//public int HPA_, HPM_, LVL_, POW_, MGT_, END_, RES_, LUK_, SPD_, MOV_; //original
	//public int HPA, HPM, POW, MGT, END, RES, LUK, SPD, MOV;
	
	[System.NonSerialized]
	public Point MonsterPoint;
	[System.NonSerialized]
	public bool alive;
	[System.NonSerialized]
	public int Team, lastDamage, turnDamage, lastTurnDamage, totalDamageTaken;
	[System.NonSerialized]
	public string lastSpellCast, lastElement;
	[System.NonSerialized]
	public List<Spell> SpellsCast; //= new List<Spell>();

	public Monster(){
		Spells_ = new List<Spell>();
		//SpellNames = new List<string>();
		SpellsCast = new List<Spell>();
		Statuses_ = new List<Status>();
		StatusNames = new List<string>();
		Stats_ = new Stat[10];
	}
	public Monster Copy(){
  		Monster mon = (Monster) this.MemberwiseClone();
		
		mon.Spells_ = new List<Spell>(Spells_);
		mon.Statuses_ = new List<Status>(Statuses_);
		//mon.SpellNames = new List<string>(SpellNames);
		mon.StatusNames = new List<string>(StatusNames);
		
		mon.PaletteA_ = new Color(PaletteA_.r, PaletteA_.g, PaletteA_.b, PaletteA_.a);
		mon.PaletteB_ = new Color(PaletteB_.r, PaletteB_.g, PaletteB_.b, PaletteB_.a);
		
		mon.Stats_ = new Stat[10];
		for (int i = 0; i < Stats_.Length; i++)
		{
			mon.Stats_[i] = new Stat(Stats_[i]);
		}
		//Array.Copy(STATS_, mon.STATS_, 9);
		
		mon.SpellsCast = new List<Spell>();

		return mon;
 	}

	public void AddSpell(Spell sp){
		Spells_.Add(sp);
		//SpellNames.Add(sp.name);
	}
	public void AddStatus(Status st){
		Statuses_.Add(st);
		StatusNames.Add(st.Name);
	}

	public bool Die(){
		alive = false;
		return true;
	}

	public List<Damage> SimulateSpellPerformance(Spell SimulatedSpell, Point TargetedCell){
		List<Damage> SimulationResult = new List<Damage>();
		List<Monster> TargetedMonsters = new List<Monster>();

		if(SimulatedSpell.DamageSegments.Count == 0){
			
		}else{
			//if(SimulatedSpell.Radius == 1){ //single target
			//TODO:	Debug.Log("do the radius 1");
			//}else{ //radius or multiple target
				//know shape = worth for terrains
				List<Point> SpellShape = SimulatedSpell.EffectShapePoints(MonsterPoint, TargetedCell);
				//know targets = important for choices
				TargetedMonsters.AddRange(Environment.GameBoard.MonstersAt(TargetedCell, SpellShape));
				//in the future, move this to outer layer perhaps
			//}
			return SimulatedSpell.DamageInstances(this, TargetedMonsters);
		}

		return SimulationResult;
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

	public bool TakeDamage(Damage TakenDamage){
		Stats_[0].Decrease(TakenDamage.FinalDamage);
		return true;
		
	}

}



