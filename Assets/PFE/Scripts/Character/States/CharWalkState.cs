using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

namespace PFE.Character {
    public class CharWalkState : ICharacterState {
        public string StateName {
            get {
                return "Walk";
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
                float perc = (float)((FixMath.Abs(cInput.LeftStick().axis.X)-cController.cacheGM.gameInfo.walkSensitivity) / (Fix.One-cController.cacheGM.gameInfo.walkSensitivity));

                Vector2 wantedSpeed = new Vector2();
                wantedSpeed.x = (cController.vars.facingRight ? 1 : -1) * Mathf.Lerp((float)cController.cInfo.attributes.minWalkSpeed, (float)cController.cInfo.attributes.walkSpeed, perc);

                Vector2 currentSpeed = (Vector2)cController.kineticEnergies.horizontal;
                currentSpeed = Vector2.Lerp(currentSpeed, wantedSpeed, (float)cController.cInfo.attributes.walkAcceleration);

                cController.kineticEnergies.horizontal = (FixVec2)currentSpeed;

                cController.HandleForces();
            }
        }

        public void OnLateUpdate() {

        }

        public bool CheckInterrupt() {
            GameInfo gi = cController.cacheGM.gameInfo;
            if (cController.CheckForAttack()) {
                return true;
            } else if (FixMath.Abs(cInput.LeftStick().axis.X) < gi.walkSensitivity) {
                cController.ChangeState("Idle");
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