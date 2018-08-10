using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using UnityEngine.Experimental.Input;
using FixedPointy;

namespace PFE.Character {
    public class CharInput : GSBehavior {

        [Header("References")]
        public CharController charController;

        [Header("Variables")]
        public bool isCPU;
        public List<PlayerInputs> localInputRecord = new List<PlayerInputs>();
        public int inputDelay = 3;
        public bool debug = false;

        public override void GSAwake() {
            base.GSAwake();
        }

        public override void GSUpdate() {
            if (isCPU) {

            } else {
                HandleInputs();
            }

            charController.CCUpdate();
        }

        public override void GSLateUpdate() {
            charController.CCLateUpdate();
        }

        public void HandleInputs() {
            var gamepad = Gamepad.current;
            PlayerInputs pi = new PlayerInputs();
            if (gamepad != null) {
                Vector2 lS = gamepad.leftStick.ReadValue();
                pi.LeftStick = new FixVec2((Fix)lS.x, (Fix)lS.y);
                pi.Attack = gamepad.buttonSouth.ReadValue() == 1 ? true : false;

                if (debug) {
                    Debug.Log(((float)pi.LeftStick.X).ToString());
                }
            }
            localInputRecord.Add(pi);
        }

        #region Inputs
        public virtual PlayerInputDefinition LeftStick() {
            PlayerInputDefinition pid = new PlayerInputDefinition();
            if (localInputRecord.Count == 0 || localInputRecord.Count-1-inputDelay < 0) {
                return pid;
            } else {
                pid.axis = localInputRecord[localInputRecord.Count - 1-inputDelay].LeftStick;
                return pid;
            }
        }

        public virtual PlayerInputDefinition Attack() {
            PlayerInputDefinition pid = new PlayerInputDefinition();
            if (localInputRecord.Count == 0 || localInputRecord.Count - 2 - inputDelay < 0) {
                return pid;
            } else {
                if (!localInputRecord[localInputRecord.Count - 2 - inputDelay].Attack) {
                    //First press
                    pid.firstPress = localInputRecord[localInputRecord.Count - 1 - inputDelay].Attack;
                }
            }
            return pid;
        }
        #endregion
    }
}
