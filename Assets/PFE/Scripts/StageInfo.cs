using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class StageInfo : MonoBehaviour {
    public static StageInfo Instance;
    public TSTransform[] SpawnPoints;

    void Awake () {
        Instance = this;
    }
}
