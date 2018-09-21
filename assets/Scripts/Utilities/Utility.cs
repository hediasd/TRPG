using System; //.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
//using UnityEngine;

public static class Utility {

	public static void Each<T> (this IEnumerable<T> items, Action<T> action) {
		foreach (var item in items) {
			action (item);
		}
	}

	public static string[] ChewUp (string s, string format) {
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

}