using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class RunState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Run.Animation, true);
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();

            TSVector temp = Con.Rigid.velocity;
            FP tempMax = Con.GetLeftStick(0).x * Con.CI.AttributesInfo.RunMaxVelo;

            temp.x += ((Con.CI.AttributesInfo.RunMaxVelo * Con.GetLeftStick(0).x) - temp.x) 
                * (1/(Con.CI.AttributesInfo.RunMaxVelo *2.5)) * (Con.CI.AttributesInfo.DashRunAcceleration + (Con.CI.AttributesInfo.DashRunDeceleration/TSMath.Abs(Con.GetLeftStick(0).x)));
            if (temp.x * Con.FaceDirection > tempMax * Con.FaceDirection) {
                temp.x = tempMax;
            }
            Con.Rigid.velocity = temp;
        }
    }

    public override bool ActionInterrupt () {
        if (!Con.IsGrounded) {
            Con.ChangeState("Fall");
        }else if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        } else if (TSMath.Abs(Con.LeftStick.x) < .62) {
            Con.ChangeState("RunBrake");
            return true;
        } else if (Con.FaceDirection == 1) { //Facing right
            if (Con.LeftStick.x <= -.25f) {
                Con.ChangeState("RunTurn");
                return true;
            }
        } else if (Con.FaceDirection == -1) { //Facing left
            if (Con.LeftStick.x >= .25f) {
                Con.ChangeState("RunTurn");
                return true;
            }
        }
        return false;
    }
}