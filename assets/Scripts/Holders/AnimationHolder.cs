using UnityEngine;

public class AnimationHolder : MonoBehaviour {

	public Animation Animation;

	void Start () {

	}

	void Update () {
		if (Time.frameCount % TimeMaster.GeneralFrameWaitingInterval == 0) { }
	}

}