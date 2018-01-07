using UnityEngine;
using System;
using System.Collections.Generic; 
using System.Reflection;
using System.Globalization;

public class Grimoire : MonoBehaviour {

	public static List<Animation> Animations;
	public List<Spell> Spells; 
	public List<Status> Statuses;
	public List<Monster> Monsters; 
	public List<Monster> Characters; 
	public List<Terrain> Terrains; 

	string log;
	
	void Start () {

		Animations = new List<Animation>();
		Characters = new List<Monster>();
		Monsters = new List<Monster>();
		Spells = new List<Spell>();
		Statuses = new List<Status>();
		Terrains = new List<Terrain>();

		AnimationLoader();
		//Relog();
		TerrainLoader();
		SpellLoader();
		//Relog();
		StatusChewUp();
		//Relog();
		CharacterChewUp();
		//Relog();
		MonsterLoader();
		//Relog();		
	
	}

	public Monster GetMonster(string name){
		foreach (Monster mon in Monsters)
		{
			if(mon.Name == name) return mon;
		}
		return null;
	}





	
	void AnimationLoader(){
		//TODO:
		AnimationChewUp();
		//string playerToJason = WriteMaster.ListToJson<Animation>(Animations, true);
		//WriteMaster.WriteUp("AnimationsJson", playerToJason);
		//Debug.Log(playerToJason);

		//TextAsset textAsset = (TextAsset)Resources.Load("Texts/AnimationsJson", typeof(TextAsset)); 
		//string line = textAsset.text;
		//Animations = WriteMaster.JsonToList<Animation>(line);
		//Debug.Log(Spells.Count);

	}

	void TerrainLoader(){
		bool read = true;
		bool write = false;

		if(read){
			TextAsset textAsset = (TextAsset)Resources.Load("Texts/TerrainsJson", typeof(TextAsset)); 
			string line = textAsset.text;
			Terrains = WriteMaster.JsonToList<Terrain>(line);
		}
		if(write){
			string playerToJason = WriteMaster.ListToJson<Terrain>(Terrains, true);
			WriteMaster.WriteUp("TerrainsJson", playerToJason);
			Debug.Log(playerToJason);
		}
		foreach (Terrain trr in Terrains)
		{
			try{
				string[] ColorA = Utility.ChewUp(trr.PaletteA, ", ");
				string[] ColorB = Utility.ChewUp(trr.PaletteB, ", ");
				trr.PaletteA_ = new Color32(byte.Parse(ColorA[0]), byte.Parse(ColorA[1]), byte.Parse(ColorA[2]), 255);					
				trr.PaletteB_ = new Color32(byte.Parse(ColorB[0]), byte.Parse(ColorB[1]), byte.Parse(ColorB[2]), 255);						
			}catch (Exception){
				trr.PaletteA_ = new Color32(250, 250, 250, 255);					
				trr.PaletteB_ = new Color32(250, 250, 250, 255);						
			}
		}
	}

	void SpellLoader(){
		bool read = true;
		bool write = false;

		if(read){
			TextAsset textAsset = (TextAsset)Resources.Load("Texts/SpellsJson", typeof(TextAsset)); 
			string line = textAsset.text;
			Spells = WriteMaster.JsonToList<Spell>(line);
		}
		foreach (Spell sp in Spells)
		{
			if(sp.MaximumCastRange - sp.MinimumCastRange < 0) throw new Exception();
		}
		if(write){
			string playerToJason = WriteMaster.ListToJson<Spell>(Spells, true);
			WriteMaster.WriteUp("SpellsJson", playerToJason);
			Debug.Log(playerToJason);
		}
		if(read || write){
			string playerToJason = WriteMaster.ListToJson<Monster>(Monsters, true);
			WriteMaster.WriteUp("Logs/SJC_"+DateTime.Now.ToString("ddMMyyhhmm")+"_"+playerToJason.GetHashCode(), playerToJason);
		}
	}

