using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameSource.Character {
    public interface ICharacterState {
        void OnStart();
        void OnUpdate();
        bool CheckInterrupt();
        void OnInterrupted();
        string StateName { get; }
        int StateDuration { get; }
        CharInput inputHandler { get; set; }
        CharController controller { get; set; }
    }
}
