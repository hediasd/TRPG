using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ResourcesMaster : MonoBehaviour {

	//public static List<Animation> Animations;
	public static List<SpellEntry> SpellEntries;
	public static List<StatusEntry> Statuses;
	public static List<MonsterEntry> MonsterEntries;
	public static List<MonsterEntry> CharacterEntries;
	public static List<TerrainEntry> Terrains;

	static GameObject CellSprite;

	void Start () {

		//Animations = new List<Animation>();
		CharacterEntries = new List<MonsterEntry> ();
		MonsterEntries = new List<MonsterEntry> ();
		SpellEntries = new List<SpellEntry> ();
		Statuses = new List<StatusEntry> ();
		Terrains = new List<TerrainEntry> ();

		TerrainLoader ();
		//Relog();
		SpellLoader ();
		//Relog();
		StatusChewUp ();
		//Relog();
		CharactersLoader ();
		//Relog();
		MonsterLoader ();
		//Relog();		
		UberDebug.LogChannel ("Resources", CharacterEntries.Count + " characters, " + MonsterEntries.Count + " monsters, " + SpellEntries.Count + " spells, " + Statuses.Count + " statuses, " + Terrains.Count + " terrains");
	}

	public static GameObject GetCellSprite () {
		if (CellSprite == null) {
			CellSprite = (GameObject) Resources.Load ("Prefabs/Piece Components/Cell Sprite", typeof (GameObject));
		}
		return CellSprite;
	}

	public static MonsterEntry GetMonsterEntry (string Name) {
		foreach (MonsterEntry mon in MonsterEntries) {
			if (mon.Name == Name) return mon;
		}
		return null;
	}

	public static SpellEntry GetSpellEntry (string Name) {
		foreach (SpellEntry spe in SpellEntries) {
			if (spe.Name == Name) return spe;
		}
		return null;
	}

	#region Loaders

	void TerrainLoader () {
		bool read = true;
		bool write = false;

		if (read) {
			TextAsset textAsset = (TextAsset) Resources.Load ("Texts/TerrainsJson", typeof (TextAsset));
			string line = textAsset.text;
			Terrains = WriteMaster.JsonToList<TerrainEntry> (line);
		}
		if (write) {
			string playerToJason = WriteMaster.ListToJson<TerrainEntry> (Terrains, true);
			WriteMaster.WriteUp ("TerrainsJson", playerToJason);
			//Debug.Log(playerToJason);
		}
		foreach (TerrainEntry trr in Terrains) {
			try {
				string[] ColorA = Utility.ChewUp (trr.PaletteA, ", ");
				string[] ColorB = Utility.ChewUp (trr.PaletteB, ", ");
				trr.PaletteA_ = new Color32 (byte.Parse (ColorA[0]), byte.Parse (ColorA[1]), byte.Parse (ColorA[2]), 255);
				trr.PaletteB_ = new Color32 (byte.Parse (ColorB[0]), byte.Parse (ColorB[1]), byte.Parse (ColorB[2]), 255);
			} catch (Exception) {
				trr.PaletteA_ = new Color32 (250, 250, 250, 255);
				trr.PaletteB_ = new Color32 (250, 250, 250, 255);
			}
		}
	}

	void CharactersLoader () {

		bool read = true;
		bool writeOriginal = true;
		bool writeCopy = true;

		if (read) {
			TextAsset textAsset = (TextAsset) Resources.Load ("Texts/CharactersJson", typeof (TextAsset));
			string line = textAsset.text;
			CharacterEntries = WriteMaster.JsonToList<MonsterEntry> (line);
		}

		CharacterEntries.Sort ((x, y) => x.Name.CompareTo (y.Name));

		foreach (MonsterEntry Entry in CharacterEntries) {
			Entry.Startup ();
		}

		if (writeOriginal) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (CharacterEntries, true);
			WriteMaster.WriteUp ("CharactersJson", playerToJason);
			Debug.Log (playerToJason);
		}
		if (writeCopy) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (CharacterEntries, true);
			WriteMaster.WriteUp ("CharactersJsonCopy", playerToJason);
			Debug.Log (playerToJason);
		}
		if (writeOriginal || writeCopy) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (CharacterEntries, true);
			WriteMaster.WriteUp ("Logs/CJC_" + DateTime.Now.ToString ("ddMMyy") + "_" + playerToJason.GetHashCode (), playerToJason);
		}

	}

	void SpellLoader () {

		bool read = true;
		bool write = true;

		if (read) {
			TextAsset textAsset = (TextAsset) Resources.Load ("Texts/SpellsJson", typeof (TextAsset));
			string line = textAsset.text;
			SpellEntries = WriteMaster.JsonToList<SpellEntry> (line);
		}

		foreach (SpellEntry sp in SpellEntries) {
			sp.Startup ();
			//Utility.Each(sp.DamageSegments, i => sp.Damages += (""+i.Value+"_"+i.Element));
		}

		if (write) {
			SpellEntries.Sort ((a, b) => a.Name.CompareTo (b.Name));
			string playerToJason = WriteMaster.ListToJson<SpellEntry> (SpellEntries, true);
			WriteMaster.WriteUp ("SpellsJson", playerToJason, true);
			//Debug.Log(playerToJason);
		}

		if (read || write) {
			string playerToJason = WriteMaster.ListToJson<SpellEntry> (SpellEntries, true);
			WriteMaster.WriteUp ("Logs/SJC_" + DateTime.Now.ToString ("ddMMyy") + "_" + playerToJason.GetHashCode (), playerToJason);
		}

	}

	void MonsterLoader () {
		//MonsterChewUp();

		bool read = true;
		bool writeOriginal = true;
		bool writeCopy = true;

		if (read) {
			TextAsset textAsset = (TextAsset) Resources.Load ("Texts/MonsterJson", typeof (TextAsset));
			string line = textAsset.text;
			MonsterEntries = WriteMaster.JsonToList<MonsterEntry> (line);
		}

		MonsterEntries.Sort ((x, y) => x.Name.CompareTo (y.Name));

		foreach (MonsterEntry Entry in MonsterEntries) {
			Entry.Startup ();
		}

		if (writeOriginal) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (MonsterEntries, true);
			WriteMaster.WriteUp ("MonsterJson", playerToJason);
			Debug.Log (playerToJason);
		}
		if (writeCopy) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (MonsterEntries, true);
			WriteMaster.WriteUp ("MonsterJsonCopy", playerToJason);
			Debug.Log (playerToJason);
		}
		if (writeOriginal || writeCopy) {
			string playerToJason = WriteMaster.ListToJson<MonsterEntry> (MonsterEntries, true);
			WriteMaster.WriteUp ("Logs/MJC_" + DateTime.Now.ToString ("ddMMyy") + "_" + playerToJason.GetHashCode (), playerToJason);
		}

	}

	void StatusChewUp () {

		TextAsset textAsset = (TextAsset) Resources.Load ("Texts/Status", typeof (TextAsset));
		string[] whole_text = Utility.ChewUp (textAsset.text, "\n");

		for (int z = 0; z < whole_text.Length - 1; z++) {

		}
	}

	#endregion Loaders

}