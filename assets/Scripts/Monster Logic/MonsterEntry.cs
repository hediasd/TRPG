using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

[System.Serializable]
public class MonsterEntry : DataEntry {

	public string Texture;

	[System.NonSerialized]
	public List<SpellEntry> SpellEntries; // = new List<Spell>();
	[System.NonSerialized]
	public List<SpellEntry> PassivesList;

	//public List<string> SpellNames;// = new List<string>();
	List<string> StatusNames; // = new List<string>();

	public string Races, Spells, Passives, Stats, PaletteA, PaletteB;

	[System.NonSerialized]
	public Color ColorPaletteA, ColorPaletteB;
	[System.NonSerialized]
	public MonsterStats StatsList; //TODO: 9
	[System.NonSerialized]
	public List<int> RacesList;

	public MonsterEntry () {
		SpellEntries = new List<SpellEntry> ();
		PassivesList = new List<SpellEntry> ();
		StatsList = new MonsterStats ();
		RacesList = new List<int> ();


	}

	public MonsterInstance Instantiate () {
		return new MonsterInstance (this);
	}

	public override void Startup () {

		try {
			string[] ColorA = Utility.ChewUp (PaletteA, ", ");
			string[] ColorB = Utility.ChewUp (PaletteB, ", ");
			ColorPaletteA = new Color32 (byte.Parse (ColorA[0]), byte.Parse (ColorA[1]), byte.Parse (ColorA[2]), 255);
			ColorPaletteB = new Color32 (byte.Parse (ColorB[0]), byte.Parse (ColorB[1]), byte.Parse (ColorB[2]), 255);
		} catch (Exception) {
			ColorPaletteA = new Color32 (250, 250, 250, 255);
			ColorPaletteB = new Color32 (250, 250, 250, 255);
		}

		string[] Values = Utility.ChewUp (Stats, ", ");
		int[] IntValues = new int[10];
		for (int i = 0; i < 10; i++) {
			IntValues[i] = (int.Parse (Values[i]));
		}
		StatsList = new MonsterStats (IntValues);

		foreach (string Race in Utility.ChewUp (Races, ", ")) {
			RacesList.Add (RACE.GetRaceValue (Race));
		}

		foreach (string SpellName in Utility.ChewUp (Spells, ", ")) {
			SpellEntry SE = ResourcesMaster.GetSpellEntry (SpellName);
			SpellEntries.Add (SE);
		}

	}

}