using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainHolder : MonoBehaviour {

	public Color TerrainColorA, TerrainColorB;
	public TerrainEntry Terrain;

	void Start () {
		TerrainColorA = Terrain.PaletteA_;
		TerrainColorB = Terrain.PaletteB_;
	}
	
}