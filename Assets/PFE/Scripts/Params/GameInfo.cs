using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

namespace PFE {
    [CreateAssetMenu(menuName = "PFE/GameInfo", order = 1)]
    public class GameInfo : ScriptableObject {
        #region Input Deadzones
        [Header("Input Deadzones")]
        public int bufferWindow = 10; //In frames
        public Fix walkSensitivity; //.25f
        public Fix turnSensitivity; //.29f
        public Fix dashSensitivity; //.83f
        public Fix crouchSensitivity; //.64f
        public Fix jumpSensitivity; //.66f
        public Fix deadzone; //.1f
        #endregion

        [Header("Collsion")]
        public Fix ecbPushback; //Pushback per frame, .34f

        [Header("Info")]
        public CharacterInfo[] characters;
        public StageInfo[] stages;
    }
}