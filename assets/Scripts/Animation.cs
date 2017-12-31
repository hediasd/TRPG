using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation {

	public string name;
	public List<SfxSpriteAnimation> EffectList;	
	//public SfxSpriteAnimation[] EffectList;
	public Color paletteA, paletteB;
	
	public Animation(){
		EffectList = new List<SfxSpriteAnimation>();
		//EffectList = new SfxSpriteAnimation[2];
	}
}
