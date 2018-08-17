using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Experimental.Input;

namespace PFE.Core {
    public class GameManager : MonoBehaviour {

        #region References
        public static GameManager instance;
        [HideInInspector]
        public MatchManager matchManager;
        [HideInInspector]
        public VCR Recorder;

        [Header("References")]
        public NetworkType networkType;
        public GameState gameState;
        public GameModeEnum gameMode;
        public CanvasGroupFader loadingScreenCG;
        //Character Select
        [Header("Character Select")]
        public GameObject cSelectScreenGO;
        public Transform cSelectCharactersHolder;
        public GameObject cSelectCharacterPrefab;
        public GameObject cSelectCamera;
        public GameObject[] cSelectPositions;
        public int selectingFor = 0;
        //Stage Select
        [Header("Stage Select")]
        public GameObject sSelectScreenGO;
        public GameObject sSelectStageHolder;
        public GameObject sSelectStagePrefab;
        #endregion

        [Header("Variables")]
        public GameInfo gameInfo;
        public GameSettingsData gameSettings;

        #region Events
        public delegate void FinishedSceneLoadAction();
        public event FinishedSceneLoadAction OnSceneFinishedLoading;
        public delegate void CharacterSelectOpenedAction();
        public event CharacterSelectOpenedAction OnCharacterSelectOpened;
        #endregion

        public virtual void Awake() {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

            matchManager = GetComponent<MatchManager>();
            Recorder = GetComponent<VCR>();
        }

        #region Character Select
        public void OpenCharacterSelect() {
            cSelectScreenGO.SetActive(true);
            cSelectCamera.SetActive(true);

            //Cleanup CSS
            foreach(Transform t in cSelectCharactersHolder) {
                Destroy(t.gameObject);
            }

            //Build CSS
            for(int i = 0; i < gameInfo.characters.Length; i++) {
                CharacterInfo ci = gameInfo.characters[i];
                GameObject go = Instantiate(cSelectCharacterPrefab, cSelectCharactersHolder, false);
                if(ci.CSSPortrait != null) {
                    go.GetComponent<Image>().sprite = ci.CSSPortrait;
                    go.GetComponentInChildren<TextMeshProUGUI>().text = ci.characterName;
                    go.GetComponent<CSSCharacterIcon>().characterIndex = i;
                }
            }

            gameState = GameState.CharacterSelection;
            if (OnCharacterSelectOpened != null) {
                OnCharacterSelectOpened();
            }
        }
        #endregion

        #region Stage Select
        public void OpenStageSelect() {
            sSelectScreenGO.SetActive(true);
            cSelectScreenGO.SetActive(false);
            cSelectCamera.SetActive(false);

            //Cleanup
            foreach(Transform t in sSelectStageHolder.transform) {
                Destroy(t.gameObject);
            }

            //Create SSS
            for(int i = 0; i < gameInfo.stages.Length; i++) {
                GameObject go = Instantiate(sSelectStagePrefab, sSelectStageHolder.transform, false);

                StageInfo sN = gameInfo.stages[i];
                EventTrigger et = go.GetComponent<EventTrigger>();
                EventTrigger.Entry entryT = new EventTrigger.Entry();
                entryT.eventID = EventTriggerType.Submit;
                entryT.callback.AddListener((eventData) => { StartMatch(sN); });
                et.triggers.Add(entryT);

                EventTrigger.Entry entryCl = new EventTrigger.Entry();
                entryCl.eventID = EventTriggerType.PointerClick;
                entryCl.callback.AddListener((eventData) => { StartMatch(sN); });
                et.triggers.Add(entryCl);
            }
        }
        #endregion

        #region Scene Loading
        public void LoadScene(string SceneName) {
            StartCoroutine(LoadSceneEnumerator(SceneName));
        }

        IEnumerator LoadSceneEnumerator(string SceneName) {
            loadingScreenCG.FadeOut(2);
            if (SceneManager.sceneCount > 1) {
                //Only unload if there's more than 1 scene currently loaded.
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
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
        #endregion

        public void StartMatch(StageInfo sI) {
            //Delete pedestal characters
            for (int i = 0; i < cSelectPositions.Length; i++) {
                foreach (Transform t in cSelectPositions[i].transform) {
                    Destroy(t.gameObject);
                }
            }

            //Setup MatchManager
            matchManager.stageInfo = sI;
            OnSceneFinishedLoading += matchManager.StartMatch;
            sSelectScreenGO.SetActive(false);
            LoadScene(sI.stageSceneName);
        }
    }
}
