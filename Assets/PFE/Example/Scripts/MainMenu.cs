using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using InControl;

public class MainMenu : Photon.MonoBehaviour{

    public static MainMenu instance;
    public GameObject MainMenuGO;
    public GameObject OnlineVSGO;
    public GameObject CharacterSelectGO;
    public CSSInfo CSSInfoHolder;
    public List<InputDevice> PlayerControllers = new List<InputDevice>();
    public CSSCursor CursorPrefab;

    [System.Serializable]
    public class CSSInfo {
        public Transform CSSParent;
        public Transform CSSCharactersParent;
        public CSSCharacterUI CSSCharacterTemplate;
        public CSSPanelUI[] PlayerPanels;
        public List<CSSCursor> Cursors = new List<CSSCursor>();
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

    void Update () {
        switch (GameManager.instance.GameS) {
            case GameState.MainMenu:
                break;
            case GameState.CharacterSelection:
                if (InputManager.ActiveDevice.AnyButton) {
                    if (!PlayerControllers.Contains(InputManager.ActiveDevice)) {
                        PlayerControllers.Add(InputManager.ActiveDevice);
                        GameObject temp = GameObject.Instantiate(CursorPrefab.gameObject, CSSInfoHolder.CSSParent);
                        temp.GetComponent<CSSCursor>().Controller = InputManager.ActiveDevice;
                        CSSInfoHolder.Cursors.Add(temp.GetComponent<CSSCursor>());
                    }
                }
                if (InputManager.ActiveDevice.MenuWasPressed) {
                    PhotonNetwork.LoadLevel("BattleArena");
                }
                break;
        }
    }

    #region Buttons
    //Local
    public void LocalMatchButton () {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("LocalMatch");
        GameManager.instance.GameS = GameState.CharacterSelection;
    }

    public void TrainingModeButton () {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.CreateRoom("LocalMatch");
        GameManager.instance.GameS = GameState.CharacterSelection;
        PhotonNetwork.LoadLevel("BattleArena");
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
        GameManager.instance.GameS = GameState.MainMenu;
    }

    public void CreateRoom () {
        PhotonNetwork.CreateRoom("Random Lobby" + Random.Range(0, 999));
        GameManager.instance.GameS = GameState.CharacterSelection;
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
        GameManager.instance.GameS = GameState.MainMenu;
    }
    #endregion
}
