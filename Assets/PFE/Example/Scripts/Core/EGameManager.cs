using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PFE.Example{
    public class EGameManager : GameManager{

        [Header("CSS Chip Info")]
        public GameObject chipCanvas;
        public PlayerChipHandler cssChipPrefab;
        public List<PlayerChipHandler> cssChips = new List<PlayerChipHandler>();
        [Header("Cursor Info")]
        public GameObject cursorCanvas;
        public PlayerCursorHandler cursorPrefab;
        public List<PlayerCursorHandler> cursors = new List<PlayerCursorHandler>();

        public List<GraphicRaycaster> m_Raycasters = new List<GraphicRaycaster>();
        [HideInInspector]
        public PointerEventData m_PointerEventData;
        public EventSystem m_EventSystem;

        public float curSpeed = 0.1f;

        public override void Awake() {
            base.Awake();

            foreach (Transform t in cursorCanvas.transform) {
                Destroy(t.gameObject);
            }
        }

        public void Update() {
            if (gameState == GameState.CharacterSelection) {
                foreach (Gamepad gp in Gamepad.all) {
                    if (gp.startButton.wasJustPressed) {
                        foreach(PlayerCursorHandler c in cursors) {
                            if(c.myGamepad == gp) {
                                return;
                            }
                        }
                        int tm = matchManager.getOpenSlot();
                        if (tm != -1) {
                            CreateCursor(gp, tm);
                        }
                    }
                }
            }
        }

        public void CreateCSSChips(PlayerCursorHandler pch, int playerSlot) {
            GameObject ch = Instantiate(cssChipPrefab.gameObject, chipCanvas.transform, false);
            ch.transform.position += new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            if (pch) {
                cssChips.Add(ch.GetComponent<PlayerChipHandler>());
                pch.myChip = ch.GetComponent<PlayerChipHandler>();
                pch.currentHeldChip = pch.myChip;
                ch.GetComponent<PlayerChipHandler>().chipName.text = "PLA " + playerSlot;
            } else {
                //CPU chip
                cssChips.Add(ch.GetComponent<PlayerChipHandler>());
                ch.GetComponent<PlayerChipHandler>().chipName.text = "CPU " + playerSlot;
            }
            ch.GetComponent<PlayerChipHandler>().playerSlot = playerSlot;
        }

        #region Virtual Cursors
        public void CreateCursor(Gamepad gp, int playerCSSSlot) {
            //Create cursors
            GameObject pc = Instantiate(cursorPrefab.gameObject, cursorCanvas.transform, false);
            pc.transform.position += new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            pc.GetComponent<Image>().color = Random.ColorHSV();
            pc.GetComponent<PlayerCursorHandler>().myGamepad = gp;
            cursors.Add(pc.GetComponent<PlayerCursorHandler>());
            CreateCSSChips(pc.GetComponent<PlayerCursorHandler>(), playerCSSSlot);
        }

        public void DestroyCursors() {
            //Destroy all cursors
            for (int i = 0; i < cursors.Count; i++) {
                Destroy(cursors[i].gameObject);
                cursors.RemoveAt(i);
            }
        }
        #endregion

        #region Helpers
        public void AddGraphicsRaycaster(GraphicRaycaster gr) {
            if (gr != null) {
                m_Raycasters.Add(gr);
            }
        }
        #endregion
    }
}