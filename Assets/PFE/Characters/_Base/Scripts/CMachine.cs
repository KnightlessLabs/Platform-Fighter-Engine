using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class CMachine : TrueSyncBehaviour {

    #region Inputs
    [Header("Inputs")]
    public TSVector2 LeftStick; //Left stick
    public Vector2 RS; //Right stick
    public bool RunB; //Run button
    public bool AttackB; //Attack button
    public bool SpecialB; //Special button
    public bool JumpB; //Jump button
    #endregion

    #region States
    [Header("States")]
    public GameObject StateHolderGO;
    public Dictionary<string, BaseAction> StatesDictionary = new Dictionary<string, BaseAction>();
    public BaseAction[] States;
    public BaseAction CurrentState;
    public bool InAttackingState = false;
    #endregion

    #region Physics/Checks
    [Header("Physics and Checks (Debug)")]
    public int FaceDirection = 1; //1 = right, -1 = left
    public bool IsGrounded = false;
    public FP GroundedCheckDistance = 1;
    public FP CurrentFallSpeed = 0;
    #endregion

    #region References
    [Header("References")]
    public TSTransform Trans;
    public TSRigidBody Rigid;
    public TSBoxCollider MainCollider;
    public CharacterInfo CI;
    public PFEAnimator Anim;
    public GameObject Visual;
    public int PlayerNumber = 0;
    #endregion

    public virtual void Awake () {
        //References
        Rigid = GetComponent<TSRigidBody>();
        Anim = GetComponent<PFEAnimator>();
    }

    public virtual void CUpdate () {
        //Grounded check
        TSRaycastHit GroundedCheck = PhysicsWorldManager.instance.Raycast(new TSRay(new TSVector(transform.position.x, transform.position.y - 0.01f, transform.position.z), TSVector.down), GroundedCheckDistance);
        if (GroundedCheck != null) {
            IsGrounded = GroundedCheck.collider.tag == "Stage" ? true : false;
        } else {
            IsGrounded = false;
        }
        //Face proper direction
        Visual.transform.eulerAngles = FaceDirection == 1 ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0);
        //Action update
        CurrentState.ActionUpdate();
    }

    public virtual void ChangeState (string StateName) {

    }

    public virtual void CheckForAttack () {

    }

    public virtual bool CheckForJump () {
        if (JumpB) {
            return true;
        }else if (GameManager.instance.TapJump && LeftStick.y > GameManager.instance.GInfo.JumpSensitivity && GetLeftStick(3).y < .2f) {
            return true;
        }
        return false;
    }

    public virtual void ReduceByTraction (bool DoubleTraction) {
        TSVector temp = Rigid.velocity;
        if (Rigid.velocity.x > 0) {
            temp.x -= DoubleTraction && Rigid.velocity.x > CI.AttributesInfo.WalkSpeed ? CI.AttributesInfo.Traction * 2 : CI.AttributesInfo.Traction;
            if(temp.x < 0) {
                temp.x = 0; //make sure we don't add force that causes us to move in the opposite direction.
            }
        } else if(Rigid.velocity.x < 0) {
            temp.x += DoubleTraction && Rigid.velocity.x < -CI.AttributesInfo.WalkSpeed ? CI.AttributesInfo.Traction * 2 : CI.AttributesInfo.Traction;
            if (temp.x > 0) {
                temp.x = 0; //make sure we don't add force that causes us to move in the opposite direction.
            }
        }
        Rigid.velocity = temp;
    }

    public virtual void ApplyGravity () {
        if (!IsGrounded) {
            CurrentFallSpeed -= CI.AttributesInfo.Gravity;
            CurrentFallSpeed = TSMath.Clamp(CurrentFallSpeed, -CI.AttributesInfo.MaxFallSpeed, 9999);
            Rigid.AddForce(CurrentFallSpeed * TSVector.up);
        } else {
            CurrentFallSpeed = 0;
        }
    }

    public TSVector2 GetLeftStick (int FramesBehind) { //0 = current frame, 1 = last frame, etc...
        if(GameManager.instance.Recorder.Players[PlayerNumber].InputBacktrack.Count-1 > FramesBehind) {
            VCR temp = GameManager.instance.Recorder;

            return temp.Players[PlayerNumber].InputBacktrack[temp.Players[PlayerNumber].InputBacktrack.Count - 1 - FramesBehind].LeftStick;
        } else {
            return new TSVector2(0, 0);
        }
    }
}
