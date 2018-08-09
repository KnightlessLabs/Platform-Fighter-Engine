using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFE {
    [CreateAssetMenu(menuName = "PFE/StageInfo", order = 3)]
    public class StageInfo : ScriptableObject {
        public string stageName;
        public string stageSceneName;
        public List<Vector2> spawnPositions = new List<Vector2>();
    }
}
