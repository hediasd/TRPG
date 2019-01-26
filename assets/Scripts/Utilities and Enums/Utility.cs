using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utility {

	public static string[] ChewUp (string s, string format = "_|, ") {
		//char[] formated = Regex.Split(format, "|");
		string[] r = Regex.Split (s, format);
		List<string> li = new List<string> ();
		foreach (string t in r) {
			//if(len) if(t.Length < 2) continue;
			foreach (char c in t.ToCharArray ()) {
				if (c != ' ') {
					li.Add (t);
					break;
				}
			}

		}
		//if(false) li.RemoveAt(0);
		return li.ToArray ();
	}

	public static void Each<T> (this IEnumerable<T> items, Action<T> action) {
		foreach (var item in items) {
			action (item);
		}
	}

	public static GameObject InstantiateGameObject (string Name, GameObject Base, Vector3 Position, Quaternion Rotation, Transform Parent){
		//string NewName = 
		if(Base == null){
			GameObject GO = new GameObject();
			GO.name = Name;
			GO.transform.SetParent(Parent);
			GO.transform.position = Position;
			GO.transform.rotation = Rotation;
			return GO;
		}else{
			GameObject GO = (GameObject) UnityEngine.Object.Instantiate(Base, Position, Rotation, Parent);
			GO.name = Name;
			return GO;
		}

	}

}