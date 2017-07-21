using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class CSSCursor : MonoBehaviour {

    public InputDevice Controller;
    public float CursorSpeed = 6;
    public CSSChip GrabbedChip;
    public int PlayerNumber = 0; 
    public int SelectedCharacter = -1;

	void Update () {
        //Set as top UI element.
        transform.SetAsLastSibling();

        if (Controller.Name == "Keyboard/Mouse") {
            transform.position = Input.mousePosition;
        } else {
            transform.position += new Vector3(Controller.LeftStickX * CursorSpeed, Controller.LeftStickY * CursorSpeed, 0);
            if (Controller.Action1.WasPressed) {
                //Code to be place in a MonoBehaviour with a GraphicRaycaster component
                GraphicRaycaster gr = MainMenu.instance.GetComponent<GraphicRaycaster>();
                //Create the PointerEventData with null for the EventSystem
                PointerEventData ped = new PointerEventData(null);
                //Set required parameters, in this case, mouse position
                ped.position = transform.position;
                //Create list to receive all results
                List<RaycastResult> results = new List<RaycastResult>();
                //Raycast it
                gr.Raycast(ped, results);

                foreach (RaycastResult result in results) {
                    if (GrabbedChip) {
                        if (result.gameObject.GetComponent<CSSCharacterUI>()) {
                            Debug.Log("Selected character " + result.gameObject.GetComponent<CSSCharacterUI>().Name.text);
                            MainMenu.instance.CSSInfoHolder.PlayerPanels[GrabbedChip.CSSPanelNumber].CharacterImage.sprite = GameManager.instance.GInfo.Characters[0].BigPortrait;
                            SelectedCharacter = result.gameObject.GetComponent<CSSCharacterUI>().CharacterID;
                            GameManager.instance.PlayerCharacters[PlayerNumber] = GameManager.instance.GInfo.Characters[SelectedCharacter];
                        }
                    }
                }

                foreach (RaycastResult result in results) {
                    if (result.gameObject.GetComponent<Button>()) {
                        result.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    if (result.gameObject.GetComponent<CSSChip>()) {
                        if (GrabbedChip == null) {
                            GrabbedChip = result.gameObject.GetComponent<CSSChip>();
                        } else {
                            GrabbedChip = null;
                        }
                    }
                }
            }
        }

        if (GrabbedChip) {
            GrabbedChip.transform.position = transform.position;
        }
    }
}
