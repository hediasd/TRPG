using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Terrain : DataObject {

	public string Texture, PaletteA, PaletteB;
	
	[System.NonSerialized]
	public Color PaletteA_, PaletteB_;
	[System.NonSerialized]
	public Point Point;
	[System.NonSerialized]
	public Spell ResultOf;
	[System.NonSerialized]
	public Monster CreatedBy;
}
