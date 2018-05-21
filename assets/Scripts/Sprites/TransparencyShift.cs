using UnityEngine;
using System.Collections;

public class TransparencyShift : MonoBehaviour {

	public float MaxValue, MinValue, Value;
	Color oldColor;
	bool growing;
	bool Dynamic = true;
	MeshRenderer MeshRenderer;

	public void Starter (bool dynamic, float MinValue = 0.1f, float MaxValue = 0.9f) {
		///MaxValue = 0.85f;
		//MinValue = 0.35f;
		Value = MinValue;
		Dynamic = dynamic;
		growing = false;
		MeshRenderer = GetComponent<MeshRenderer>();
		oldColor = MeshRenderer.material.color;
		MeshRenderer.material.color = new Color(oldColor.r,oldColor.g,oldColor.b, Value);
	}
	
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
}
