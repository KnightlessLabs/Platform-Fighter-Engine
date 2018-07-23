using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageInfo : MonoBehaviour {
    public static StageInfo Instance;
    public Transform[] SpawnPoints;

    void Awake () {
        Instance = this;
    }
}
