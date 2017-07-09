using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSSPanelUI : MonoBehaviour {

    public CSSPlayerType PlayerType = CSSPlayerType.None;
    public int PanelNumber = 0;
    public Image CharacterImage;
    public Button TypeOfPlayerB;
    public Text TypeOfPlayerT;
    public Dropdown Tags;
    public GameObject CurrentChip;

	void OnEnable () {
        TypeOfPlayerB.onClick.AddListener(() => ChangePlayerType());
	}

    void OnDisable () {
        TypeOfPlayerB.onClick.RemoveAllListeners();
    }

    void Update () {
        
    }

    public void ChangePlayerType () {
        if(PlayerType == CSSPlayerType.None) {
            PlayerType = CSSPlayerType.Human;
            TypeOfPlayerT.text = "HMN";
            Tags.gameObject.SetActive(true);
            CreateChip();
        } else if (PlayerType == CSSPlayerType.Human) {
            PlayerType = CSSPlayerType.CPU;
            TypeOfPlayerT.text = "CPU";
            Tags.gameObject.SetActive(false);
            CreateChip();
        } else {
            PlayerType = CSSPlayerType.None;
            PhotonNetwork.Destroy(CurrentChip);
            TypeOfPlayerT.text = "None";
            Tags.gameObject.SetActive(false);
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
