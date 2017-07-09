using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class GameManager : TrueSyncBehaviour {

    public static GameManager instance;
    public GameState GameS;
    public MatchState MatchS;
    public GameInfo GInfo;
    public VCR Recorder;
    public List<CInput> Players = new List<CInput>();
    public bool TapJump = false; //Tap jump on/off
    public List<CharacterInfo> PlayerCharacters = new List<CharacterInfo>(); //Support multiple players on same game

    public int PlayerNumber = 0; //Used online

    void Start () {
        DontDestroyOnLoad(gameObject);
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        if (PhotonNetwork.inRoom) {
            if (PhotonNetwork.offlineMode) {
                for (int i = 0; i < PlayerCharacters.Count; i++) {
                    TrueSyncManager.SyncedInstantiate(PlayerCharacters[i].Costumes[0], StageInfo.Instance.SpawnPoints[0].position, TSQuaternion.identity);
                }
            } else {
                for (int i = 0; i < PlayerCharacters.Count; i++) {
                    TrueSyncManager.SyncedInstantiate(PlayerCharacters[i].Costumes[0], StageInfo.Instance.SpawnPoints[PlayerNumber].position, TSQuaternion.identity);
                }
            }
        }
    }
}
