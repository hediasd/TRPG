using UnityEngine;

public class TerrainHolder : MonoBehaviour {

	public Color TerrainColorA, TerrainColorB;
	public TerrainEntry Terrain;

	void Start () {
		TerrainColorA = Terrain.PaletteA_;
		TerrainColorB = Terrain.PaletteB_;
	}

	void Update () {
		if (Time.frameCount % TimeMaster.GeneralFrameWaitingInterval == 0) { }
	}

}