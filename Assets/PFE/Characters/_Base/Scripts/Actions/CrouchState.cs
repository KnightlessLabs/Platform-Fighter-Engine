using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class CrouchState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Crouch.Animation, true);
        Con.Rigid.velocity = TSVector.zero;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
        }
    }

    public override bool ActionInterrupt () {
        if (Con.LeftStick.y > GameManager.instance.GInfo.CrouchSensitivity) {
            if (Con.FaceDirection == 1) { //Facing right
                if (Con.LeftStick.x >= 0.83f && Con.GetLeftStick(1).x <= .35f) {
                    Con.ChangeState("Dash");
                    return true;
                } else if (Con.LeftStick.x >= GameManager.instance.GInfo.WalkSensitivity) {
                    Con.ChangeState("Walk");
                    return true;
                } else if (Con.LeftStick.x <= -.25f) {
                    Con.ChangeState("TurnAround");
                    return true;
                }else if (TSMath.Abs(Con.LeftStick.x) < GameManager.instance.GInfo.WalkSensitivity) {
                    Con.ChangeState("Idle");
                    return true;
                }
            } else if (Con.FaceDirection == -1) { //Facing left
                if (Con.LeftStick.x <= -0.83f && Con.GetLeftStick(1).x >= -.35f) {
                    Con.ChangeState("Dash");
                    return true;
                } else if (Con.LeftStick.x <= -GameManager.instance.GInfo.WalkSensitivity) {
                    Con.ChangeState("Walk");
                    return true;
                } else if (Con.LeftStick.x >= .25f) {
                    Con.ChangeState("TurnAround");
                    return true;
                } else if (TSMath.Abs(Con.LeftStick.x) < GameManager.instance.GInfo.WalkSensitivity) {
                    Con.ChangeState("Idle");
                    return true;
                }
            }
        }
        return false;
    }
}
