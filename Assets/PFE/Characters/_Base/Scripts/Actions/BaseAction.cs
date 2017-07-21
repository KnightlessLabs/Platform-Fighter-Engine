using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAction : MonoBehaviour{

    public CController Con;
    public string Name;

    //Timer
    public float ActionTimerDuration = 10; //In frames
    public float CurrentActionTime = 0;

    //Variables
    public bool canEdgeCancel = true;
    public bool canBeGrabbed = true;
    public bool disableTeeter = false;
    public bool Crouch = false; 

    public virtual void ActionAwake () {
    }

    public virtual void ActionStart () {
        CurrentActionTime = 1;
    }

    public virtual void ActionUpdate () {

    }

    public virtual bool ActionInterrupt () {
        return false;
    }
}
