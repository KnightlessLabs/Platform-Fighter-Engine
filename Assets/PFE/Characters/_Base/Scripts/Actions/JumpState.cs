using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class JumpState : BaseAction {

    public int JumpType = 0; //0 = forward/neutral, 2 = back

    public override void ActionStart () {
        base.ActionStart();

        //Detect jump type (forward/backwards)
        JumpType = Con.FaceDirection * Con.LeftStick.x >= -0.3f ? 0 : 1;

        //Apply force based on type
        Con.Rigid.velocity = new TSVector(Con.Rigid.velocity.x * Con.CI.AttributesInfo.GroundToAir + (Con.LeftStick.x * Con.CI.AttributesInfo.JumpVelo), Con.Rigid.velocity.y + Con.CI.AttributesInfo.JumpVelo, Con.Rigid.velocity.z);

        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.JumpStraight.Animation, true);
        ActionTimerDuration = Con.CI.AttributesInfo.JumpFrameTime;
        Con.IsGrounded = false;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if(CurrentActionTime >= ActionTimerDuration) {
            Con.ChangeState("Fall");
            return true;
        }
        return false;
    }
}

