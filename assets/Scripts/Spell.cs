using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Spell : DataObject {

	public string Description;
	public string AnimationName;
	public int Cooldown, Level;
	public int Radius;
	
	//public List<Property> Properties = new List<Property>();
	//public List<DamageSegment> DamageSegments = new List<DamageSegment>();
	public string Damage;

	public bool LoS, BoostableRange, Linear;
	public int MinimumCastRange, MaximumCastRange;
	public string Property1, Property2, Property3;
	public string CastShape, EffectShape; //EffectSquare / EffectCone / EffectCircle / EffectLine / Effect3Line (Default: EffectCircle)
	public string Targets; //Self / Allies / Enemies / Both / None / All
	
	[System.NonSerialized]
	public int SpellCastShape, SpellEffectShape, SpellTargets;
	[System.NonSerialized]
	public List<DamageSegment> DamageSegments = new List<DamageSegment>();

	public Spell(){
		//Elements.Add(E.DARKNESS);
		//Damages.Add(10);
		Cooldown = Level = MinimumCastRange = MaximumCastRange = 1;
		Radius = 0;
		SpellCastShape = SpellEffectShape = GEOMETRY.CIRCLE;
		SpellTargets = TARGET.ENEMIES;		
		//anim = new Animation();
		//anim.sheet = "Bright2";
	}

	public List<Damage> DamageInstances(Monster Caster, List<Monster> targets){
		List<Damage> Instances = new List<Damage>();

		foreach (Monster TargetMonster in targets)
		{
			List<int> SegmentsIndividualDamages = new List<int>();
			int OverallSegmentsBruteDamage = 0;
			int OverallSegmentsTotalDamage = 0;
			foreach (DamageSegment Segment in DamageSegments)
			{ //natural damage
				OverallSegmentsBruteDamage += Segment.Value;

				int SegmentBakedDamage = Segment.Value; //spell segment power
				SegmentBakedDamage *= (Caster.StatList[1] + 1) / (TargetMonster.StatList[1] + 1);
				//BakedDamage *= 
				SegmentsIndividualDamages.Add(SegmentBakedDamage);
				OverallSegmentsTotalDamage += SegmentBakedDamage;
			}
			Instances.Add(new Damage(Caster, TargetMonster, this, SegmentsIndividualDamages, OverallSegmentsBruteDamage, OverallSegmentsTotalDamage));
		}
		return Instances;
		
	}

	public List<Point> CastShapePoints(Point PivotPoint = null){// int Radius){
		
		List<Point> ShapePoints = Shape.GetShape(SpellCastShape, MaximumCastRange, MinimumCastRange);
		if(PivotPoint != null){
			for (int i = 0; i < ShapePoints.Count; i++)
			{
				ShapePoints[i] += PivotPoint;
			}
		}

		return ShapePoints;
	}

	public List<Point> EffectShapePoints(Point fr, Point to, Point PivotPoint = null){// int Radius){
		List<Point> ShapePoints = new List<Point>();
		float[] d = new float[2];
		float[] f = new float[2];

		float difz = to.z - fr.z;
		float difx = to.x - fr.x;
		if(Mathf.Abs(difx) > Mathf.Abs(difz)){
			d[0] = Mathf.Sign(to.x - fr.x);
			d[1] = 0;
			f[0] = 0;
			f[1] = Mathf.Sign(to.z - fr.z);
		}else{
			d[0] = 0;
			d[1] = Mathf.Sign(to.z - fr.z);
			f[0] = Mathf.Sign(to.x - fr.x);
			f[1] = 0;
		}

		switch(SpellEffectShape){
			case GEOMETRY.CIRCLE:
			case GEOMETRY.SQUARE:
				ShapePoints = Shape.GetShape(SpellEffectShape, MaximumRange: Radius);
				break;
			case GEOMETRY.CONE:
				for (int i = 0; i < Radius+1; i++)
				{
					for (int j = -i; j < i+1; j++)
					{
						Point p = new Point(i*d[0] + j*f[0], i*d[1] + j*f[1]);
						ShapePoints.Add(p);
					}
				}
				break;
			case GEOMETRY.LINE:
				for (int i = 0; i < Radius+1; i++){
					Point p = new Point(i*d[0], i*d[1]);
					ShapePoints.Add(p);
				}
				break;
			case GEOMETRY.NONE: ///
				Shape.GetShape(GEOMETRY.CIRCLE, MaximumRange: Radius);
				break;
		}

		if(PivotPoint != null){
			for (int i = 0; i < ShapePoints.Count; i++)
			{
				ShapePoints[i] += PivotPoint;
			}
		}

		return ShapePoints;
	}

	public List<List<Point>> FragmentedShape(Point fr, Point to, List<Point> shape, int fragmentshape){// int Radius){
		
		List<List<Point>> pts = new List<List<Point>>();
		List<Point> point_pool = new List<Point>(shape);
		if(point_pool.Count == 0){
			Debug.Log("Empty shape");
			return pts;
		}

		float[] d = new float[2];
		float[] f = new float[2];
		float difz = to.z - fr.z;
		float difx = to.x - fr.x;
		if(Mathf.Abs(difx) > Mathf.Abs(difz)){
			d[0] = Mathf.Sign(to.x - fr.x);
			d[1] = 0;
			f[0] = 0;
			f[1] = Mathf.Sign(to.z - fr.z);
		}else{
			d[0] = 0;
			d[1] = Mathf.Sign(to.z - fr.z);
			f[0] = Mathf.Sign(to.x - fr.x);
			f[1] = 0;
		}

		switch(fragmentshape){
			case GEOMETRY.CIRCLE:
				point_pool.Sort((p1, p2) => Mathf.RoundToInt(Point.Distance(to, p1) - Point.Distance(to, p2)));
				for (int i = 0; point_pool.Count > 0; i++){
					List<Point> pt = new List<Point>();
					pt.Add(point_pool[0]);
					point_pool.RemoveAt(0);
					while(point_pool.Count > 0){
						if(Point.Distance(to, point_pool[0]) == Point.Distance(to, pt[0])){
							pt.Add(point_pool[0]);
							point_pool.RemoveAt(0);
						}else{
							break;
						}
					}
					pts.Add(pt);
				}
				break;
			case GEOMETRY.CONE:
				/*for (int i = 0; i < Radius+1; i++)
				{
					for (int j = -i; j < i+1; j++)
					{
						Point p = new Point(to.x + i*d[0] + j*f[0], to.z + i*d[1] + j*f[1]);
						pts.Add(p);
					}
				}
				*/
			break;
			case GEOMETRY.LINE:
				for (int i = 0; i < Radius+1; i++){
					Point p = new Point(to.x + i*d[0], to.z + i*d[1]);
				//	pts.Add(p);
				}
			break;
			case GEOMETRY.HORIZONTAL_LINE:
				point_pool.Sort((p1, p2) => Mathf.RoundToInt(p1.z - p2.z));
				for (int i = 0; point_pool.Count > 0; i++){
					List<Point> pt = new List<Point>();
					pt.Add(point_pool[0]);
					point_pool.RemoveAt(0);
					while(point_pool.Count > 0){
						if(point_pool[0].z == pt[0].z){
							pt.Add(point_pool[0]);
							point_pool.RemoveAt(0);
						}else{
							break;
						}
					}
					pts.Add(pt);
				}
			break;
			case GEOMETRY.NONE: ///

			break;
		}
		return pts;
	}

}