	void MonsterLoader(){
		//MonsterChewUp();
		bool read = true;
		bool writeOriginal = false;
		bool writeCopy = true;

		if(read){
			TextAsset textAsset = (TextAsset)Resources.Load("Texts/MonsterJsonCopy", typeof(TextAsset)); 
			string line = textAsset.text;
			Monsters = WriteMaster.JsonToList<Monster>(line);
		}
		Monsters.Sort((x, y) => x.Name.CompareTo(y.Name));
		foreach (Monster mon in Monsters)
		{
			//
		}
		if(writeOriginal){
			string playerToJason = WriteMaster.ListToJson<Monster>(Monsters, true);
			WriteMaster.WriteUp("MonsterJson", playerToJason);
			Debug.Log(playerToJason);
		}
		if(writeCopy){
			string playerToJason = WriteMaster.ListToJson<Monster>(Monsters, true);
			WriteMaster.WriteUp("MonsterJsonCopy2", playerToJason);
			Debug.Log(playerToJason);
		}
		if(writeOriginal || writeCopy){
			string playerToJason = WriteMaster.ListToJson<Monster>(Monsters, true);
			WriteMaster.WriteUp("Logs/MJC_"+DateTime.Now.ToString("ddMMyyhhmm")+"_"+playerToJason.GetHashCode(), playerToJason);
		}

		foreach (Monster mon in Monsters) //TODO: This is awful
		{
			try{
				string[] ColorA = Utility.ChewUp(mon.PaletteA, ", ");
				string[] ColorB = Utility.ChewUp(mon.PaletteB, ", ");
				mon.PaletteA_ = new Color32(byte.Parse(ColorA[0]), byte.Parse(ColorA[1]), byte.Parse(ColorA[2]), 255);					
				mon.PaletteB_ = new Color32(byte.Parse(ColorB[0]), byte.Parse(ColorB[1]), byte.Parse(ColorB[2]), 255);						
			}catch (Exception){
				mon.PaletteA_ = new Color32(250, 250, 250, 255);					
				mon.PaletteB_ = new Color32(250, 250, 250, 255);						
			}
			
			string[] Values = Utility.ChewUp(mon.Stats, ", ");
			for (int i = 0; i < 10; i++)
			{
				mon.Stats_[i] = new Stat(byte.Parse(Values[i]));
			}

			foreach (string SpellName in Utility.ChewUp(mon.Spells, ", "))
			{
				foreach (Spell sp in Spells) 
				{
					if(SpellName.Equals(sp.Name)){
						mon.Spells_.Add(sp);
						break;
					}
				}
			}
		}

	}

	void AnimationChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Bright Lights Big City", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = Utility.ChewUp(lineless, "= ");

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = Utility.ChewUp(whole_text[z], "> ");
			//try{
				Animation animation = new Animation();

				string[] first_line = Utility.ChewUp(sections[0], @" \(|\)");
				animation.name = first_line[0];
				string[] texture = Utility.ChewUp(first_line[1], ", ");
				//anim.sheet = texture[0];
				//Debug.Log(anim.sheet);
				
				/*
				string[] colorA = Utility.ChewUp(texture[0], "_", false);
				animation.paletteA = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				string[] colorB = Utility.ChewUp(texture[0], "_", false);
				animation.paletteB = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								
				*/

				string[] second_line = Utility.ChewUp(sections[1], @", ");
				//anim.type = Thesaurus.Chew(second_line[0]);
				//int amount = int.Parse(second_line[0]);

				for (int i = 2; i < 3; i++) //amount+2
				{
					string[] sfx_line = Utility.ChewUp(sections[i], @", ");
					SfxSpriteAnimation sfx = new SfxSpriteAnimation();
					sfx.Sheetname = sfx_line[0];
					for (int j = 1; j < sfx_line.Length; j++)
					{
						string[] state = Utility.ChewUp(sfx_line[j], @"_");
						switch(state[0]){
							case "Frames":
								sfx.FirstFrame = int.Parse(state[1]);
								sfx.LastFrame = int.Parse(state[2]);
								break;
							case "Step":
								sfx.Step = true;
								break;
							case "SpawnInterval":
								sfx.SpawnInterval = float.Parse(state[1], CultureInfo.InvariantCulture);
								break;
							case "Type":
								sfx.Type = Thesaurus.Chew(state[1]);
								sfx.Shape = Thesaurus.Chew(state[2]);
								break;
							default:
								try{
									int data = int.Parse(state[1]);
									var esp = sfx.GetType().GetField(state[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
									esp.SetValue(sfx, data);
								}catch{
//									Alog("SfxNullRef " + state[0]);
								}							
								//Debug.Log("SpellSpec Error " + state[0] + " at " + spell.name);
								break;	
						}						
					}
					//...

					animation.EffectList.Add(sfx);
					//animation.EffectList[0] = (sfx);
				}

				/*foreach (string s in second_line){
					Spell sp = new Spell();
					sp.name = s;
					character.AddSpell(sp);
				}*/

				Animations.Add(animation);
				
			//}catch{}
				//Debug.Log("Error Monster Utility.ChewUp");
			
		}		
	}
	
