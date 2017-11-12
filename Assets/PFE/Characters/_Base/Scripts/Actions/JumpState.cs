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
        if (Con.currentJump <= 1) {
            Con.Rigid.velocity = new TSVector(Con.Rigid.velocity.x * Con.CI.AttributesInfo.GroundToAir + ( Con.LeftStick.x * Con.CI.AttributesInfo.JumpVelo ),
                Con.Rigid.velocity.y + ( Con.JumpType == 1 ? Con.CI.AttributesInfo.JumpVelo : Con.CI.AttributesInfo.sHopVelo ), Con.Rigid.velocity.z);
        } else {
            Con.Rigid.velocity = new TSVector(Con.LeftStick.x * Con.CI.AttributesInfo.dJumpMomentum,
                Con.CI.AttributesInfo.JumpVelo * (Con.CI.AttributesInfo.dJumpMulti/(Con.currentJump-1)), Con.Rigid.velocity.z);
        }

        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.JumpStraight.Animation, true);
        ActionTimerDuration = Con.CI.AttributesInfo.JumpFrameTime;
        Con.IsGrounded = false;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            Con.AirDrift();
            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if (Con.GetJumpButton(0) && !Con.GetJumpButton(1) && Con.currentJump < 2) {
            Con.currentJump++;
            Con.ChangeState("Jump");
            return true;
        } else if(CurrentActionTime >= ActionTimerDuration) {
            Con.ChangeState("Fall");
            return true;
        }
        return false;
    }
}

