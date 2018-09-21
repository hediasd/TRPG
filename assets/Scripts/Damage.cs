using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : DataObject {

	public MonsterInstance Caster, TargetMonster;
	public int CasterID, TargetID;
	public SpellEntry Spell;
	public int BruteDamage, FinalDamage;
	public List<int> Instances;
	public bool EnoughToKill;
	//kind   normal, ground, poison

	public Damage(MonsterInstance Caster, MonsterInstance TargetMonster, SpellEntry Spell, List<int> Instances, int BruteDamage, int FinalDamage){
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
