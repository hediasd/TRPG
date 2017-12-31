using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMaster : MonoBehaviour {

	public GameObject Logic;
	BattleMaster BattleMaster;
	bool Q, W, e, R, T, Y, U;

	void Start () {
		BattleMaster = Logic.GetComponent<BattleMaster>();	
		
		Point a = new Point(1,2);
		//LinkedPoint BlurredPoint = (LinkedPoint) (a + a);
		//Debug.Log(BlurredPoint);
		//Debug.Log(BlurredPoint.Parents.Count);
	}

	float[,] InfluenceMap(){
		GameboardMaster GameBoard = BattleMaster.GameBoard;
		float[,] FinalMap = new float[GameBoard.size.x, GameBoard.size.z];

		float MaxInfluence = 0.5f;
		float MaxDepth = 5;

		foreach (Monster mon in GameBoard.MonstersOnBoard.Values)
		{
			if(mon.Team == 0){
				List<Point> MonInfluences = Environment.GetReachableUnnocupiedCells(mon.Point, 5);
				foreach (Point InfluencePoint in MonInfluences)
				{
					FinalMap[InfluencePoint.x, InfluencePoint.z] += (MaxDepth - InfluencePoint.Depth) * 0.12f;
					//FinalMap[InfluencePoint.x, InfluencePoint.z] += 1 * (Mathf.Pow(0.6f, InfluencePoint.Depth));
					//FinalMap[InfluencePoint.x, InfluencePoint.z] += MaxInfluence - (MaxInfluence * (InfluencePoint.Depth / MaxDepth) * (InfluencePoint.Depth / MaxDepth));
				}
			}
		}
		foreach (Monster mon in GameBoard.MonstersOnBoard.Values)
		{
			FinalMap[mon.Point.x, mon.Point.z] = 0;
		}
	
		return FinalMap;
	}

	List<LinkedPoint> BlurredSpellCastRange(Point PointPivot, Spell SimulatedSpell, int BlurAmount){
		List<LinkedPoint> BlurredCastShape = new List<LinkedPoint>();

		//CastShape with Pivot
		List<Point> CastShape = SimulatedSpell.CastShapePoints(PointPivot);
		int[,] GroundMap = BattleMaster.GameBoard.GetLayer(E.GROUND_LAYER);
		List<Point> Circle = Shape.GetShape(E.CIRCLE, BlurAmount);
		
		foreach (Point CastShapePoint in CastShape)
		{
			foreach (Point CirclePoint in Circle)
			{
				// Blurred point from a cast point
				LinkedPoint BlurredCastShapePoint = new LinkedPoint(CastShapePoint + CirclePoint);
				// If the cell is not obstructed, go ahead
				if(GroundMap[BlurredCastShapePoint.x, BlurredCastShapePoint.z] != 1){
					// If new, add to the list and add parent shape
					// Else only add parent shape to existing point
					int Index = BlurredCastShape.IndexOf(BlurredCastShapePoint);
					if(Index >= 0){ // Exists
						BlurredCastShape[Index].Parents.Add(CastShapePoint);
					}else{
						BlurredCastShapePoint.Parents.Add(CastShapePoint);
						BlurredCastShape.Add(BlurredCastShapePoint);
					}
				}
			}
		}

		return BlurredCastShape;
	}

	void Update () {
		if(Input.GetKeyDown("q")){
			Q = !Q;
		}
		if(Input.GetKeyDown("w")){
			W = !W;
		}		
		if(Input.GetKeyDown("e")){
			e = !e;
		}	

		if(false){
			BlueCells BlueCells = GetComponent<BlueCells>();
			BlueCells.CleanUp();

			List<Point> PointList = new List<Point>();
			float[,] InfMap = InfluenceMap();
			for (int i = 0; i < InfMap.GetLength(0); i++)
			{
				for (int j = 0; j < InfMap.GetLength(1); j++)
				{
					if(InfMap[i,j] > 0) PointList.Add(new Point(i,j, Depth:InfMap[i,j]));
				}
			}
			BlueCells.Feed(PointList, true, Max: 0.52f);	
		}
		if(Q){
			BlueCells BlueCells = GetComponent<BlueCells>();
			BlueCells.CleanUp();

			Spell sp = BattleMaster.Grimoire.Spells[0];
			Point p1 = new Point(27, 35);
			List<LinkedPoint> lp = BlurredSpellCastRange(p1, sp, 5);
			List<Point> pp = new List<Point>();
			foreach (LinkedPoint p2 in lp)
			{
				//pp.Add((Point) p2);
			}

			BlueCells.Feed(lp);
			//BlueCells.Feed(pp, depther: false);
		}
		if(W){
			BlueCells BlueCells = GetComponent<BlueCells>();
			BlueCells.CleanUp();

			Spell sp = new Spell();
			sp.Radius = 3;
			sp.CastShape = E.CROSS;
			List<Point> pts = sp.CastShapePoints();
			for (int i = 0; i < pts.Count; i++)
			{
			//	pts[i] += new Point(20,20);
			}
			BlueCells.Feed(pts, depther: false);	
		}
			
	//	}		
			
		//if(Input.GetKeyDown("down") && undefined < 5) undefined++;
		//if(Input.GetKeyDown("up") && undefined > 0) undefined--;
	}
}