	void CharacterChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Salad", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = Utility.ChewUp(lineless, "= ");

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = Utility.ChewUp(whole_text[z], "> ");
			//try{
				Monster character = new Monster();

				string[] first_line = Utility.ChewUp(sections[0], @" \[|]|\(|\)");
				character.Name = first_line[0];
				string[] texture = Utility.ChewUp(first_line[1], ", ");
				character.Texture = texture[0];
				
				string[] colorA = Utility.ChewUp(texture[1], "_");
				character.PaletteA_ = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				string[] colorB = Utility.ChewUp(texture[2], "_");
				character.PaletteB_ = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								

				string[] second_line = Utility.ChewUp(sections[1], @", ");
				int[] stats = Array.ConvertAll(second_line, s => int.Parse(s));

				character.Stats_[0] = new Stat(stats[0]);
				character.Stats_[1] = new Stat(stats[1]);
				
				/*monster.POW_ = stats[2];
				monster.MGT_ = stats[3];
				monster.END_ = stats[4];
				monster.RES_ = stats[5];
				monster.SPD_ = stats[6];
				monster.LUK_ = stats[7];
				monster.MOV_ = stats[8];
				*/
				
				string[] fourth_line = Utility.ChewUp(sections[3], @", ");
				foreach (string s in fourth_line){
					Spell sp = new Spell();
					sp.Name = s;
					character.AddSpell(sp);
				}

				Characters.Add(character);
				
