using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;


public class PFEAnimator : MonoBehaviour {

    public Animator Anim;

    //Playable API
    public PlayableGraph playableGraph;
    public float CurrentAnimationTimeline = 0.0f;
    public bool AnimStarted = false;

    public void PlayAnimation ( AnimationClip clip, bool AutoPlay ) {
        if (clip != null) {
            if (AnimStarted) {
                playableGraph.Destroy();
            }
            Anim.speed = 1;
            AnimationPlayableUtilities.PlayClip(Anim, clip, out playableGraph);
            AnimStarted = true;
        } else {
            if (AnimStarted) {
                playableGraph.Destroy();
            }
        }
    }

    public void PlayAnimation ( AnimationClip clip, bool AutoPlay, float Speed ) {
        if (clip != null) {
            if (AnimStarted) {
                playableGraph.Destroy();
            }
            AnimationClipPlayable pp = AnimationPlayableUtilities.PlayClip(Anim, clip, out playableGraph);
            pp.SetSpeed(Speed);
            AnimStarted = true;
        } else {
            if (AnimStarted) {
                playableGraph.Destroy();
            }
        }
    }

    public void PauseAnimation () {
        playableGraph.Stop();
    }

    public void PlayAnimation () {
        playableGraph.Play();
    }
}
