using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFE.Core {
    public class VCR : MonoBehaviour {

        //public List<PlayersInputHolder> Players = new List<PlayersInputHolder>(); //Holds each player's input backlog
        public bool Playback = false;
        public int CurrentFrame = 0;
        public bool tPlay = false;

    }
}

[System.Serializable]
public class PlayersInputHolder {
    public List<LocalPlayerInputs> InputBacktrack = new List<LocalPlayerInputs>();
}