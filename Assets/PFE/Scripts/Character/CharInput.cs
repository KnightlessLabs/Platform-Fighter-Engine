using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInput : GSBehavior {

    [Header("References")]
	public CharController charController;

    [Header("Variables")]
	public List<LocalPlayerInputs> localInputRecord = new List<LocalPlayerInputs>();

    public override void GSUpdate() {

        charController.GSUpdate();
    }

    public override void GSLateUpdate() {
        charController.GSLateUpdate();
    }

    public virtual LocalPlayerInputs HandleInput() {
        return new LocalPlayerInputs();
    }
}
