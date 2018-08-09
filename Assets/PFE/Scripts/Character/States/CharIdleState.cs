using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFE.Character {
    public class CharIdleState : ICharacterState {
        public string StateName {
            get {
                return "Idle";
            }
        }

        public int StateDuration {
            get {
                return 0;
            }
        }

        public CharInput inputHandler {
            get {
                return cInput;
            }
            set {
                cInput = value;
            }
        }
        public CharController controller {
            get {
                return cController;
            }
            set {
                cController = value;
            }
        }

        public CharInput cInput;
        public CharController cController;

        public void OnStart() {
            cController.applyGravity = true;
        }

        public void OnUpdate() {
            if (!CheckInterrupt()) {
                cController.ApplyForces();
            }
        }

        public void OnLateUpdate() {

        }

        public bool CheckInterrupt() {
            GameInfo gi = cController.cacheGM.gameInfo;
            if (cController.CheckForAttack()) {
                Debug.Log("Attack");
                return true;
            } else if (Mathf.Abs((float)cInput.LeftStick().axis.x) > gi.walkSensitivity) {
                Debug.Log("Walk");
                return true;
            }
            return false;
        }

        public void OnInterrupted() {

        }
    }
}
