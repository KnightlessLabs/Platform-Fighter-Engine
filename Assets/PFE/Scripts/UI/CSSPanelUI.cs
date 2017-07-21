using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class CSSPanelUI : MonoBehaviour {

    public InputDevice Controller;
    public CSSPlayerType PlayerType = CSSPlayerType.None;
    public int PanelNumber = 0;
    public GameObject CurrentChip;

    #region UI
    public Image CharacterImage;
    public Button TypeOfPlayerB;
    public Text TypeOfPlayerT;
    public Dropdown Tags;
    #endregion

    void OnEnable () {
        TypeOfPlayerB.onClick.AddListener(() => ChangePlayerType());
	}

    void OnDisable () {
        TypeOfPlayerB.onClick.RemoveAllListeners();
    }

    public void ChangePlayerType () {
        if(PlayerType == CSSPlayerType.None) {
            if (!MainMenu.instance.PlayerControllers.Contains(InputManager.ActiveDevice) || Controller == null) {
                PlayerType = CSSPlayerType.Human;
                TypeOfPlayerT.text = "HMN";
                Tags.gameObject.SetActive(true);
                CharacterImage.gameObject.SetActive(true);
                CreateChip();
                Controller = InputManager.ActiveDevice;
            }
        } else if (PlayerType == CSSPlayerType.Human) {
            Controller = null;
            PlayerType = CSSPlayerType.CPU;
            TypeOfPlayerT.text = "CPU";
            Tags.gameObject.SetActive(false);
            CharacterImage.gameObject.SetActive(true);
            CreateChip();
        } else {
            PlayerType = CSSPlayerType.None;
            PhotonNetwork.Destroy(CurrentChip);
            TypeOfPlayerT.text = "None";
            Tags.gameObject.SetActive(false);
            CharacterImage.gameObject.SetActive(false);
        }

    }

    public void CreateChip () {
        if (CurrentChip == null) {
            CurrentChip = PhotonNetwork.Instantiate("Chip", MainMenu.instance.CSSInfoHolder.CSSParent.position, Quaternion.identity, 0);
        }
        CurrentChip.GetComponent<PhotonView>().RPC("ChangeText", PhotonTargets.AllBuffered, TypeOfPlayerT.text);
        CurrentChip.GetComponent<PhotonView>().RPC("SetPanelNumber", PhotonTargets.AllBuffered, PanelNumber);
    }
}
