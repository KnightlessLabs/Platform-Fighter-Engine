using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class WalkState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Walk.Animation, true);
        TSVector temp = Con.Rigid.velocity;
        FP tempInitVelo = Con.CI.AttributesInfo.WalkInitialVelocity * Con.FaceDirection;
        if ((tempInitVelo > 0 && temp.x < tempInitVelo) || (tempInitVelo < 0 && temp.x > tempInitVelo)) {
            temp.x += Con.CI.AttributesInfo.WalkInitialVelocity * Con.FaceDirection;
        }
        Con.Rigid.velocity = temp;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();

            FP tempMax = Con.CI.AttributesInfo.WalkSpeed * Con.GetLeftStick(0).x;
            FP tempMin = Con.CI.AttributesInfo.minWalkSpeed * (Con.GetLeftStick(0).x > 0 ? 1 : -1);

            if (TSMath.Abs(Con.Rigid.velocity.x) > TSMath.Abs(tempMax)) {
                Con.ReduceByTraction(true);
            } else {
                FP tempAcceleration = (tempMax - Con.Rigid.velocity.x) * (1/Con.CI.AttributesInfo.WalkSpeed*2) * (Con.CI.AttributesInfo.WalkInitialVelocity + Con.CI.AttributesInfo.WalkAcceleration);

                TSVector temp = Con.Rigid.velocity;
                temp.x += tempAcceleration;
                if (temp.x * Con.FaceDirection > tempMax * Con.FaceDirection) {
                    temp.x = tempMax;
                }
                if(temp.x * Con.FaceDirection < tempMin * Con.FaceDirection) {
                    temp.x = tempMin;
                }
                Con.Rigid.velocity = temp;
            }
        }
    }

    public override bool ActionInterrupt () {
        if (!Con.IsGrounded) {
            Con.ChangeState("Fall");
            return true;
        } else if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        } else if (Con.LeftStick.y <= GameManager.instance.GInfo.CrouchSensitivity) {
            Con.ChangeState("Crouch");
            return true;
        } else if (Con.FaceDirection == 1) {
            if (Con.GetLeftStick(0).x * Con.FaceDirection > 0.79f && Con.GetLeftStick(2).x * Con.FaceDirection < 0.3 && Con.GetLeftStick(2).x != 0) {
                Con.ChangeState("Dash");
                return true;
            } else if (Con.LeftStick.x <= -.25f) {
                Con.ChangeState("Turn");
                return true;
            } else if (TSMath.Abs(Con.LeftStick.x) < GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Idle");
                return true;
            }
        } else if(Con.FaceDirection == -1) {
            if (Con.GetLeftStick(0).x * Con.FaceDirection > 0.79f && Con.GetLeftStick(2).x * Con.FaceDirection < 0.3 && Con.GetLeftStick(2).x != 0) {
                Con.ChangeState("Dash");
                return true;
            } else if (Con.LeftStick.x >= .25f) {
                Con.ChangeState("Turn");
                return true;
            } else if (TSMath.Abs(Con.LeftStick.x) < GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Idle");
                return true;
            }
        }
        return false;
    }
}
