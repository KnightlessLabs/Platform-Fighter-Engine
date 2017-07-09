using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class CInput : TrueSyncBehaviour {

    CController _CCon;
    TSVector2 LeftStick;
    bool AttackB;
    bool SpecialB;
    bool JumpB;

    void Awake () {
        _CCon = GetComponent<CController>();
    }

    public override void OnSyncedStart () {
        GameManager.instance.Players.Add(this);
    }

    public override void OnSyncedInput () {
        //Get inputs
        LeftStick = new TSVector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        AttackB = Input.GetButton("Attack");
        SpecialB = Input.GetButton("Special");
        JumpB = Input.GetButton("Jump");

        //Deadzone handling
        FP deadzone = GameManager.instance.GInfo.Deadzone;
        if (LeftStick.magnitude < deadzone) {
            LeftStick = TSVector2.zero;
        } else {
            LeftStick = LeftStick.normalized * ( ( LeftStick.magnitude - deadzone ) / ( 1 - deadzone ) );
        }

        //Pass over inputs
        SetTSIs();
        GameManager.instance.Recorder.RecordInputs(0, new PlayerInputs(LeftStick, TSVector2.zero));
    }

    public void SetTSIs () { //Set TrueSyncInputs
        TrueSyncInput.SetTSVector2(0, LeftStick);
        TrueSyncInput.SetBool(2, AttackB);
        TrueSyncInput.SetBool(3, JumpB);
    }

    //Update controller with inputs
    public override void OnSyncedUpdate () {
        TSVector2 LS = TrueSyncInput.GetTSVector2(0);
        bool Attack = TrueSyncInput.GetBool(2);
        bool Jump = TrueSyncInput.GetBool(3);

        _CCon.LeftStick = LS;
        _CCon.AttackB = Attack;
        _CCon.JumpB = Jump;
        _CCon.CUpdate();
    }
}
