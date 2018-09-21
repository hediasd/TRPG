using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainEntry : DataObject {

	public string Texture, PaletteA, PaletteB;
	
	[System.NonSerialized]
	public Color PaletteA_, PaletteB_;
	[System.NonSerialized]
	public Point Point;
	[System.NonSerialized]
	public SpellEntry ResultOf;
	[System.NonSerialized]
	public MonsterInstance CreatedBy;
}
