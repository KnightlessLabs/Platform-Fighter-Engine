using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class VCR : TrueSyncBehaviour {

    public List<PlayersInputHolder> Players = new List<PlayersInputHolder>(); //Holds each player's input backlog
    [AddTracking]
    public bool Playback = false;
    [AddTracking]
    public int CurrentFrame = 0;
    public bool tPlay = false;

    public override void OnSyncedUpdate () {
        if (Playback) {
            CurrentFrame += 1;
        }
        if (tPlay) {
            CurrentFrame = 0;
            GameManager.instance.Players[0].tsTransform.position = new TSVector(0, 1.28f, 0);
            tPlay = false;
            Playback = true;
            TrueSyncInput.SetBool(10, true);
        }
    }

    void Awake () {
        Players = new List<PlayersInputHolder>();
        Players.Add(new PlayersInputHolder());
    }

    public void RecordInputs (int PlayerNumber, PlayerInputs PInputs) {
        Players[PlayerNumber].InputBacktrack.Add(PInputs);
    }

    public void StartPlayback () {
        tPlay = true;
        CurrentFrame = 0;
    }
}

[System.Serializable]
public class PlayersInputHolder {
    public List<PlayerInputs> InputBacktrack = new List<PlayerInputs>();
}

[System.Serializable]
public class PlayerInputs {
    public TSVector2 LeftStick, RightStick;
    public bool Attack, Special, Jump, Shield, Grab;
    public bool UpTaunt, DownTaunt, LeftTaunt, RightTaunt;

    public PlayerInputs(TSVector2 LS, TSVector2 RS){
        LeftStick = LS;
        RightStick = RS;
    }
}