using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dump {

	/*

		PieceMove OldPathmakerPlanningMaster(Monster ThinkingMonster, Point Goal){

		Point Here = new Point(ThinkingMonster.Point);

		Monster NearestMonster = Environment.GetNearestTarget(ThinkingMonster.Point, BattleMaster.Teams[1].Contains(ThinkingMonster) ? BattleMaster.Teams[0] : BattleMaster.Teams[1]);
		
		BattleMaster.Log("["+ThinkingMonster.Name+"] aims for ["+NearestMonster.Name+"]");

		Point near = new Point(NearestMonster);
		Point next = Here;
		int dist = 99;
		List<Point> reachables = Environment.GetReachableUnnocupiedCells(Here, 2);//Unnocupied
		//OverlaysMaster.CleanUp();
		//OverlaysMaster.SpawnSpellCells(reachables, 1);
		foreach (Point r in reachables)
		{
			if(!Environment.IsOccupied(r)){
				if(Point.Distance(r, near) < dist){
					dist = Point.Distance(r, near);
					next = r;
				}
				else if(Point.Distance(r, near) == dist && Point.Distance(Here, r) < Point.Distance(Here, near)){
					next = r;
				}
			}
		}

		Actions.Enqueue(new PieceMove(ThinkingMonster, onTurnPoint, next));
		


	}





	public GameboardMaster(int x, int z){
		size = new Point(x, z);
		Board = new int[x, z, 3];

		int[,,] a = new int[2,2,2];
		int[,,] b = new int[2,2,2];
		a[0,0,1] = 22;
		a[0,1,0] = 11;
		System.Array.Copy(a, b, a.Length);
		b[0,1,0] = 33;

		string s = "";
		for(int k = 0; k < a.GetLength(0); k++){
			s += "[ ";
			for(int i = 0; i < a.GetLength(1); i++){
				for(int j = 0; j < a.GetLength(2); j++){
					s += a[i,j,k] + " ";
				}
			}
			s += "] ";
		}
		Debug.Log(s);
		for(int i = 0; i < a.GetLength(0); i++){
			for(int j = 0; j < a.GetLength(1); j++){
				for(int k = 0; k < a.GetLength(2); k++){
					Debug.Log(string.Format("{0} ", b[i,j,k]));
				}
			}
		}
		Debug.Log("");
		//Debug.Log(string.Join("; ", a));
	}
	

	public static List<Point> GetSimplePath(Point a, Point b, bool full) {
		
		Point.pivot = b;

		List<Point> walkable = new List<Point>(GetReachableCells(a, 8));
		if(!IsContained(b, walkable)) return new List<Point>();

		visited = new List<Point>();
		List<Point> queue = new List<Point>();
		queue.Add(a);

		while(queue.Count > 0){

			Point p = queue[0];
			visited.Add(p);
			//if(p.Equals(b)){ 
			if((full && p.Equalpoint(b)) || (!full && Point.Distance(p, b) == 1)){
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

	public static List<Point> GetPaths(Point a, Point b, bool full) {
		
		Point.pivot = b;
		List<GameObject> obstacles = new List<GameObject>();
		if(BattleMaster.Allies.Contains(BattleMaster.OnTurn)){
			obstacles.AddRange(BattleMaster.Enemies);
		}else{
			obstacles.AddRange(BattleMaster.Allies);
		}


		for (int i = walkable.Count - 1; i >= 0 ; i--)
		{
			for (int j = 0; j < obstacles.Count; j++)
			{
				if(walkable[i].Equalpoint(new Point(obstacles[j]))){
					walkable.RemoveAt(i);
					break;
				}
			}
		}

		visited = new List<Point>();
		List<Point> queue = new List<Point>();
		queue.Add(a);

		while(queue.Count > 0){

			Point p = queue[0];
			visited.Add(p);
			//if(p.Equals(b)){ 
			if((full && p.Equalpoint(b)) || (!full && Point.Distance(p, b) == 1)){
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

		public static List<Point> GetReachableCells(Point a, int radius) {

		List<Point> moves = GetEmptyCells(a, radius); 
		List<Point> reach = new List<Point>(); 
		Queue<Point> queue = new Queue<Point>(); 

		queue.Enqueue(a); 

		while (queue.Count > 0) {
			Point p = queue.Dequeue(); 
			foreach (Point q in moves) {
				if (Point.Distance(p, q) == 1 && !reach.Contains(q)) {
					reach.Add(q); 
					queue.Enqueue(q); 
				}
			}			
		}
		return reach; 
	}
	
	
	
	public static List<Point> GetNeighbors(Point p) {

		List<Point> neighbors = new List<Point>(); 

		int j = 0;
		int i = 0;

		for (i = -1; i < 2; i += 2) {	
			if (Environment.GameBoard.At((int)p.x+i, (int)p.z+j) == 0) {
				Point n = new Point(p.x+i, p.z+j);
				neighbors.Add(n); 
			}
		}

		i = 0;

		for (j = -1; j < 2; j += 2) {	
			if (Environment.GameBoard.At((int)p.x+i, (int)p.z+j) == 0) {
				Point n = new Point(p.x+i, p.z+j);
				neighbors.Add(n); 
			}
		}

		return neighbors; 
	}

	
	
	
	
	
	
	
	
	
	
	
	
	
	 */


}
