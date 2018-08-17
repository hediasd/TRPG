using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpin : BaseParticlesAction {

	public Vector3 FinalRotation;
	Quaternion TargetRotation;
	public int Duration;

	float t = 0.0001f;

	public override void Play () {
		//Piece.ParticleFly (PieceSpell.CastedFrom, PieceSpell.CastedTo);
		for (int i = 0; i < 3; i++)
		{
			if(FinalRotation[i] < 0) FinalRotation[i] = 360 - FinalRotation[i];
		}
		TargetRotation = Quaternion.Euler(FinalRotation);
		//TargetRotation *= Quaternion.AngleAxis (FinalRotation.x, Vector3.right);
		//TargetRotation *= Quaternion.AngleAxis (FinalRotation.y, Vector3.up);
		//TargetRotation *= Quaternion.AngleAxis (FinalRotation.z, Vector3.forward);

		//Debug.Log("fnal " + FinalRotation);
		//Debug.Log("tqrget " + TargetRotation + " " + TargetRotation.eulerAngles);
	}

	void Update () {
		//(TimeMaster.GeneralTiming * Time.deltaTime * Time.timeScale) +
		t += (TimeMaster.GeneralTiming * Time.deltaTime * Time.timeScale) + 0;
		if (t < 1) {
			transform.rotation = Quaternion.Lerp (transform.rotation, TargetRotation, t);
		}
		//Debug.Log("now "+ transform.rotation.eulerAngles + " " + FinalRotation);
		if (transform.rotation.eulerAngles == FinalRotation) GetComponentInChildren<ParticleSystem> ().Stop (false, ParticleSystemStopBehavior.StopEmitting);//AndClear
	}

}