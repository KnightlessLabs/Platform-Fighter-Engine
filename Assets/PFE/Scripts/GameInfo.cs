using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInfo : ScriptableObject {
    //Inputs
    public int BufferWindow = 10; //In frames
    public float WalkSensitivity = .18f;
    public float TurnSensitivity = .25f;
    public float DashSensitivity = .83f;
    public float CrouchSensitivity = -.66f;
    public float JumpSensitivity = .66f;
    public float Deadzone = 0.1f;
    public CharacterInfo[] Characters;
}
