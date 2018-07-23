using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region References
    public static GameManager instance;
    public GameState GameS;
    public MatchManager matchManager;
    public GameInfo GInfo;
    public VCR Recorder;
    public CanvasGroupFader loadingScreenCG;
    #endregion

    #region variables
    public bool TapJump = false; //Tap jump on/off
    #endregion

    public delegate void FinishedSceneLoadAction();
    public event FinishedSceneLoadAction OnSceneFinishedLoading;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string SceneName) {
        StartCoroutine(LoadSceneEnumerator(SceneName));
    }

    IEnumerator LoadSceneEnumerator(string SceneName) {
        loadingScreenCG.FadeOut(2);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        while (!async.isDone) {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        if (OnSceneFinishedLoading != null) {
            OnSceneFinishedLoading();
        }
        loadingScreenCG.FadeIn(1);
    }
}
