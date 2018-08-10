using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PFE.Core;
using PFE.Networking;

namespace PFE.Example {
    public class MainMenuHandler : MonoBehaviour {

        public GameObject mainMenuGO;
        public GameObject defaultSelection;

        private void Start() {
            EventSystem.current.SetSelectedGameObject(defaultSelection);
        }


        public void OpenTrainingMode() {
            mainMenuGO.SetActive(false);
            GameManager.instance.gameMode = GameModeEnum.Training;
            GameManager.instance.OpenCharacterSelect();
        }

        public void CreateLobby() {
            NetManager.instance.StartHosting();
        }

        public void MMJoinLobby() {

        }
    }
}
