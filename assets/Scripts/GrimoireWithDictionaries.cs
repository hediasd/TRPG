/*  
using UnityEngine;
using System;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 
using System.Reflection;
using System.Globalization;

public class Grimoire : MonoBehaviour {

	public static List<Animation> Animations;
	public Dictionary<string, Spell> Spells; 
	public Dictionary<int, Monster> Monsters; 
	public List<Monster> Characters; 
	public List<Status> Statuses;

	string log;
	
	void Start () {

		Animations = new List<Animation>();
		Characters = new List<Monster>();
		Monsters = new Dictionary<int, Monster>();
		Spells = new Dictionary<string, Spell>();
		Statuses = new List<Status>();

		AnimationLoader();
		//Relog();
		SpellLoader();
		//Relog();
		StatusChewUp();
		//Relog();
		CharacterChewUp();
		//Relog();
		MonsterLoader();
		//Relog();		

		//Debug.Log("Grimoire Counts: "+ Animations.Count + " " + Characters.Count
		// + " " + Monsters.Count + " " + Spells.Count + " " + Statuses.Count);

		//Debug.Log("Animations: " + Animations.Count + " Characters: " +
		//Characters.Count + " Monsters: " + Monsters.Count +
		//" Spells: " + Spells.Count + " Statuses: " + S
		/*
		string playerToJason = JsonHelper.ToJson<Monster>(Monsters, true);
		JsonHelper.WriteUp("Output", playerToJason);
		Debug.Log(playerToJason);

		string goToJason = JsonHelper.ToJson<Spell>(Spells, true);
		JsonHelper.WriteUp("Output2", goToJason);
		Debug.Log(goToJason);
	
	
	

	public void Alog(string s){
		log += s + ", ";
	}
	public void Relog(){
		log += "\n";
		Debug.Log(log);
		log = "";
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

	void SpellLoader(){
		bool read = true;
		bool write = true;

		if(read){
			TextAsset textAsset = (TextAsset)Resources.Load("Texts/SpellsJson", typeof(TextAsset)); 
			string line = textAsset.text;
			List<Spell> SpellList = WriteMaster.JsonToList<Spell>(line);
			foreach (Spell sp in SpellList)
			{
				Spells.Add(sp.name, sp);
			}
		}
		//foreach (Spell sp in Spells)
		//{
		//	sp.DamageSegments.Add(new DamageSegment(1, 10));
		//}
		if(write){
			List<Spell> SpellList = new List<Spell>(Spells.Values);
			string playerToJason = WriteMaster.ListToJson<Spell>(SpellList, true);
			WriteMaster.WriteUp("SpellsJson", playerToJason);
			Debug.Log(playerToJason);
		}
	}

	void MonsterLoader(){
		//MonsterChewUp();
		bool read = true;
		bool write = true;

		if(read){
			TextAsset textAsset = (TextAsset)Resources.Load("Texts/MonsterJson", typeof(TextAsset)); 
			string line = textAsset.text;
			List<Monster> MonsterList = WriteMaster.JsonToList<Monster>(line);
			int i = 0;
			foreach (Monster mon in MonsterList)
			{
				Monsters.Add(i, mon);
				i += 1;
			}
		}
		if(write){
			List<Monster> MonsterList = new List<Monster>(Monsters.Values);
			string playerToJason = WriteMaster.ListToJson<Monster>(MonsterList, true);
			WriteMaster.WriteUp("MonsterJson", playerToJason);
			Debug.Log(playerToJason);
		}

		foreach (Monster mon in Monsters.Values)
		{
			foreach (string SpellName in mon.SpellNames)
			{
				//mon.Spells
			}
		}
		//Debug.Log(Spells.Count);
	}

	string[] ChewUp(string s, string format, bool len, bool eatFirst = false){
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
		if(eatFirst) li.RemoveAt(0);
		return li.ToArray();
	}

	void AnimationChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Bright Lights Big City", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = ChewUp(lineless, "= ", true);

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = ChewUp(whole_text[z], "> ", true);
			//try{
				Animation animation = new Animation();

				string[] first_line = ChewUp(sections[0], @" \(|\)", true);
				animation.name = first_line[0];
				string[] texture = ChewUp(first_line[1], ", ", true);
				//anim.sheet = texture[0];
				//Debug.Log(anim.sheet);
				
				/*
				string[] colorA = ChewUp(texture[0], "_", false);
				animation.paletteA = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				string[] colorB = ChewUp(texture[0], "_", false);
				animation.paletteB = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								
				

				string[] second_line = ChewUp(sections[1], @", ", true);
				//anim.type = Thesaurus.Chew(second_line[0]);
				//int amount = int.Parse(second_line[0]);

				for (int i = 2; i < 3; i++) //amount+2
				{
					string[] sfx_line = ChewUp(sections[i], @", ", true);
					SfxSpriteAnimation sfx = new SfxSpriteAnimation();
					sfx.Sheetname = sfx_line[0];
					for (int j = 1; j < sfx_line.Length; j++)
					{
						string[] state = ChewUp(sfx_line[j], @"_", true);
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
									Alog("SfxNullRef " + state[0]);
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
				}

				Animations.Add(animation);
				
			//}catch{}
				//Debug.Log("Error Monster Chewup");
			
		}		
	}
	
	void CharacterChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Salad", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = ChewUp(lineless, "= ", true);

		for (int z = 1; z < whole_text.Length; z++) { //
			string[] sections = ChewUp(whole_text[z], "> ", true);
			//try{
				Monster character = new Monster();

				string[] first_line = ChewUp(sections[0], @" \[|]|\(|\)", true);
				character.name = first_line[0];
				string[] texture = ChewUp(first_line[1], ", ", true);
				character.texture = texture[0];
				
				string[] colorA = ChewUp(texture[1], "_", false);
				character.PaletteA = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				string[] colorB = ChewUp(texture[2], "_", false);
				character.PaletteB = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								

				string[] second_line = ChewUp(sections[1], @", ", true);
				int[] stats = Array.ConvertAll(second_line, s => int.Parse(s));

				character.Stats[0] = new Stat(stats[0]);
				character.Stats[1] = new Stat(stats[1]);
				
				/*monster.POW_ = stats[2];
				monster.MGT_ = stats[3];
				monster.END_ = stats[4];
				monster.RES_ = stats[5];
				monster.SPD_ = stats[6];
				monster.LUK_ = stats[7];
				monster.MOV_ = stats[8];
				
				
				string[] fourth_line = ChewUp(sections[3], @", ", true);
				foreach (string s in fourth_line){
					Spell sp = new Spell();
					sp.name = s;
					character.AddSpell(sp);
				}

				Characters.Add(character);
				
			//}catch{}
				//Debug.Log("Error Monster Chewup");
			
		}		
	}

	/*
	void MonsterChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Plaything", typeof(TextAsset)); 
		string lineless = textAsset.text.Replace(System.Environment.NewLine, "");
		string[] whole_text = ChewUp(lineless, "= ", true);

		for (int z = 1; z < whole_text.Length-1; z++) { //
			string[] sections = ChewUp(whole_text[z], "> ", true);
				Monster monster = new Monster();
			try{	
				string[] first_line = ChewUp(sections[0], @" \[|]|\(|\)", true);
				monster.name = first_line[0];
				string[] texture = ChewUp(first_line[2], ", ", true);
				monster.texture = texture[0];
				
				try{
					string[] colorA = ChewUp(texture[1], "_", false);
					monster.PaletteA = new Color32(byte.Parse(colorA[0]), byte.Parse(colorA[1]), byte.Parse(colorA[2]), 255);					
				//}catch{}
				//try{
					string[] colorB = ChewUp(texture[2], "_", false);
					monster.PaletteB = new Color32(byte.Parse(colorB[0]), byte.Parse(colorB[1]), byte.Parse(colorB[2]), 255);								
				}catch{}				

				string[] second_line = ChewUp(sections[1], @", ", true);
				int[] stats = Array.ConvertAll(second_line, s => int.Parse(s));
				
				try{
					for (int i = 0; i < 9; i++){
						monster.Stats[i] = new Stat(stats[i]);
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
				

				string[] third_line = ChewUp(sections[2], @", ", true);

				string[] fourth_line = ChewUp(sections[3], @", ", true);
				foreach (string s in fourth_line)
				{
					bool not = true;
					foreach (Spell p in Spells)
					{
						if(p.name == s){
							not = false;
							monster.AddSpell(p);
							break;
						}else{
						}	
					}
					if(not) Alog("No Spell " + s);
				}
				Monsters.Add(monster);
			}
			catch (Exception){
				//Debug.Log("Error at " + monster.name + e.StackTrace);
			}
		}		
	}
	*/

	/*
	void SpellChewUp(){
		
		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Through the Bridge of Light", typeof(TextAsset)); 
		string[] elements = ChewUp(textAsset.text, ":", true);

		for (int z = 0; z < elements.Length-1; z+=2)
		{ //

			string[] elem = ChewUp(elements[z], @"\+", true);
			string e_content = elements[z+1].Replace('"', '#');
			e_content = e_content.Replace("# -", "#");//.Remove(0, 3);
			string[] e_spells = ChewUp(e_content,  @"= |\r|\n\r", true); // @"#|\[|]|\n|\r|\n\r"
			
			for (int w = 1; w < e_spells.Length; w+=4)
			{
				try{
				
					Spell spell = new Spell();
					foreach (string el in elem)
					{
						spell.Elements.Add(Thesaurus.Chew(el.ToLower()));
					}
					//spell.elements = elem;
					string[] name_desc = ChewUp(e_spells[w], @"#", true);
					spell.name = name_desc[0];					
					//spell.description = name_desc[1];

					string[] first_line = ChewUp(e_spells[w+1], "> |, ", true, eatFirst: true);
					foreach (string statement in first_line)
					{
						//Debug.Log(statement);
						string[] state = ChewUp(statement, "_", false);
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

					string[] second_line = ChewUp(e_spells[w+2], "> |, ", true, eatFirst: true);
					foreach (string statement in second_line)
					{
						int a = 0, b = 0, c = 0, d = 0;
						string[] state = ChewUp(statement, "_", false);

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
	

	void StatusChewUp(){

		TextAsset textAsset = (TextAsset)Resources.Load("Texts/Status", typeof(TextAsset)); 
		string[] whole_text = Regex.Split(textAsset.text, "\n"); 

		for (int z = 0; z < whole_text.Length-1; z++) {
			
			
			
		}
	}


}
 */