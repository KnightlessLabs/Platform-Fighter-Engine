using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using PFE.Core;

namespace PFE.Networking {
    public class NetClient : MonoBehaviour {

        public static bool isConnected;

        public delegate void ConnectToHostAction();
        public static event ConnectToHostAction OnConnectToHost;
        public delegate void DisconnectFromHostAction();
        public static event DisconnectFromHostAction OnDisconnectFromHost;

        public delegate void StageChangedAction(int index);
        public static event StageChangedAction OnStageChanged;

        #region Connecting
        public void ConnectToHost(string ipAdress, ConnectionConfig cc, int maxPlayersPerRoom, ref int connectionId, ref int socketId, int socketPort, ref float connectionTime) {
            NetworkTransport.Init(NetManager.instance.networkConfiguration);

            HostTopology topology = new HostTopology(cc, maxPlayersPerRoom);
            int sId = NetworkTransport.AddHost(topology, socketPort + 1, null);
            socketId = sId;

            byte error;
            connectionId = NetworkTransport.Connect(sId, ipAdress, socketPort, 0, out error);
            connectionTime = Time.time;

            if (error != 0) {
                if (OnDisconnectFromHost != null){
                    OnDisconnectFromHost();
                }
                Debug.Log((NetworkError)error);
                Destroy(this);
            }
        }

        public void DisconnectFromHost() {
            NetworkTransport.RemoveHost(NetManager.instance.socketId);
            NetworkTransport.Shutdown();
            isConnected = false;
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

            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            if (error != 0) {
                Debug.Log((NetworkError)error);
            }
            switch (recData) {
                case NetworkEventType.ConnectEvent:    //2
                    isConnected = true;
                    Debug.Log("Connected to host.");
                    if (OnConnectToHost != null) {
                        OnConnectToHost();
                    }
                    break;
                case NetworkEventType.DataEvent:       //3
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    string[] splitData = msg.Split('^');
                    break;
                case NetworkEventType.DisconnectEvent: //4
                    Debug.Log("Disconnected from host.");
                    isConnected = false;
                    if (OnDisconnectFromHost != null) {
                        OnDisconnectFromHost();
                    }
                    Destroy(this);
                    break;
            }
        }

        #region Send Messages
        public void Send(string message, int channelId, int connectionId) {
            byte error;
            byte[] msg = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(NetManager.instance.socketId, connectionId, channelId, msg, message.Length * sizeof(char), out error);
        }
        #endregion
    }
}
