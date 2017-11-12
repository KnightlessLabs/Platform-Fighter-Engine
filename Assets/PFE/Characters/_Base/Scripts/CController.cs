using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using TrueSync.Physics3D;

/// <summary>
/// Controller that operates similar to Smash Bros.
/// </summary>
public class CController : CMachine {

    public override void Awake () {
        base.Awake();
        //Grab all our states and put them in a dictionary
        for (int i = 0; i < States.Length; i++) {
            BaseAction temp = (BaseAction)StateHolderGO.AddComponent(States[i].GetType());

            temp.Con = this;
            temp.Name = States[i].Name;

            StatesDictionary.Add(temp.Name, temp);
        }

        CurrentState = StatesDictionary["Idle"];
        CurrentState.ActionStart();
    }

    public override void CUpdate () {
        base.CUpdate();
    }

    public override void ChangeState ( string StateName ) {
        if (StatesDictionary.ContainsKey(StateName)) {
            CurrentState = StatesDictionary[StateName];
            CurrentState.ActionStart();
        } else {
            Debug.Log("State " + StateName + " does not exist.");
        }
    }

    void OnGUI () {
        GUI.color = Color.red;
        GUI.Label(new Rect(0, 0, 400, 75), "Current State: " + CurrentState.Name);
        GUI.Label(new Rect(0, 80, 400, 75), CurrentState.CurrentActionTime + "/" + CurrentState.ActionTimerDuration);
    }
}
