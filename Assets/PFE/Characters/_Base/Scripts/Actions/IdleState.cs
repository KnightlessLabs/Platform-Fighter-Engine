using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class IdleState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Idle.Animation, true);
        Con.Rigid.velocity = new TSVector(Con.Rigid.velocity.x, 0, Con.Rigid.velocity.z);
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ReduceByTraction(false);
        }
    }

    public override bool ActionInterrupt () {
        if (!Con.IsGrounded) {
            Con.ChangeState("Fall");
            return true;
        } else if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        }else if (Con.LeftStick.y <= GameManager.instance.GInfo.CrouchSensitivity) {
            Con.ChangeState("Crouch");
            return true;
        } else if (Con.FaceDirection == 1) { //Facing right
            if (Con.GetLeftStick(0).x * Con.FaceDirection > GameManager.instance.GInfo.DashSensitivity && Con.GetLeftStick(2).x * Con.FaceDirection < 0.3 && Con.GetLeftStick(2).x != 0) {
                Con.ChangeState("Dash");
                return true;
            } else if (Con.LeftStick.x >= GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Walk");
                return true;
            } else if(Con.LeftStick.x <= -GameManager.instance.GInfo.TurnSensitivity) {
                Con.ChangeState("Turn");
                return true;
            }
        } else if(Con.FaceDirection == -1) { //Facing left
            if (Con.GetLeftStick(0).x * Con.FaceDirection > GameManager.instance.GInfo.DashSensitivity && Con.GetLeftStick(2).x * Con.FaceDirection < 0.3 && Con.GetLeftStick(2).x != 0) {
                Con.ChangeState("Dash");
                return true;
            } else if (Con.LeftStick.x <= -GameManager.instance.GInfo.WalkSensitivity) {
                Con.ChangeState("Walk");
                return true;
            } else if (Con.LeftStick.x >= GameManager.instance.GInfo.TurnSensitivity) {
                Con.ChangeState("Turn");
                return true;
            }
        }
        return false;
    }
}
