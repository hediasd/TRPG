using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using UnityEngine;

public class Utility : MonoBehaviour {


	public static string[] ChewUp(string s, string format){
		string[] r = Regex.Split(s, format);
		List<string> li = new List<string>();
		foreach (string t in r)
		{
			//if(len) if(t.Length < 2) continue;
			foreach (char c in t.ToCharArray())
			{
				if(c != ' '){
					li.Add(t);
					break;
				}
			}
			
		}
		//if(false) li.RemoveAt(0);
		return li.ToArray();
	}
}
