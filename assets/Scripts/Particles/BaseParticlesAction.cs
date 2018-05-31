using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseParticlesAction : MonoBehaviour {

	public ParticlesAnimation ParticlesAnimation;
	public Piece Piece;
	public PieceSpell PieceSpell;

	public void Startup(PieceSpell ps){
		Piece = GetComponentInParent<Piece>();
		ParticlesAnimation = GetComponentInParent<ParticlesAnimation>();
		PieceSpell = ps;
	}
	
	public abstract void Play ();
	
}
