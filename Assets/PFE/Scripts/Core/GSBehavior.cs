using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSBehavior : MonoBehaviour, IGSBehavior {
        public GameStepManager Lsm {
            get {
                return GameStepManager.instance;
            }
        }

        void Awake() {
            GSAwake();
        }

        public virtual void Start() {
            if (!Lsm.gsBehaviors.Contains(this)) {
                Lsm.gsBehaviors.Add(this);
            }
            GSStart();
        }

        public virtual void GSAwake() {

        }
        public virtual void GSStart() {

        }

        public virtual void GSUpdate() {

        }

        public virtual void GSLateUpdate() {

        }
}
