using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFE {
    [CreateAssetMenu(menuName = "PFE/GameInfo", order = 1)]
    public class GameInfo : ScriptableObject {
        #region Input Deadzones
        [Header("Input Deadzones")]
        public int bufferWindow = 10; //In frames
        public float walkSensitivity = .25f;
        public float turnSensitivity = .29f;
        public float dashSensitivity = .83f;
        public float crouchSensitivity = .64f;
        public float jumpSensitivity = .66f;
        public float deadzone = 0.1f;
        #endregion

        [Header("Collsion")]
        public float ecbPushback = 0.34f; //Pushback per frame

        [Header("Info")]
        public CharacterInfo[] characters;
        public StageInfo[] stages;
    }
}