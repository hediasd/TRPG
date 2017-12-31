using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueCells : MonoBehaviour {

	public GameObject blueCell;
	public int radius;
	public float x, z;

	public void StartOver (float xa, float za, int rad){
		Startup(new Point(xa,za), rad, blueCell);
	}

	public void Feed(List<LinkedPoint> points, bool HSV = false, bool depther = false, float Max = 0.5f, float Min = 0.1f){
		if(HSV){
			float maxDepth = 0;
			float minDepth = 0;
			foreach (Point p in points)
			{
				if(p.Depth > maxDepth) maxDepth = p.Depth;
				else if(p.Depth < minDepth) minDepth = p.Depth;
			}
			for (int i = 0; i < points.Count; i++) //new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f)
			{
				GameObject blue = (GameObject)Instantiate (blueCell, new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f), Quaternion.Euler (90, 0, 0));
				blue.GetComponent<TransparencyShift>().enabled = false;
				HSVShift hsvs = blue.AddComponent<HSVShift>();
				float fd = ((points[i].Depth * 0.26f) - minDepth) / (maxDepth - minDepth);
				hsvs.Starter(false, new float[]{ fd, 1, 1});
				blue.transform.SetParent (this.transform, false);
			}
		}else{
			for (int i = 0; i < points.Count; i++) //new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f)
			{
				float Intensity = 0.1f * points[i].Parents.Count;
				GameObject blue = (GameObject)Instantiate (blueCell, new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f), Quaternion.Euler (90, 0, 0));
				blue.GetComponent<TransparencyShift>().Starter(false, MinValue : Intensity);
				blue.transform.SetParent (this.transform, false);
			}
		}

	}

	public void Feed(List<Point> points, bool HSV = false, bool depther = false, float Max = 0.5f, float Min = 0.1f){
		if(HSV){
			float maxDepth = 0;
			float minDepth = 0;
			foreach (Point p in points)
			{
				if(p.Depth > maxDepth) maxDepth = p.Depth;
				else if(p.Depth < minDepth) minDepth = p.Depth;
			}
			for (int i = 0; i < points.Count; i++) //new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f)
			{
				GameObject blue = (GameObject)Instantiate (blueCell, new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f), Quaternion.Euler (90, 0, 0));
				blue.GetComponent<TransparencyShift>().enabled = false;
				HSVShift hsvs = blue.AddComponent<HSVShift>();
				float fd = ((points[i].Depth * 0.26f) - minDepth) / (maxDepth - minDepth);
				hsvs.Starter(false, new float[]{ fd, 1, 1});
				blue.transform.SetParent (this.transform, false);
			}
		}else{
			for (int i = 0; i < points.Count; i++) //new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f)
			{
				float Intensity = (depther) ? points[i].Depth : 0.7f;
				GameObject blue = (GameObject)Instantiate (blueCell, new Vector3(points[i].x + 0.5f, 0.025f, points[i].z + 0.5f), Quaternion.Euler (90, 0, 0));
				blue.GetComponent<TransparencyShift>().Starter(false, MinValue : Intensity);
				blue.transform.SetParent (this.transform, false);
			}
		}

	}

	//public int[] 

	public void Startup (Point point, int Radius, GameObject CellExample) {
		
		x = point.x;
		z = point.z;
		radius = Radius;
		blueCell = CellExample;

		List<Point> fc = Environment.GetReachableCells(new Point(x, z), radius);
		for (int i = 0; i < fc.Count; i++)
		{
			GameObject blue = (GameObject)Instantiate (blueCell, new Vector3(fc[i].x + 0.5f, 0.025f, fc[i].z + 0.5f), Quaternion.Euler (90, 0, 0));
			blue.transform.SetParent (this.transform, false);
		}
		
	}

	public void CleanUp(){
		foreach (Transform child in transform) {
     		GameObject.Destroy(child.gameObject);
 		}
	}


	
}
