using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace PFE.Networking {
    public class NetServer : MonoBehaviour {

        public static bool isHosting = false;

        public delegate void StartHostingAction();
        public static event StartHostingAction OnStartHosting;
        public delegate void StopHostingAction();
        public static event StopHostingAction OnStopHosting;

        #region Connecting
        public void StartHosting(ConnectionConfig cc, int maxPlayersPerRoom, out int socketId, out int webSocketId, int socketPort) {
            NetworkTransport.Init(NetManager.instance.networkConfiguration);

            HostTopology topology = new HostTopology(cc, maxPlayersPerRoom);
            socketId = NetworkTransport.AddHost(topology, socketPort, null);
            webSocketId = NetworkTransport.AddWebsocketHost(topology, socketPort, null);

            if (socketId == -1) {
                Destroy(this);
            } else {
                isHosting = true;
                Debug.Log("Started hosting on port " + socketPort);
                if (OnStartHosting != null) {
                    OnStartHosting();
                }
            }
        }

        public void StopHosting() {
            Debug.Log("Stopped hosting.");
            if (OnStopHosting != null) {
                OnStopHosting();
            }
            NetworkTransport.RemoveHost(NetManager.instance.socketId);
            NetworkTransport.RemoveHost(NetManager.instance.webSocketId);
            NetworkTransport.Shutdown();
            isHosting = false;
            Destroy(this);
        }
        #endregion

        void Update() {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType recData;

            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            if (error != 0) {
                Debug.Log((NetworkError)error);
            }
            switch (recData) {
                case NetworkEventType.ConnectEvent:    //2
                    NetManager.instance.OnClientConnectToHost(connectionId);
                    Debug.Log("Client connected.");
                    break;
                case NetworkEventType.DataEvent:       //3
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    string[] splitData = msg.Split('^');
                    break;
                case NetworkEventType.DisconnectEvent: //4
                    isHosting = false;
                    Debug.Log("Client disconnected.");
                    Destroy(this);
                    break;
            }
        }

        #region Send Messages
        public void Send(string message, int channelId, int connectionId) {
            List<ServersClientDefinition> targets = new List<ServersClientDefinition>();
            targets.Add(NetManager.instance.clients.Find(x => x.connectionId == connectionId));
            Send(message, channelId, targets);
        }

        public void SendAll(string message, int channelId) {
            Send(message, channelId, NetManager.instance.clients);
        }

        public void Send(string message, int channelId, List<ServersClientDefinition> targetClients) {
            byte error;
            byte[] msg = Encoding.Unicode.GetBytes(message);
            foreach (ServersClientDefinition sc in targetClients) {
                NetworkTransport.Send(NetManager.instance.SocketID, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
                if (error != 0) {
                    Debug.Log((NetworkError)error);
                }
            }
        }
        #endregion
    }
}