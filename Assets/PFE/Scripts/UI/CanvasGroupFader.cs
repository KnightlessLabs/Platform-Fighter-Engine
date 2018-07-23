using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupFader : MonoBehaviour {

    public CanvasGroup cGroup;

    public void FadeOut(float multi) {
        if (cGroup.alpha != 1) {
            StartCoroutine(FadeOutEnumerator(multi));
        }
    }

    public void FadeIn(float multi) {
        if (cGroup.alpha != 0) {
            StartCoroutine(FadeInEnumerator(multi));
        }
    }

    IEnumerator FadeOutEnumerator(float multi) {
        float t = 0;
        cGroup.alpha = 0;
        while (t < 1) {
            t += Time.deltaTime * multi;
            cGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeInEnumerator(float multi) {
        float t = 0;
        cGroup.alpha = 1;
        while (t < 1) {
            t += Time.deltaTime * multi;
            cGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return new WaitForEndOfFrame();
        }
    }
}