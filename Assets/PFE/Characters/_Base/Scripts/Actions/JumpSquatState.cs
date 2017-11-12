using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class JumpSquatState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Jumpsquat.Animation, true);
        Con.JumpType = 1;
        Con.Rigid.velocity = TSVector.zero;
        ActionTimerDuration = Con.CI.AttributesInfo.JumpSquat;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            Con.ReduceByTraction(true);

            if (!Con.JumpB) {
                Con.JumpType = 0;
            }

            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if(CurrentActionTime >= ActionTimerDuration) {
            Con.currentJump++;
            Con.IsGrounded = false;
            Con.ChangeState("Jump");
            return true;
        }
        return false;
    }
}
