using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Character;
using PFE.Core;

namespace PFE.Core {
    public class MatchManager : MonoBehaviour {

        public MatchState matchState;
        public StageInfo stageInfo;
        public List<CharInput> playersInMatch = new List<CharInput>();
        [Header("Debug")]
        public List<PlayerDefinition> cssCharacterSlots = new List<PlayerDefinition>(4);

        public void StartMatch() {
            for(int i = 0; i < cssCharacterSlots.Count; i++) {
                if (cssCharacterSlots[i].playerType != PlayerType.None) {
                    GameObject gO = Instantiate(cssCharacterSlots[i].selectedChar.prefab, stageInfo.spawnPositions[i], Quaternion.identity);
                }
            }

            GameManager.instance.OnSceneFinishedLoading -= StartMatch;

            GameStepManager.instance.StartGSManager();
        }

        public int getOpenSlot() {
            for(int i = 0; i < cssCharacterSlots.Count; i++) {
                if(cssCharacterSlots[i].playerType == PlayerType.None) {
                    cssCharacterSlots[i].playerType = PlayerType.Player;
                    return i;
                }
            }
            return -1;
        }
    }
}
