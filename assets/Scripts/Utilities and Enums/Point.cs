using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedPoint : Point {
	public List<Point> Parents = new List<Point> ();

	public LinkedPoint () : base () { }
	public LinkedPoint (float xi, float zi) : base (xi, zi) { }
	public LinkedPoint (Point p) : base (p) { }
}

public class PointDuo {
	Point One, Two;
	public PointDuo (Point a, Point b) {
		One = a;
		Two = b;
	}
	public static bool operator == (PointDuo pa, PointDuo pb) {
		if (((object) pa == null) || ((object) pb == null)) return ((object) pa == (object) pb);
		return ((pa.One == pb.One) && (pa.Two == pb.Two));
	}
	public static bool operator != (PointDuo pa, PointDuo pb) {
		if (((object) pa == null) || ((object) pb == null)) return ((object) pa == (object) pb);
		return ((pa.One == pb.One) && (pa.Two == pb.Two));
	}
}

public class Point {
	public int x;
	public int z;
	public Point Father;
	public float Depth = 0;

	public static Point pivot;
	public static Point Limits;

	public Point () {
		x = 0;
		z = 0;
	}
	public Point (GameObject go) {
		x = (int) go.transform.position.x;
		z = (int) go.transform.position.z;
	}
	public Point (MonsterInstance mon) {
		x = (int) mon.MonsterPoint.x;
		z = (int) mon.MonsterPoint.z;
	}
	public Point (Point p) {
		x = p.x;
		z = p.z;
	}

	public Point (float xi, float zi, float Depth = 0) {
		this.Depth = Depth;
		x = (int) xi;
		z = (int) zi;
	}

	public bool WithinLimits () {
		return (this.x >= 0 && this.x < Limits.x && this.z >= 0 && this.z < Limits.z);
	}
	public bool WithinLimits (Point p) {
		return (this.x >= 0 && this.x < p.x && this.z >= 0 && this.z < p.z);
	}

	public static int Distance (Point a, Point b) {
		return (int) (Mathf.Abs (b.x - a.x) + Mathf.Abs (b.z - a.z));
	}

	public List<Point> FathersList () {
		List<Point> Fathers = new List<Point> ();
		Point p = this;
		Fathers.Add (p);

		while (p.Father != null) {
			p = p.Father;
			Fathers.Insert (0, p);
		}

		return Fathers;
	}

	public static Point operator + (Point a, Point b) {
		return new Point (a.x + b.x, a.z + b.z);
	}
	public static Point operator - (Point a, Point b) {
		return new Point (a.x - b.x, a.z - b.z);
	}
	public static bool operator == (Point a, Point b) {
		if (((object) a == null) || ((object) b == null)) return ((object) a == (object) b);
		return ((a.x == b.x) && (a.z == b.z));
	}
	public static bool operator != (Point a, Point b) {
		if (((object) a == null) || ((object) b == null)) return !((object) a == (object) b);
		return !((a.x == b.x) && (a.z == b.z));
	}
	public override bool Equals (object obj) {
		if (obj == null || GetType () != obj.GetType ()) {
			return false;
		}
		return (this == (Point) obj);
	}

	// override object.GetHashCode
	public override int GetHashCode () {
		return ((x * 10000) + (z));
	}

	public static int DistancePivot (Point a, Point b) {
		int da = Distance (a, pivot);
		int db = Distance (b, pivot);
		return da.CompareTo (db);
	}

	public override string ToString () {
		return "(" + x + ", " + z + ")";
	}

}