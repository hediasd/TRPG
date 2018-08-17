using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesAnimation : MonoBehaviour {

	BaseParticlesAction PlayingAction;
	PiecesMaster PiecesMaster;
	PieceSpell PieceSpell;
	//int ChildAmount = 0;

	public void Startup (PieceSpell ps) {
		//PlayingAction = transform.GetChild (0).GetComponent<BaseParticlesAction> ();
		PiecesMaster = transform.GetComponentInParent<PiecesMaster> ();
		PieceSpell = ps;

		BaseParticlesAction[] Bases = GetComponentsInChildren<BaseParticlesAction> ();

		Utility.Each (Bases, Base => Base.Startup (ps));
		foreach (Transform Child in this.GetComponentsInChildren<Transform> ()) {
			Child.gameObject.SetActive (false);
		}
		
		this.gameObject.SetActive (true);
		//PlayingAction.Play ();
	}

	void Update () {
		if (transform.childCount == 0) {
			//PiecesMaster.Acting--;
			//TimeMaster.WaitSeconds(1);
			Destroy (this.gameObject);
		} else {
			if (!transform.GetChild (0).gameObject.activeSelf) {
				this.transform.GetChild (0).gameObject.SetActive (true);
				Transform FirstChild = transform.GetChild (0);
				BaseParticlesAction FirstAction = FirstChild.GetComponent<BaseParticlesAction> ();
				if (FirstAction != null && FirstAction != PlayingAction) {
					PlayingAction = FirstAction;
					PlayingAction.Play ();
				}
			}
		}
	}

}