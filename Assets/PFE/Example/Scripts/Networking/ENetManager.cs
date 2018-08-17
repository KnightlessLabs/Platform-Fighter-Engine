using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PFE.Networking;
using PFE.Core;
using TMPro;

namespace PFE.Example {
    public class ENetManager : NetManager {

        [Header("UI")]
        public Button confirmConnectButton;
        public Button backOutOfConnectButton;
        public TMP_InputField ipField;
        public GameObject connectionScreen;

        public override void Awake() {
            base.Awake();
        }

        public void Start() {
            NetClient.OnConnectToHost += GameManager.instance.OpenCharacterSelect;
            NetServer.OnStartHosting += GameManager.instance.OpenCharacterSelect;
        }

        #region UI
        public void OpenConnectionScreen() {
            connectionScreen.SetActive(true);
        }

        public void ConfirmConnectToHost() {
            ConnectToHost(ipField.text);
            connectionScreen.SetActive(false);
            GameManager.instance.gameMode = GameModeEnum.Versus;
        }
        #endregion
    }
}