using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shape {

	public static List<Point> GetShape(int Name, int MaximumRange, int MinimumRange = -1){
		List<Point> MaximumShape = SwitchShape(Name, MaximumRange);
		if(MinimumRange >= 1){
			MinimumRange -= 1;
			List<Point> MinimumShape = SwitchShape(Name, MinimumRange);
			MaximumShape.RemoveAll(x => MinimumShape.Contains(x));
		}
		return MaximumShape;
	}
	static List<Point> SwitchShape(int Name, int Range){
		switch(Name){
			case GEOMETRY.CIRCLE:
				return CircleShape(Range);
			case GEOMETRY.CROSS:
				return CrossShape(Range);
			case GEOMETRY.SQUARE:
				return SquareShape(Range);
			default:
				Debug.Log("Unclassified or Unknown Shape " + Name);
				return new List<Point>();
		}
	}
	
	static List<Point> CircleShape(int Range){
		if(Range < 0) return new List<Point>();
		List<Point> FinalPoints = new List<Point>();
		for (int i = -Range; i < Range+1; i++)
		{
			for (int j = -Range; j < Range+1; j++)
			{
				if(Mathf.Abs(j)+Mathf.Abs(i) < Range+1){
					Point ShapePoint = new Point(i, j);
					FinalPoints.Add(ShapePoint);		
				}				
			}
		}
		return FinalPoints;
	}

	static List<Point> SquareShape(int Range){
		if(Range < 0) return new List<Point>();
		List<Point> FinalPoints = new List<Point>();
		for (int i = -Range; i <= Range; i++)
		{
			for (int j = -Range; j <= Range; j++)
			{
				Point ShapePoint = new Point(i, j);
				FinalPoints.Add(ShapePoint);						
			}
		}
		
		return FinalPoints;
	}

	static List<Point> CrossShape(int Range){
		if(Range < 0) return new List<Point>();
		List<Point> FinalPoints = new List<Point>();
		for (int i = -Range; i < 0; i++){
			Point p = new Point(i, 0);
			Point q = new Point(0, i);
			FinalPoints.Add(p);	
			FinalPoints.Add(q);	
		}
		for (int i = 1; i <= Range; i++){
			Point p = new Point(i, 0);
			Point q = new Point(0, i);
			FinalPoints.Add(p);	
			FinalPoints.Add(q);	
		}
		FinalPoints.Add(new Point(0,0));	
		return FinalPoints;
	}

		
}
