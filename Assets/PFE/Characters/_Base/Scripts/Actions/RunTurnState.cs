using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class RunTurnState : BaseAction {

    public FP RunSpeed;

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Idle.Animation, true);
        ActionTimerDuration = 21;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();

            TSVector temp = Con.Rigid.velocity;
            FP tempAcc;

            if (CurrentActionTime <= 5 && Con.GetLeftStick(0).x * Con.FaceDirection < -0.3) {
                tempAcc = (Con.CI.AttributesInfo.DashRunAcceleration - (1 - TSMath.Abs(Con.GetLeftStick(0).x)) * (Con.CI.AttributesInfo.DashRunAcceleration)) * Con.FaceDirection;
                temp.x -= tempAcc;
                Con.Rigid.velocity = temp;
            } else if (CurrentActionTime > 5 && Con.GetLeftStick(0).x * Con.FaceDirection > 0.3) {
                tempAcc = ( Con.CI.AttributesInfo.DashRunAcceleration - ( 1 - TSMath.Abs(Con.GetLeftStick(0).x) ) * ( Con.CI.AttributesInfo.DashRunAcceleration ) ) * Con.FaceDirection;
                temp.x += tempAcc;
                Con.Rigid.velocity = temp;
            } else {
                Con.ReduceByTraction(true);
            }

            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if (CurrentActionTime == 6) { //On the 5th frame we want to turn around.
            Con.FaceDirection = -Con.FaceDirection;
        }

        if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        }if (CurrentActionTime >= 21) {
            if (TSMath.Abs(Con.LeftStick.x) < GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Idle");
                return true;
            } else if (TSMath.Abs(Con.LeftStick.x) >= GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Run");
                return true;
            }
        }
        return false;
    }
}