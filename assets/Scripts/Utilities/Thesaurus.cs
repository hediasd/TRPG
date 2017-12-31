using UnityEngine;
using System.Collections;

public class Thesaurus {
	
	public static int Chew(string s){
		string st = s.ToLower();
		switch(st){
			case "darkness":
				return E.DARKNESS;
			case "earth":
				return E.EARTH;
			case "fire":
				return E.FIRE;			 
			case "ice":
				return E.ICE;
			case "light":
				return E.LIGHT;			 			
			case "steel":
				return E.STEEL;			 			
			case "thunder":
				return E.THUNDER;			 			
			case "water":
				return E.WATER;			 			
			case "wind":
				return E.WIND;			 			
			case "wood":
				return E.WOOD;	

			case "linear":
				return E.LINE;
			case "trilinear":
				return E.TRILINE;
			case "horizontalline":
				return E.HORIZONTALLINE;
			case "verticalline":
				return E.VERTICALLINE;
			case "circle":
				return E.CIRCLE;
			case "square":
				return E.SQUARE;
			case "cone":
				return E.CONE;

			case "simplesingle":
				return E.SIMPLESINGLE;
			case "waves":
				return E.WAVES;


			default:
				Debug.Log("ThesaurusError " + s);
				break;

		}

		return 0;
	}

}
