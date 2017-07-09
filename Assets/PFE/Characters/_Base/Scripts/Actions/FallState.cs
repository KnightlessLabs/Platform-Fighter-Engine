using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;
using TrueSync;

[System.Serializable]
public class FallState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.FallStraight.Animation, true);
        ActionTimerDuration = Con.CI.AttributesInfo.JumpSquat;
        Con.IsGrounded = false;
        Con.ApplyGravity();
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if (Con.IsGrounded) {
            Con.ChangeState("Idle");
            return true;
        }
        return false;
    }
}