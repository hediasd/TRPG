using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : DataObject {

	public Monster Caster, TargetMonster;
	public int CasterID, TargetID;
	public Spell Spell;
	public int BruteDamage, FinalDamage;
	public List<int> Instances;
	public bool EnoughToKill;
	//kind   normal, ground, poison

	public Damage(Monster Caster, Monster TargetMonster, Spell Spell, List<int> Instances, int BruteDamage, int FinalDamage){
		this.Caster = Caster;
		this.Spell = Spell;
		this.TargetMonster = TargetMonster;
		this.Instances = Instances;
		this.BruteDamage = BruteDamage;
		this.FinalDamage = FinalDamage;
		CasterID = Caster.ID;
		TargetID = TargetMonster.ID;
	}

}
