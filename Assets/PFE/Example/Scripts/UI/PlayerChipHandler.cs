using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFE.Core;
using TMPro;

namespace PFE.Example {
    public class PlayerChipHandler : MonoBehaviour {
        public TextMeshProUGUI chipName;
        public int playerSlot = -1;
        public PlayerType playerType = PlayerType.Player;
    }
}
