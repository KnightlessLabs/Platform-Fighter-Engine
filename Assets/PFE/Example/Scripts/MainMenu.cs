using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class MainMenu : Photon.MonoBehaviour{

    public static MainMenu instance;
    public GameObject MainMenuGO;
    public GameObject OnlineVSGO;
    public GameObject CharacterSelectGO;
    public CSSInfo CSSInfoHolder;
    public CSSCharacterUI[] CurrentSelectedCharacter;

    [System.Serializable]
    public class CSSInfo {
        public Transform CSSParent;
        public Transform CSSCharactersParent;
        public CSSCharacterUI CSSCharacterTemplate;
        public CSSPanelUI[] PlayerPanels;
    }

    private void Awake () {
        instance = this;
    }

    void Start () {
        MainMenuGO.SetActive(true);
        OnlineVSGO.SetActive(false);
        CharacterSelectGO.SetActive(false);
        //Build CSS characters
        for (int i = 0; i < GameManager.instance.GInfo.Characters.Length; i++) {
            GameObject temp = GameObject.Instantiate(CSSInfoHolder.CSSCharacterTemplate.gameObject, CSSInfoHolder.CSSCharactersParent, false);
            CSSCharacterUI tempUI = temp.GetComponent<CSSCharacterUI>();
            tempUI.Name.text = GameManager.instance.GInfo.Characters[i].Name;
        }
    }

    private void Update () {
    }

    //Local
    public void LocalMatchButton () {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("LocalMatch");
    }

    //Online
    public void OnlineButton () {
        PhotonNetwork.ConnectUsingSettings("v1.0");
        PhotonNetwork.automaticallySyncScene = true;
        MainMenuGO.SetActive(false);
        OnlineVSGO.SetActive(true);
        CharacterSelectGO.SetActive(false);
    }

    public void BackOutOnline () {
        PhotonNetwork.Disconnect();
        MainMenuGO.SetActive(true);
        OnlineVSGO.SetActive(false);
        CharacterSelectGO.SetActive(false);
    }

    public void CreateRoom () {
        PhotonNetwork.CreateRoom("Random Lobby" + Random.Range(0, 999));
    }

    public void JoinRandomRoom () {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoomButton () {
        PhotonNetwork.LeaveRoom();
    }

    public void ReadyUpButton () {
        if (PhotonNetwork.offlineMode) {
            PhotonNetwork.LoadLevel("BattleArena");
        }
    }

    public void OnJoinedRoom () {
        MainMenuGO.SetActive(false);
        OnlineVSGO.SetActive(false);
        CharacterSelectGO.SetActive(true);
    }

    public void OnLeftRoom () {
        if (PhotonNetwork.offlineMode) {
            MainMenuGO.SetActive(true);
            OnlineVSGO.SetActive(false);
            CharacterSelectGO.SetActive(false);
        } else {
            MainMenuGO.SetActive(false);
            OnlineVSGO.SetActive(true);
            CharacterSelectGO.SetActive(false);
        }
    }
}
