using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PFE.Example {
    public class PlayerCursorHandler : MonoBehaviour {

        EGameManager gManager;
        RectTransform rectTransform;
        public Gamepad myGamepad;
        public PlayerChipHandler myChip;
        public PlayerChipHandler currentHeldChip;

        private void Start() {
            gManager = (EGameManager)GameManager.instance;
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            HandleMovement();

            //Make chip follow cursor
            if(currentHeldChip != null) {
                currentHeldChip.transform.position = transform.position;
            }

            //"Confirm" click
            if (myGamepad.buttonSouth.wasJustPressed) {
                HandleClick();
            }

            //"Back" click
            if (myGamepad.buttonEast.wasJustPressed) {
                if(currentHeldChip == null) {
                    currentHeldChip = myChip;
                }
            }

            //Start Button
            if (myGamepad.startButton.wasJustPressed) {
                gManager.OpenStageSelect();
            }
        }

        void HandleMovement() {
            Vector2 mov = new Vector2(myGamepad.leftStick.x.ReadValue(), myGamepad.leftStick.y.ReadValue());
            if (mov.magnitude < 0.1f) {
                mov = Vector2.zero;
            }
            float cSpeed = GameManager.instance.gameInfo.cursorSpeed;
            rectTransform.localPosition += new Vector3(mov.x * cSpeed * Time.deltaTime, mov.y * cSpeed * Time.deltaTime, 0);
        }

        void HandleClick() {
            //Set pointer position to the cursor's position
            gManager.m_PointerEventData = new PointerEventData(gManager.m_EventSystem);
            gManager.m_PointerEventData.position = transform.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            foreach (GraphicRaycaster gr in gManager.m_Raycasters) {
                if (gr != null) {
                    gr.Raycast(gManager.m_PointerEventData, results);
                }
            }

            foreach (RaycastResult result in results) {
                //Picking up chip
                if (currentHeldChip == null) {
                    if (result.gameObject.GetComponent<PlayerChipHandler>() != null) {
                        currentHeldChip = result.gameObject.GetComponent<PlayerChipHandler>();
                    }
                } else {
                    //Setting down chip
                    if (result.gameObject.GetComponent<CSSCharacterIcon>()) {
                        gManager.matchManager.cssCharacterSlots[currentHeldChip.playerSlot].selectedChar = gManager.gameInfo.characters[result.gameObject.GetComponent<CSSCharacterIcon>().characterIndex];
                        currentHeldChip = null;
                    }
                }

                if (result.gameObject.GetComponent<EventTrigger>()) {
                    var pointer = new PointerEventData(EventSystem.current);
                    ExecuteEvents.Execute(result.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
                    ExecuteEvents.Execute(result.gameObject, pointer, ExecuteEvents.pointerDownHandler);
                    ExecuteEvents.Execute(result.gameObject, pointer, ExecuteEvents.pointerUpHandler);
                    ExecuteEvents.Execute(result.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                }
            }
        }
    }
}
