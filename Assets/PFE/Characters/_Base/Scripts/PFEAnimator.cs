using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;

public class PFEAnimator : MonoBehaviour {

    public Animator Anim;

    //Playable API
    public PlayableGraph playableGraph;
    public PlayableHandle playableHandle;
    public float CurrentAnimationTimeline = 0.0f;
    public bool AnimStarted = false;

    public void Awake () {
    }

    public void PlayAnimation (AnimationClip clip, bool AutoPlay) {
        if (clip != null) {
            if (AnimStarted) {
                playableHandle.Destroy();
                playableGraph.Destroy();
            }
            playableHandle = AnimationPlayableUtilities.PlayClip(Anim, clip, out playableGraph);
            if (!AutoPlay) {
                playableHandle.playState = PlayState.Paused;
            }
            AnimStarted = true;
        } else {
            if (AnimStarted) {
                playableHandle.Destroy();
                playableGraph.Destroy();
            }
        }
    }

    public void Update () {
    }
}