			//}catch{}
				//Debug.Log("Error Monster Utility.ChewUp");
			
		}		
	}

	
	void MonsterChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Plaything", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = Utility.ChewUp(lineless, "= ");

		for (int z = 1; z < whole_text.Length-1; z++) { //
			string[] sections = Utility.ChewUp(whole_text[z], "> ");
				Monster monster = new Monster();
			try{	
				string[] first_line = Utility.ChewUp(sections[0], @" \[|]|\(|\)");
				monster.Name = first_line[0];
				string[] texture = Utility.ChewUp(first_line[2], ", ");
				monster.Texture = texture[0];
				
				try{
					string[] colorA = Utility.ChewUp(texture[1], "_");
					monster.PaletteA_ = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				//}catch{}
				//try{
					string[] colorB = Utility.ChewUp(texture[2], "_");
					monster.PaletteB_ = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								
				
					monster.PaletteA = colorA[0]+", "+colorA[1]+", "+colorA[2];
					monster.PaletteB = colorB[0]+", "+colorB[1]+", "+colorA[2];
				
				}catch{}				

				string[] second_line = Utility.ChewUp(sections[1], @", ");
				int[] stats = Array.ConvertAll(second_line, s => int.Parse(s));
				
				try{
					for (int i = 0; i < 9; i++){
						monster.Stats_[i] = new Stat(stats[i]);
					}
				}
				catch (Exception)
				{
				}

				/*monster.POW_ = stats[2];
				monster.MGT_ = stats[3];
				monster.END_ = stats[4];
				monster.RES_ = stats[5];
				monster.SPD_ = stats[6];
				monster.LUK_ = stats[7];
				monster.MOV_ = stats[8];
				*/

				string[] third_line = Utility.ChewUp(sections[2], @", ");

				string[] fourth_line = Utility.ChewUp(sections[3], @", ");
				foreach (string s in fourth_line)
				{
					bool not = true;
					foreach (Spell p in Spells)
					{
						if(p.Name == s){
							not = false;
							monster.AddSpell(p);
							break;
						}else{
						}	
					}
				//	if(not) Alog("No Spell " + s);
				}
				Monsters.Add(monster);
			}
			catch (Exception){
				//Debug.Log("Error at " + monster.name + e.StackTrace);
			}
		}		
	}

	/*
	void SpellChewUp(){
		
		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Through the Bridge of Light", typeof(TextAsset)); 
		string[] elements = Utility.ChewUp(textAsset.text, ":", true);

		for (int z = 0; z < elements.Length-1; z+=2)
		{ //

			string[] elem = Utility.ChewUp(elements[z], @"\+", true);
			string e_content = elements[z+1].Replace('"', '#');
			e_content = e_content.Replace("# -", "#");//.Remove(0, 3);
			string[] e_spells = Utility.ChewUp(e_content,  @"= |\r|\n\r", true); // @"#|\[|]|\n|\r|\n\r"
			
			for (int w = 1; w < e_spells.Length; w+=4)
			{
				try{
				
					Spell spell = new Spell();
					foreach (string el in elem)
					{
						spell.Elements.Add(Thesaurus.Chew(el.ToLower()));
					}
					//spell.elements = elem;
					string[] name_desc = Utility.ChewUp(e_spells[w], @"#", true);
					spell.name = name_desc[0];					
					//spell.description = name_desc[1];

					string[] first_line = Utility.ChewUp(e_spells[w+1], "> |, ", true, eatFirst: true);
					foreach (string statement in first_line)
					{
						//Debug.Log(statement);
						string[] state = Utility.ChewUp(statement, "_", false);
						int data = 0;
						try{
							if(state.Length > 1) data = int.Parse(state[1]);
						}catch{}
						switch(state[0]){
							case "All":
							case "Allies":
							case "Enemies":
							case "None":
								spell.targeter = (int) typeof(E).GetField(state[0].ToUpper()).GetValue(null);
								break;
							case "CastShape":
								spell.CastShape = Thesaurus.Chew(state[1]);
								break;
							case "EffectShape":
								spell.EffectShape = Thesaurus.Chew(state[1]);
								break;
							case "Animation":
								foreach (Animation a in Animations)
								{	
									if(a.name.Equals(state[1])){
										spell.AnimationName = a.name;
										break;
									}
								}
								break;
							default:
								try{
									var esp = spell.GetType().GetField(state[0].ToLower(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
									esp.SetValue(spell, data);
								}catch{
									Alog("SpellNullRef " + state[0]);
								}							
								//Debug.Log("SpellSpec Error " + state[0] + " at " + spell.name);
								break;						
						}
					}

					string[] second_line = Utility.ChewUp(e_spells[w+2], "> |, ", true, eatFirst: true);
					foreach (string statement in second_line)
					{
						int a = 0, b = 0, c = 0, d = 0;
						string[] state = Utility.ChewUp(statement, "_", false);

						try
						{
							if(state.Length > 1) b = int.Parse(state[1]);
						}
						catch (FormatException)	{}
						if(state.Length > 2) {
							c = int.Parse(state[2]);
							if(state.Length > 3) d = int.Parse(state[3]);
						}											
												
						switch(state[0]){
							case "Damage":
								a = E.DAMAGE;
								b = Thesaurus.Chew(state[1]);
								break;
							case "Poison":
								a = E.POISON;
								b = Thesaurus.Chew(state[1]);
								break;
							default:
								a = (int) typeof(E).GetField(state[0].ToUpper()).GetValue(null);
								if(a == 0) Alog("SpellProp Error " + state[0] + " at " + spell.name);
								break;
						}

						Property pr = new Property(a, b, c, d);
						//Debug.Log(pr.ToString());
						spell.Properties.Add(pr);
					}
					
					Spells.Add(spell);
					
				}catch(Exception e){
					//Alog("Exception");
					//Debug.Log(e.);
					//Debug.Log("Threw");
				}
			}
			
				
			
		}		
	}
	*/

	void StatusChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Status", typeof(TextAsset)); 
		string[] whole_text = Utility.ChewUp(textAsset.text, "\n"); 

		for (int z = 0; z < whole_text.Length-1; z++) {
			
			
			
		}
	}


}
