using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using PFE.Core;
using PFE.Networking;
using UnityEngine.UI;

namespace PFE.Example {
    public class MainMenuHandler : MonoBehaviour {

        public GameObject mainMenuGO;
        public GameObject onlineDropdownGO;
        public GameObject defaultSelection;

        private void Start() {
            if(SceneManager.GetSceneByName("Managers") == null)
            {
                Debug.LogError("Load the 'Managers' scene first!");
            }
            EventSystem.current.SetSelectedGameObject(defaultSelection);
            ((EGameManager)(GameManager.instance)).AddGraphicsRaycaster(GetComponent<GraphicRaycaster>());
        }

        private void OnEnable() {
            NetClient.OnConnectToHost += CloseMainMenu;
            NetServer.OnStartHosting += CloseMainMenu;
            NetServer.OnStopHosting += OpenMainMenu;
        }

        private void OnDisable() {
            NetClient.OnConnectToHost -= CloseMainMenu;
            NetServer.OnStartHosting -= CloseMainMenu;
        }

        public void CloseMainMenu() {
            mainMenuGO.SetActive(false);
            onlineDropdownGO.SetActive(false);
        }

        public void OpenMainMenu() {
            mainMenuGO.SetActive(true);
        }

        public void OpenTrainingMode() {
            mainMenuGO.SetActive(false);
            GameManager.instance.gameMode = GameModeEnum.Training;
            GameManager.instance.OpenCharacterSelect();
        }

        public void CreateLobby() {
            NetManager.instance.StartHosting();
            GameManager.instance.gameMode = GameModeEnum.Versus;
        }

        public void MMJoinLobby() {
            ((ENetManager)NetManager.instance).OpenConnectionScreen();
        }
    }
}
