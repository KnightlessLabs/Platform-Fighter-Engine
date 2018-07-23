using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VCR : MonoBehaviour {

    //public List<PlayersInputHolder> Players = new List<PlayersInputHolder>(); //Holds each player's input backlog
    public bool Playback = false;
    public int CurrentFrame = 0;
    public bool tPlay = false;

    public void OnSyncedUpdate () {
        if (Playback) {
            CurrentFrame += 1;
        }
        if (tPlay) {
            CurrentFrame = 0;
            GameManager.instance.matchManager.playersInMatch[0].transform.position = new Vector3(0, 0, 0);
            tPlay = false;
            Playback = true;
        }
    }

    void Awake () {
        //Players = new List<PlayersInputHolder>();
        //Players.Add(new PlayersInputHolder());
    }

    public void RecordInputs (int PlayerNumber, LocalPlayerInputs PInputs) {
        //Players[PlayerNumber].InputBacktrack.Add(PInputs);
    }

    public void StartPlayback () {
        tPlay = true;
        CurrentFrame = 0;
    }
}

[System.Serializable]
public class PlayersInputHolder {
    public List<LocalPlayerInputs> InputBacktrack = new List<LocalPlayerInputs>();
}