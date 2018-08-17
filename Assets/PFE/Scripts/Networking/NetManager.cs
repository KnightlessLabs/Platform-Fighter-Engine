using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

namespace PFE.Networking {
    public class NetManager : MonoBehaviour {

        public static NetManager instance;
        public GlobalConfig networkConfiguration;
        public ConnectionConfig connectionConfig;
        public int maxPlayersPerRoom = 2;

        [Header("General Information")]
        public int reliableChannel = -1;
        public int unreliableChannel = -1;

        [Header("Hosting Information")]
        public int socketId = -1;
        public int webSocketId = -1;
        public int socketPort = 5002;
        public List<ServersClientDefinition> clients = new List<ServersClientDefinition>();
        [HideInInspector]
        public NetServer networkServer;

        [Header("Client information")]
        public int connectionId = -1;
        public float connectionTime;
        public Text debugText;
        [HideInInspector]
        public NetClient networkClient;

        public virtual void Awake() {
            instance = this;
        }

        public virtual void InitNetworking() {
            NetworkTransport.Init(networkConfiguration);
            connectionConfig.Channels.Clear();
            reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
            unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);
        }

        public virtual void StartHosting() {
            if (networkServer == null) {
                InitNetworking();

                networkServer = gameObject.AddComponent<NetServer>();
                networkServer.StartHosting(connectionConfig, maxPlayersPerRoom, out socketId, out webSocketId, socketPort);
            } else {
                Debug.Log("Already hosting!");
            }
        }

        public virtual void ConnectToHost(string ipAdress) {
            if (networkClient == null) {
                InitNetworking();

                networkClient = gameObject.AddComponent<NetClient>();
                networkClient.ConnectToHost(ipAdress, connectionConfig, maxPlayersPerRoom, ref connectionId, ref socketId, socketPort, ref connectionTime);
            }
        }

        #region Events
        public void OnClientConnectToHost(int connectionID) {
            ServersClientDefinition sc = new ServersClientDefinition();
            sc.connectionId = connectionID;
            clients.Add(sc);
        }
        #endregion

        #region Attributes
        public int SocketID {
            get {
                return socketId;
            }
        }
        #endregion
    }
}