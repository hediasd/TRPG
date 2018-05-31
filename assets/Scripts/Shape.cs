using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shape {

	public static List<Point> GetShape (int Name, float[, ] DirectionPivot, int MaximumRange, int MinimumRange = -1) {
		List<Point> MaximumShape = SwitchShape (Name, DirectionPivot, MaximumRange);
		if (MinimumRange >= 1) {
			MinimumRange -= 1;
			List<Point> MinimumShape = SwitchShape (Name, DirectionPivot, MinimumRange);
			MaximumShape.RemoveAll (x => MinimumShape.Contains (x));
		}
		return MaximumShape;
	}

	static List<Point> SwitchShape (int Name, float[, ] DirectionPivot, int Range) {
		try {

			switch (Name) {
				case GEOMETRY.CIRCLE:
					return CircleShape (Range);
				case GEOMETRY.CROSS:
					return CrossShape (Range);
				case GEOMETRY.SQUARE:
					return SquareShape (Range);
				case GEOMETRY.CONE:
					return ConeShape (DirectionPivot, Range);
				case GEOMETRY.LINE:
					return LineShape (DirectionPivot, Range);
				default:
					Debug.Log ("Unclassified or Unknown Shape " + Name);
					return new List<Point> ();
			}

		} catch (System.Exception) {
			return CircleShape (Range);
		}

	}

	static List<Point> CircleShape (int Range) {
		if (Range < 0) return new List<Point> ();
		List<Point> FinalPoints = new List<Point> ();

		for (int i = -Range; i <= Range; i++) {
			for (int j = -Range; j <= Range; j++) {
				if (Mathf.Abs (j) + Mathf.Abs (i) <= Range) {
					Point ShapePoint = new Point (i, j);
					FinalPoints.Add (ShapePoint);
				}
			}
		}

		return FinalPoints;
	}

	static List<Point> ConeShape (float[, ] Pivots, int Range) {
		if (Range < 0) return new List<Point> ();
		List<Point> FinalPoints = new List<Point> ();

		for (int i = 0; i <= Range; i++) {
			for (int j = -i; j <= i; j++) {
				Point p = new Point (i * Pivots[0, 0] + j * Pivots[1, 0], i * Pivots[1, 0] + j * Pivots[1, 1]);
				FinalPoints.Add (p);
			}
		}

		return FinalPoints;
	}

	static List<Point> LineShape (float[, ] Pivots, int Range) {
		if (Range < 0) return new List<Point> ();
		List<Point> FinalPoints = new List<Point> ();

		for (int i = 0; i < Range + 1; i++) {
			Point p = new Point (i * Pivots[0, 0], i * Pivots[0, 1]);
			FinalPoints.Add (p);

		}

		return FinalPoints;

	}

	static List<Point> SquareShape (int Range) {
		if (Range < 0) return new List<Point> ();
		List<Point> FinalPoints = new List<Point> ();

		for (int i = -Range; i <= Range; i++) {
			for (int j = -Range; j <= Range; j++) {
				Point ShapePoint = new Point (i, j);
				FinalPoints.Add (ShapePoint);
			}
		}

		return FinalPoints;
	}

	static List<Point> CrossShape (int Range) {
		if (Range < 0) return new List<Point> ();
		List<Point> FinalPoints = new List<Point> ();
		
		for (int i = -Range; i < 0; i++) {
			Point p = new Point (i, 0);
			Point q = new Point (0, i);
			FinalPoints.Add (p);
			FinalPoints.Add (q);
		}
		for (int i = 1; i <= Range; i++) {
			Point p = new Point (i, 0);
			Point q = new Point (0, i);
			FinalPoints.Add (p);
			FinalPoints.Add (q);
		}
		FinalPoints.Add (new Point (0, 0));
		return FinalPoints;
	}

}