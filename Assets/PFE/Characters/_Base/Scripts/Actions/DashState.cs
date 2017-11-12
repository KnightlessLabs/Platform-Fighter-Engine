using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

[System.Serializable]
public class DashState : BaseAction {

    public int InitDirection = 1;

    public override void ActionStart () {
        base.ActionStart();
        ActionTimerDuration = Con.CI.AttributesInfo.DashFrames;
        Con.Anim.PlayAnimation(Con.CI.CombatStances[0].BasicMoves.Dash.Animation, true);
        CurrentActionTime = 1;
    }

    public override void ActionUpdate () {
        if (!ActionInterrupt()) {
            if (CurrentActionTime == 2) {
                TSVector temp = Con.Rigid.velocity;
                temp.x = Con.CI.AttributesInfo.DashInitialVelocity * Con.FaceDirection;
                if(TSMath.Abs(temp.x) > Con.CI.AttributesInfo.DashSpeed) {
                    temp.x = Con.CI.AttributesInfo.DashSpeed * Con.FaceDirection;
                }
                temp.y = 0;
                Con.Rigid.velocity = temp;
            }

             if (CurrentActionTime > 1) {
                if (TSMath.Abs(Con.LeftStick.x) < 0.3f) {
                    Con.ReduceByTraction(false);
                } else {
                    TSVector temp = Con.Rigid.velocity;
                    temp.y = 0;
                    FP tempMax = Con.LeftStick.x * Con.CI.AttributesInfo.DashSpeed;
                    FP tempAcceleration = Con.LeftStick.x * Con.CI.AttributesInfo.DashRunAcceleration;
                    temp.x += tempAcceleration;
                    Con.Rigid.velocity = temp;
                    temp = Con.Rigid.velocity;

                    if (( tempMax > 0 && temp.x > tempMax ) || ( tempMax < 0 && temp.x < tempMax )) {
                        Con.ReduceByTraction(false);
                        if (( tempMax > 0 && temp.x < tempMax ) || ( tempMax < 0 && temp.x > tempMax )) {
                            temp.x = tempMax;
                        }
                        Con.Rigid.velocity = temp;
                    } else {
                        temp.x += tempAcceleration;
                        if (( tempMax > 0 && temp.x > tempMax ) || ( tempMax < 0 && temp.x < tempMax )) {
                            temp.x = tempMax;
                        }
                        Con.Rigid.velocity = temp;
                    }
                }
            }
        }
        CurrentActionTime++;
    }

    public override bool ActionInterrupt () {
        if (Con.CheckForJump()) {
            Con.ChangeState("JumpSquat");
            return true;
        } else if (CurrentActionTime < Con.CI.AttributesInfo.DashDanceWindow) {
            if (Con.FaceDirection == 1) {
                if (Con.GetLeftStick(0).x < -GameManager.instance.GInfo.DashSensitivity) {
                    Con.FaceDirection = -Con.FaceDirection;
                    Con.ChangeState("Dash");
                    return true;
                }
            } else {
                if (Con.GetLeftStick(0).x > GameManager.instance.GInfo.DashSensitivity) {
                    Con.FaceDirection = -Con.FaceDirection;
                    Con.ChangeState("Dash");
                    return true;
                }
            }
        }else if(CurrentActionTime >= ActionTimerDuration && TSMath.Abs(Con.LeftStick.x) >= GameManager.instance.GInfo.WalkSensitivity) {
            Con.ChangeState("Run");
            return true;
        } else if (Con.LeftStick.y <= -0.66f) {
            Con.ChangeState("Crouch");
            return true;
        } else if (CurrentActionTime >= ActionTimerDuration) {
            Con.ChangeState("Idle");
            return true;
        }
        return false;
    }
}
