using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSpriteAnimation : MonoBehaviour {

	public string Kind = "", Sheetname = "";
    [Range(0.0f, 1.0f)]
    public float r, g, b, a;
    public List<Sprite> sprites;
    public SpriteRenderer sr;
    public string[] names;

	public bool Step = false;
	public int MaxLoops = 1, Looped = 0, FirstFrame = 0, LastFrame = -1;
	public float Timer = 0, FrameTimer = 0.1f, SpawnInterval = 0;
	public int CurrentFrame = 0, FrameAmount = 0;

	public void Startup () {
		
		Startstuff();

		Sprite[] spritearray = Resources.LoadAll<Sprite>(string.Format(Kind+"/"+Sheetname));
		sprites = new List<Sprite>(spritearray);
        FrameAmount = sprites.Count;
        sr = GetComponent<SpriteRenderer>();
        names = new string[sprites.Count];

        for(int i = 0; i < names.Length; i++) 
        {
            names[i] = sprites[i].name;
        }

        r = g = b = a = 1.0f;
        if(LastFrame == -1) LastFrame = FrameAmount;
        CurrentFrame = FirstFrame;

	}
	
	public virtual void Startstuff () {
	}
	
}
