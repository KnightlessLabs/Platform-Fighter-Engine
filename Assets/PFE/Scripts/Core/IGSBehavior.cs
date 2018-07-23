using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGSBehavior{
        GameStepManager Lsm { get; }

        void GSAwake();
        void GSStart();
        void GSUpdate();
        void GSLateUpdate();
}
