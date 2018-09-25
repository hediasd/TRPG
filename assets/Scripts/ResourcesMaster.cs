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
	public static List<MonsterEntry> Characters;
	public static List<TerrainEntry> Terrains;

	void Start () {

		//Animations = new List<Animation>();
		Characters = new List<MonsterEntry> ();
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
		CharacterChewUp ();
		//Relog();
		MonsterLoader ();
		//Relog();		
		UberDebug.LogChannel ("Resources", Characters.Count + " characters, " + MonsterEntries.Count + " monsters, " + SpellEntries.Count + " spells, " + Statuses.Count + " statuses, " + Terrains.Count + " terrains");
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

	void AnimationLoader () {

		// TODO:

		AnimationChewUp ();
		//string playerToJason = WriteMaster.ListToJson<Animation>(Animations, true);
		//WriteMaster.WriteUp("AnimationsJson", playerToJason);
		//Debug.Log(playerToJason);

		//TextAsset textAsset = (TextAsset)Resources.Load("Texts/AnimationsJson", typeof(TextAsset)); 
		//string line = textAsset.text;
		//Animations = WriteMaster.JsonToList<Animation>(line);
		//Debug.Log(Spells.Count);

	}

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
		bool writeOriginal = false;
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

	void AnimationChewUp () {

		TextAsset textAsset = (TextAsset) Resources.Load ("Texts/Bright Lights Big City", typeof (TextAsset));
		string lineless = textAsset.text.Replace (System.Environment.NewLine, "");
		string[] whole_text = Utility.ChewUp (lineless, "= ");

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = Utility.ChewUp (whole_text[z], "> ");
			//try{
			Animation animation = new Animation ();

			string[] first_line = Utility.ChewUp (sections[0], @" \(|\)");
			animation.name = first_line[0];
			string[] texture = Utility.ChewUp (first_line[1], ", ");
			//anim.sheet = texture[0];
			//Debug.Log(anim.sheet);

			/*
			string[] colorA = Utility.ChewUp(texture[0], "_", false);
			animation.paletteA = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
			string[] colorB = Utility.ChewUp(texture[0], "_", false);
			animation.paletteB = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								
			*/

			string[] second_line = Utility.ChewUp (sections[1], @", ");
			//anim.type = Thesaurus.Chew(second_line[0]);
			//int amount = int.Parse(second_line[0]);

			for (int i = 2; i < 3; i++) //amount+2
			{
				string[] sfx_line = Utility.ChewUp (sections[i], @", ");
				SfxSpriteAnimation sfx = new SfxSpriteAnimation ();
				sfx.Sheetname = sfx_line[0];
				for (int j = 1; j < sfx_line.Length; j++) {
					string[] state = Utility.ChewUp (sfx_line[j], @"_");
					switch (state[0]) {
						case "Frames":
							sfx.FirstFrame = int.Parse (state[1]);
							sfx.LastFrame = int.Parse (state[2]);
							break;
						case "Step":
							sfx.Step = true;
							break;
						case "SpawnInterval":
							sfx.SpawnInterval = float.Parse (state[1], CultureInfo.InvariantCulture);
							break;
						case "Type":
							sfx.Type = Thesaurus.Chew (state[1]);
							sfx.Shape = Thesaurus.Chew (state[2]);
							break;
						default:
							try {
								int data = int.Parse (state[1]);
								var esp = sfx.GetType ().GetField (state[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
								esp.SetValue (sfx, data);
							} catch { }
							//Debug.Log("SpellSpec Error " + state[0] + " at " + spell.name);
							break;
					}
				}
				//...

				animation.EffectList.Add (sfx);
				//animation.EffectList[0] = (sfx);
			}

			/*foreach (string s in second_line){
				Spell sp = new Spell();
				sp.name = s;
				character.AddSpell(sp);
			}*/

			////////Animations.Add(animation);

			//}catch{}
			//Debug.Log("Error Monster Utility.ChewUp");

		}
	}

	void CharacterChewUp () {

		TextAsset textAsset = (TextAsset) Resources.Load ("Texts/Salad", typeof (TextAsset));
		string lineless = textAsset.text.Replace (System.Environment.NewLine, "");
		string[] whole_text = Utility.ChewUp (lineless, "= ");

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = Utility.ChewUp (whole_text[z], "> ");
			//try{
			MonsterEntry character = new MonsterEntry ();

			string[] first_line = Utility.ChewUp (sections[0], @" \[|]|\(|\)");
			character.Name = first_line[0];
			string[] texture = Utility.ChewUp (first_line[1], ", ");
			character.Texture = texture[0];

			string[] colorA = Utility.ChewUp (texture[1], "_");
			character.ColorPaletteA = new Color32 (byte.Parse (colorA[0]), byte.Parse (colorA[1]), byte.Parse (colorA[2]), 255);
			string[] colorB = Utility.ChewUp (texture[2], "_");
			character.ColorPaletteB = new Color32 (byte.Parse (colorB[0]), byte.Parse (colorB[1]), byte.Parse (colorB[2]), 255);

			string[] second_line = Utility.ChewUp (sections[1], @", ");
			int[] stats = Array.ConvertAll (second_line, s => int.Parse (s));

			//string[] Values = Utility.ChewUp(mon.Stats, ", ");
			//int[] IntValues = new int[10];
			//for (int i = 0; i < 10; i++)
			//{
			//	IntValues[i] = (int.Parse(stats[i]));
			//}
			character.StatsList = new MonsterStats (stats);

			string[] fourth_line = Utility.ChewUp (sections[3], @", ");
			foreach (string s in fourth_line) {
				SpellEntry sp = new SpellEntry ();
				sp.Name = s;
				//TODO: character.AddSpell (sp);
			}

			Characters.Add (character);

			//}catch{}
			//Debug.Log("Error Monster Utility.ChewUp");

		}
	}

	void StatusChewUp () {

		TextAsset textAsset = (TextAsset) Resources.Load ("Texts/Status", typeof (TextAsset));
		string[] whole_text = Utility.ChewUp (textAsset.text, "\n");

		for (int z = 0; z < whole_text.Length - 1; z++) {

		}
	}

}