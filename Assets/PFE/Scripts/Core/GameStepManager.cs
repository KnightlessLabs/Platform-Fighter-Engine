using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStepManager : MonoBehaviour {
	public static GameStepManager instance;

	public bool simulate = false;
	public float timeScale = 1.0f;
	private float uTime = 0;

	float accumulator;
	//A framerate-independent interval that dictates when physics steps are done.
	public float fixedDelta = 0.01666667f;
	public float alpha;

	public List<IGSBehavior> gsBehaviors = new List<IGSBehavior>();

	//public FreeLookCam cam;
	private int simulationGroupMask;

	void Awake() {
		instance = this;
        simulate = false;
        Physics.autoSimulation = false;
        Physics2D.autoSimulation = false;
		accumulator = 0;
	}

	// Updates the physics
	void Update() {
		if (!simulate) {
			return;
		}

		float delta = Time.deltaTime;

		accumulator += delta * timeScale;

		if (accumulator > 0.2f) {
			accumulator = 0.2f;
		}

		while (accumulator >= fixedDelta) {
			Step(fixedDelta);
			uTime += fixedDelta;
			accumulator -= fixedDelta;
		}

		alpha = (float)(accumulator / fixedDelta);
	}

	public void Step(float fD) {
        //UPDATE//
        if (gsBehaviors.Count > 0) {
            for (int i = 0; i < gsBehaviors.Count; i++) {
                gsBehaviors[i].GSUpdate();
            }

            //LATE UPDATE//
            for (int i = 0; i < gsBehaviors.Count; i++) {
                gsBehaviors[i].GSLateUpdate();
            }
        }

		Physics.Simulate(fD);
		Physics2D.Simulate(fD);
	}

	public void DestroyObj(GSBehavior obj) {
		if (obj != null) {
			gsBehaviors.Remove(obj);
			Destroy(obj.gameObject);
		}
	}

	public float GetUTime {
		get {
			return uTime;
		}
	}

	public void StartGSManager() {
        simulate = true;
        accumulator = 0;
		alpha = 0;
		uTime = 0;
	    Physics.autoSimulation = false;
        Physics2D.autoSimulation = false;
	}

	public void PauseGSManager(){
		simulate = false;
		accumulator = 0;
	}

	public void ResumeGSManager(){
		simulate = true;
	}

	public void StopGSManager() {
		simulate = false;
		ResetLSManager();
	}

	public void ResetLSManager() {
		gsBehaviors = new List<IGSBehavior>();
	}
}
