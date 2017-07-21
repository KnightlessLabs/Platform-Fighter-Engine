using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class JumpSquatState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Jumpsquat.Animation, true);
        Con.Rigid.velocity = TSVector.zero;
        ActionTimerDuration = Con.CI.AttributesInfo.JumpSquat;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if(CurrentActionTime >= ActionTimerDuration) {
            Con.IsGrounded = false;
            Con.ChangeState("Jump");
            return true;
        }
        return false;
    }
}
