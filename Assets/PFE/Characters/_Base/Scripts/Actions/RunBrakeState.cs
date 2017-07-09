using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;
using TrueSync;

[System.Serializable]
public class RunBrakeState : BaseAction {

    public override void ActionStart () {
        base.ActionStart();
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Idle.Animation, true);
        ActionTimerDuration = 35;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            Con.ReduceByTraction(true);

            CurrentActionTime++;
        }
    }

    public override bool ActionInterrupt () {
        if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        }else if (Con.GetLeftStick(0).x * Con.FaceDirection < -0.3) {
            Con.ChangeState("RunTurn");
            return true;
        }else if(CurrentActionTime > ActionTimerDuration) {
            Con.ChangeState("Idle");
            return true;
        }
        return false;
    }
}
