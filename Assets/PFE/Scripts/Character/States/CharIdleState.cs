using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

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
            cController.vars.applyGravity = true;
        }

        public void OnUpdate() {
            if (!CheckInterrupt()) {
                cController.ApplyTraction();

                cController.HandleForces();
            }
        }

        public void OnLateUpdate() {

        }

        public bool CheckInterrupt() {
            GameInfo gi = cController.cacheGM.gameInfo;
            if (cController.CheckForAttack()) {
                return true;
            } else if (FixMath.Abs(cInput.LeftStick().axis.X) >= gi.walkSensitivity) {
                cController.ChangeState("Walk");
                return true;
            }
            return false;
        }

        public void OnInterrupted() {

        }

        public void Setup(CharInput cInput, CharController cCon) {
            this.cInput = cInput;
            cController = cCon;
        }
    }
}
