using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellEntry : DataEntry {

	public string Description, AnimationName;
	public int Cooldown, Level, Radius;

	//public List<Property> Properties = new List<Property>();
	//public List<DamageSegment> DamageSegments = new List<DamageSegment>();
	public string Damage;

	public bool LoS, BoostableRange, Linear;
	public string Property1, Property2, Property3;
	public string CastRange, CastShape, EffectShape; //EffectSquare / EffectCone / EffectCircle / EffectLine / Effect3Line (Default: EffectCircle)
	public string Targets; //Self / Allies / Enemies / Both / None / All

	[System.NonSerialized]
	public int SpellCastShape, SpellEffectShape, SpellTargets, MinimumCastRange, MaximumCastRange;
	[System.NonSerialized]
	public List<DamageSegment> DamageSegments = new List<DamageSegment> ();

	public SpellEntry () {
		//Elements.Add(E.DARKNESS);
		//Damages.Add(10);
		Cooldown = Level = MinimumCastRange = MaximumCastRange = 1;
		Radius = 0;
		SpellCastShape = SpellEffectShape = GEOMETRY.CIRCLE;
		SpellTargets = TARGETS.ENEMIES;
		//anim = new Animation();
		//anim.sheet = "Bright2";
	}

	public SpellInstance Instantiate () {
		return new SpellInstance (this);
	}

	public override void Startup () {

		//if (MaximumCastRange - MinimumCastRange < 0) //throw new Exception ();
		string[] Segments = Utility.ChewUp (Damage); //sp.Damage

		for (int i = 0; i < Segments.Length; i += 2) {
			DamageSegments.Add (new DamageSegment (int.Parse (Segments[i]), ELEMENT.GetElementValue (Segments[i + 1])));
		}

		string[] Range = Utility.ChewUp (CastRange); //sp.Damage

		for (int i = 0; i < Range.Length; i++) {
			MinimumCastRange = int.Parse (Range[0]);
			MaximumCastRange = int.Parse (Range[1]);
		}

		SpellCastShape = GEOMETRY.GetGeometryValue (CastShape);
		SpellEffectShape = GEOMETRY.GetGeometryValue (EffectShape);
		SpellTargets = TARGETS.GetTargetsValue (Targets);

	}

}