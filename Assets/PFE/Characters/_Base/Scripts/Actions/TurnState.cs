using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;
using TrueSync;

[System.Serializable]
public class TurnState : BaseAction {

    public FP WalkSpeed;
    public FP TiltType = 0; //0 = TiltTurn, 1 = SmashTurn

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Turn.Animation, true);
        TiltType = TSMath.Abs(Con.LeftStick.x) < .8f ? 0 : 1;
        if(TiltType == 1) {
            Con.FaceDirection = -Con.FaceDirection;
        }
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ApplyGravity();
            Con.ReduceByTraction(true);
            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if (TiltType == 0) { //TiltTurn
            if(CurrentActionTime == 5) {
                Con.FaceDirection = -Con.FaceDirection;
                if (Con.LeftStick.x * Con.FaceDirection >= GameManager.instance.GInfo.DashSensitivity) {
                    Con.ChangeState("Dash");
                    return true;
                }
            }

            if(CurrentActionTime >= 11) {
                if (TSMath.Abs(Con.LeftStick.x) >= .18f) {
                    Con.ChangeState("Walk");
                    return true;
                } else {
                    Con.ChangeState("Idle");
                    return true;
                }
            }
        }else if (TiltType == 1) { //SmashTurn
            if (CurrentActionTime > 1) {
                if (Con.LeftStick.x * Con.FaceDirection >= GameManager.instance.GInfo.DashSensitivity) {
                    Con.ChangeState("Dash");
                    return true;
                } else if (CurrentActionTime > ActionTimerDuration) {
                    Con.ChangeState("Idle");
                    return true;
                }
            }
        }
        return false;
    }
}
