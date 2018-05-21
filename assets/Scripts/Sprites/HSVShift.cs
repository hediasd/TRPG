using UnityEngine;
using System.Collections;

public class HSVShift : MonoBehaviour {

	public float MaxValue, MinValue, Value;
	public Color Palette;
	bool growing;
	bool Dynamic = true;
	MeshRenderer MeshRenderer;
	float[] HSV = null;

	public void Starter (bool dynamic, float[] colors, float alpha = 0.4f) {
		///MaxValue = 0.85f;
		//MinValue = 0.35f;
		HSV = colors;
		Value = MinValue;
		Dynamic = dynamic;
		growing = false;
		MeshRenderer = GetComponent<MeshRenderer>();
		Palette = Color.HSVToRGB(colors[0], colors[1], colors[2]);
		MeshRenderer.material.color = new Color(Palette.r, Palette.g, Palette.b, alpha);

		Palette = MeshRenderer.material.color;
	}
	
	/*
	void Update () {
		if(Value > 1) Value = 1;
		if(Dynamic){
			oldColor = GetComponent<MeshRenderer>().material.color;
			Value = oldColor.a + (growing ? 0.01f : - 0.01f);
			if(Value >= MaxValue || Value <= MinValue){
				growing = !growing;
			}
			GetComponent<MeshRenderer>().material.color = new Color(oldColor.r,oldColor.g,oldColor.b, Value);
		}else{

		}
		
	
	}
	 */
}
