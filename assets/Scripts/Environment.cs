using UnityEngine; 
using System.IO;
using System.Collections; 
using System.Collections.Generic; 

public class Environment : MonoBehaviour {

	// destroy this class, make a full-algorithmwise one

	//public static Hashtable firstTable;
	public static GameboardMaster GameBoard; 
	BattleMaster BattleMaster;

	void Start () { //chews up map
		BattleMaster = GetComponent<BattleMaster>();
		GameBoard = BattleMaster.GameBoard;
	}

	public static bool IsOccupied(Point Cell){
		return (GameBoard.MonsterAt(Cell) != null);
	}


	public static Monster GetNearestTarget(Point p, List<Monster> ValidTargets){
		int dist = 99;
		int index = -1;

		for (int i = 0; i < ValidTargets.Count; i++){ //new Point(ValidTargets[i])
			List<Point> path = GetSimplePath(p, ValidTargets[i].MonsterPoint, false);
			if(path.Count < dist){
				dist = path.Count;
				index = i;
			}
		}
		//pathes.Reverse();	
		
		if(index == -1) return null;
		return ValidTargets[index];
	}



	public static List <Point> GetEmptyCells(Point a, int radius) {

		List <Point> moves = new List <Point> ();
		
		for (int i = -radius; i <= radius; i++) {
			for (int j = -radius; j <= radius; j++) {
				if (Mathf.Abs(i) + Mathf.Abs(j) <= radius) {
					int xi = (int)a.x + i; 
					int zj = (int)a.z + j; 

					if(xi >= 0 && zj >= 0){
						Point keyp = new Point(xi, zj); 
						if (Environment.GameBoard.At(keyp) == 0) {
							bool good = true;
							/*foreach (GameObject go in BattleMaster.Enemies)
							{
								Point p = new Point(go);
								if(p.x == xi && p.z == zj) {
									good = false;
									break;
								}
							}*/
							if(good) moves.Add(keyp); 
						}
					}
				}
			}
		}
		
		return moves; 
	}



	public static List<Point> GetReachableUnnocupiedCells(Point StartingPoint, int Radius) {

		List<Point> moves = GetEmptyCells(StartingPoint, Radius); 
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

	public static bool IsNeighbor(Point p, Point q){
		return Point.Distance(p, q) == 1;
	}

    static bool IsContained(Point p, List<Point> visited){
		 foreach (Point q in visited)
		 {
			 if(p.x == q.x && p.z == q.z) return true;
		 }
		 return false;
	 }

	static bool IsIncluded(Point p, Point q){
		 if(q.Father == null) return false;
		 if(p.x == q.Father.x && p.z == q.Father.z) return true;
		 return IsIncluded(p, q.Father);
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
		try{
			if(GameBoard.Board[x-1, z, layer] == 0) neighbors.Add(new Point(x-1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(GameBoard.Board[x+1, z, layer] == 0) neighbors.Add(new Point(x+1, z));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(GameBoard.Board[x, z-1, layer] == 0) neighbors.Add(new Point(x, z-1));
		}
		catch (System.IndexOutOfRangeException){}
		try{
			if(GameBoard.Board[x, z+1, layer] == 0) neighbors.Add(new Point(x, z+1));
		}
		catch (System.IndexOutOfRangeException){}
		return neighbors;
	}

	public static List<Point> GetPath(Point a, Point b, bool full) {
		int[,] obstacles = GameBoard.GetLayer(0);

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


	public static List<Point> GetFlyingPath(Point a, Point b) {
		List<Point> final = new List<Point>();
		float dx = b.x - a.x;
		float dz = b.z - a.z;
		float abs = Mathf.Abs(dx) + Mathf.Abs(dz);
		float moveX = dx / abs;
		float moveZ = dz / abs;

		float q = 0;
		do{
			q++;
			final.Add(new Point(moveX, moveZ));
		}while(q < abs);

		return final;
	}

	public static List<Point> GetSimplePath(Point a, Point b, bool full) {
		
		Point.pivot = b;

		List<Point> walkable = new List<Point>(GetReachableCells(a, 8));
		if(!IsContained(b, walkable)) return new List<Point>();

		List<Point> visited = new List<Point>();
		List<Point> queue = new List<Point>();
		queue.Add(a);

		while(queue.Count > 0){

			Point p = queue[0];
			visited.Add(p);
			//if(p.Equals(b)){ 
			if((full && (p == b)) || (!full && Point.Distance(p, b) == 1)){
				List<Point> final = new List<Point>();
				while (p.Father != null){
					Point r = p - p.Father;
					final.Add(r);
					p = p.Father;
				}
				final.Reverse();
				return final;
			}

			queue.RemoveAt(0);
			List<Point> neighbors = new List<Point>(GetNeighbors(p));

			foreach (Point q in neighbors)
			{
				q.Father = p;
				q.Depth = p.Depth + 1;
				if(!IsContained(q, visited) && IsContained(q, walkable)){ // && !IsIncluded(q, p)
						queue.Add(q);
				}
			}
			queue.Sort((p1, p2) => Point.DistancePivot(p1, p2));
		}	

		return new List<Point>();

	}

	
	
/*
	public static List<Point> GetPossiblePath(Point a, Point b, bool full) {
		
		List<Point> walkable = new List<Point>(GetReachableCells(a.x, a.z, 8));
		int distAG = 99;
		int distBG = 99;
		Point goal = new Point(2000, 2000);

		for (int i = 0; i < walkable.Count; i++)
		{
			int distAW = Point.Distance(a, walkable[i]);
			int distBW = Point.Distance(b, walkable[i]);
			if(distAW <= distAG && distBW <= distBG){
				goal = walkable[i];
				distAG = distAW;
				distBG = distBW;
			}
		}

		//if(!IsContained(goal, walkable)) return new List<Point>();
		visited = new List<Point>();
		List<Point> queue = new List<Point>();
		queue.Add(a);
		Point.pivot = goal;
		Point p;

		while(queue.Count > 0){

			p = queue[0];
			visited.Add(p);
			//if(Point.Distance(p, b) == 1){
			if(p.x == goal.x && p.z == goal.z){
				List<Point> final = new List<Point>();
				while (p.father != null){
					Point r = Point.Degrade(p, p.father);
					final.Add(r);
					p = p.father;
				}
				final.Reverse();
				return final;
			}

			queue.RemoveAt(0);
			List<Point> neighbors = new List<Point>(GetNeighbors(p));

			foreach (Point q in neighbors)
			{
				q.father = p;
				q.depth = p.depth + 1;
				if(!IsContained(q, visited) && IsContained(q, walkable)){ // && !IsIncluded(q, p)
						queue.Add(q);
				}
			}
			queue.Sort((p1, p2) => Point.DistanceP(p1, p2));
		}	

		return new List<Point>();

	}
	*/
	
}
