using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour{

	public string Name;
	public List<SfxSpriteAnimation> EffectList;	
	//public SfxSpriteAnimation[] EffectList;
	public Color paletteA, paletteB;
	
	public Animation(){
		EffectList = new List<SfxSpriteAnimation>();
		//EffectList = new SfxSpriteAnimation[2];
	}
}
