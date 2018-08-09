using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;
using UnityEngine.Experimental.Input.LowLevel;

namespace PFE.Character {
    public class CharInput : GSBehavior {

        [Header("References")]
        public CharController charController;
        public GameControls gControls;

        [Header("Variables")]
        public bool isCPU;
        public List<PlayerInputs> localInputRecord = new List<PlayerInputs>();

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
                pi.LeftStick.x = (FPValue)gamepad.leftStick.ReadValue().x;
                pi.LeftStick.y = (FPValue)gamepad.leftStick.ReadValue().y;
                pi.Attack = gamepad.buttonSouth.ReadValue() == 1 ? true : false;
            }
            localInputRecord.Add(pi);
        }

        #region Inputs
        public virtual PlayerInputDefinition LeftStick() {
            PlayerInputDefinition pid = new PlayerInputDefinition();
            if (localInputRecord.Count == 0) {
                return pid;
            } else {
                pid.axis.x = localInputRecord[localInputRecord.Count - 1].LeftStick.x;
                pid.axis.y = localInputRecord[localInputRecord.Count - 1].LeftStick.y;
                return pid;
            }
        }

        public virtual PlayerInputDefinition Attack() {
            PlayerInputDefinition pid = new PlayerInputDefinition();
            if(localInputRecord.Count == 0) {
                return pid;
            } else {
                if (localInputRecord.Count >= 2) {
                    if (!localInputRecord[localInputRecord.Count - 2].Attack) {
                        //First press
                        pid.firstPress = localInputRecord[localInputRecord.Count - 1].Attack;
                    }
                }
                return pid;
            }
        }
        #endregion
    }
}
