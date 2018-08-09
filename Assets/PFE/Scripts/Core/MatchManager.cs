using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Character;
using PFE.Core;

namespace PFE {
    public class MatchManager : MonoBehaviour {
        public MatchState matchState;
        public bool VCR = false;

        public StageInfo stageInfo;
        public List<CharacterInfo> playerCharacters = new List<CharacterInfo>();
        public List<CharInput> playersInMatch = new List<CharInput>();

        public void StartMatch() {
            for(int i = 0; i < playerCharacters.Count; i++) {
                GameObject gO = Instantiate(playerCharacters[i].prefab, stageInfo.spawnPositions[i], Quaternion.identity);
            }

            GameManager.instance.OnSceneFinishedLoading -= StartMatch;

            GameStepManager.instance.StartGSManager();
        }
    }
}
