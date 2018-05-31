using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMove : BaseParticlesAction {
    
    public override void Play () {
        Piece.ParticleFly(PieceSpell.CastedFrom, PieceSpell.CastedTo);
    }

}