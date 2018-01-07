using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public static class Algorithms {

		static int[,] Corners = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};

		public static List<LinkedPoint> BlurredSpellCastRange(Point PointPivot, int[,] GroundMap, Spell SimulatedSpell, int BlurAmount){
			List<LinkedPoint> BlurredCastShape = new List<LinkedPoint>();

			//CastShape with Pivot
			List<Point> CastShape = SimulatedSpell.CastShapePoints(PointPivot);
			List<Point> Circle = Shape.GetShape(E.CIRCLE, BlurAmount);
			
			foreach (Point CastShapePoint in CastShape)
			{
				foreach (Point CirclePoint in Circle)
				{
					// Blurred point from a cast point
					LinkedPoint BlurredCastShapePoint = new LinkedPoint(CastShapePoint + CirclePoint);
					// If the cell is not obstructed, go ahead
					if(BlurredCastShapePoint.WithinLimits() && GroundMap[BlurredCastShapePoint.x, BlurredCastShapePoint.z] != 1){
						// If new, add to the list and add parent shape
						// Else only add parent shape to existing point
						int Index = BlurredCastShape.IndexOf(BlurredCastShapePoint);
						if(Index >= 0){ // Exists
							BlurredCastShape[0].Parents.Add(CastShapePoint);
						}else{
							BlurredCastShapePoint.Parents.Add(CastShapePoint);
							BlurredCastShape.Add(BlurredCastShapePoint);
						}
					}
				}
			}

			return BlurredCastShape;
		}

		public static List<Point> EmptyCells(Point StartingPoint, int radius, int[,] Map) {

			List<Point> moves = new List<Point>();
			
			for (int i = -radius; i <= radius; i++) {
				for (int j = -radius; j <= radius; j++) {
					if (Mathf.Abs(i) + Mathf.Abs(j) <= radius) {
						int xi = (int)StartingPoint.x + i; 
						int zj = (int)StartingPoint.z + j; 

						if(xi >= 0 && zj >= 0 && xi < Map.GetLength(0) && zj < Map.GetLength(1)){
							Point keyp = new Point(xi, zj); 
							if (Map[xi, zj] == 0) {
								bool good = true;
								if(good) moves.Add(keyp); 
							}
						}
					}
				}
			}
			
			return moves; 
		}

		public static List<Point> NeighbosdfsrPoints(Point OriginPoint, int[,] Map){
			List<Point> Neighbors = new List<Point>(); 
			int x = OriginPoint.x;
			int z = OriginPoint.z; 
			try{
			if(Map[x-1, z] == 0) Neighbors.Add(new Point(x-1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(Map[x+1, z] == 0) Neighbors.Add(new Point(x+1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(Map[x, z-1] == 0) Neighbors.Add(new Point(x, z-1));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(Map[x, z+1] == 0) Neighbors.Add(new Point(x, z+1));
		}
		catch (System.IndexOutOfRangeException){}
			/*
					for (int i = 0; i < 4; i++)
					{
						try
						{
							Point CornerPoint = new Point(Corners[i, 0], Corners[i, 1]);
							Point NeighborPoint = new Point(OriginPoint + CornerPoint);
							if(Map[NeighborPoint.x, NeighborPoint.z] == 0) Neighbors.Add(NeighborPoint);
						}
						catch (System.IndexOutOfRangeException){}
					}
		*/
			return Neighbors;

		}

		
	public static List<Point> GetReachableCells(Point a, int radius, int layer = 0){
		List<Point> neighbors = new List<Point>(); 
		Queue<Point> points = new Queue<Point>(GetNeighbors(a));

		while(points.Count > 0){
			Point p = points.Dequeue();
			if(Point.Distance(p, a) <= radius && !IsContained(p, neighbors)){
				neighbors.Add(p);
				foreach (Point q in GetNeighbors(p)){
					points.Enqueue(q);
				}
			}
		}

		return neighbors;
	}
	
	public static List<Point> GetNeighbors(Point a, int layer = 0, bool free = true){
		List<Point> neighbors = new List<Point>(); 
		int x = a.x;
		int z = a.z;
		GameboardMaster gb = GameObject.Find("Logic").GetComponent<GameboardMaster>();
		try{
			if(gb.Board[x-1, z, layer] == 0) neighbors.Add(new Point(x-1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(gb.Board[x+1, z, layer] == 0) neighbors.Add(new Point(x+1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(gb.Board[x, z-1, layer] == 0) neighbors.Add(new Point(x, z-1));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(gb.Board[x, z+1, layer] == 0) neighbors.Add(new Point(x, z+1));
		}
		catch (System.IndexOutOfRangeException){}
		return neighbors;
	}



	public static List<Point> GetPath(Point a, Point b) {

		Point.pivot = b;

		List<Point> walkable = new List<Point>(GetReachableCells(a, 7));
		if(!IsContained(b, walkable)) return new List<Point>();

		List<Point>	Visited = new List<Point>();
		List<Point> Queue = new List<Point>();
		Queue.Add(a);

		while(Queue.Count > 0){

			Point p = Queue[0];
			Queue.RemoveAt(0);
			Visited.Add(p);
			
			if(p == b){
				List<Point> final = new List<Point>();
				while (p.Father != null){
					Point r = p - p.Father;
					final.Add(r);
					p = p.Father;
				}
				final.Reverse();
				return final;
			}

			
			List<Point> neighbors = new List<Point>(GetNeighbors(p));

			foreach (Point q in neighbors)
			{
				q.Father = p;
				q.Depth = p.Depth + 1;
				if(!IsContained(q, Visited) && IsContained(q, walkable)){ // && !IsIncluded(q, p)
						Queue.Add(q);
				}
			}
			Queue.Sort((p1, p2) => Point.DistancePivot(p1, p2));
		}	

		//return new List<Point>();

		return null;
	}


	public static List<Point> ReachableUnnocupiedCells(Point StartingPoint, int Radius, int[,] Map) {

		List<Point> moves = EmptyCells(StartingPoint, Radius, Map); 
		List<Point> reach = new List<Point>(); 
		Queue<Point> queue = new Queue<Point>(); 

		List<Monster> Obstacles = new List<Monster>();
		if(BattleMaster.Teams[0].Contains(BattleMaster.OnTurn)){
			Obstacles.AddRange(BattleMaster.Teams[1]);
		}else{
			Obstacles.AddRange(BattleMaster.Teams[0]);
		}
		//obstacles.AddRange(BattleMaster.Allmons);

		for (int i = moves.Count-1; i >= 0; i--)
		{
			for (int j = 0; j < Obstacles.Count; j++)
			{
				if(moves[i] == new Point(Obstacles[j])){
					moves.RemoveAt(i);
					break;
				}
			}
			
		}

		queue.Enqueue(StartingPoint); 

		while (queue.Count > 0) {
			Point p = queue.Dequeue(); 
			foreach (Point q in moves) {
				if (Point.Distance(p, q) == 1 && !reach.Contains(q)) {
					q.Depth = Point.Distance(StartingPoint, q);
					reach.Add(q); 
					queue.Enqueue(q); 
				}
			}			
		}

		return reach; 
	}

		static bool IsContained(Point p, List<Point> visited){
			foreach (Point q in visited)
			{
				if(p.x == q.x && p.z == q.z) return true;
			}
		 return false;
		}



	}


