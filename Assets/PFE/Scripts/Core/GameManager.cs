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
        [Header("References")]
        public GameState gameState;
        public GameModeEnum gameMode;
        public MatchManager matchManager;
        public VCR Recorder;
        public CanvasGroupFader loadingScreenCG;
        //Character Select
        public GameObject cSelectScreenGO;
        public Transform cSelectCharactersHolder;
        public GameObject cSelectCharacterPrefab;
        public GameObject cSelectCamera;
        public GameObject[] cSelectPositions;
        public int selectingFor = 0;
        //Stage Select
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
        #endregion

        void Awake() {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }
        }

        #region Character Select
        public void OpenCharacterSelect() {
            cSelectScreenGO.SetActive(true);
            cSelectCamera.SetActive(true);
            foreach(Transform t in cSelectCharactersHolder) {
                Destroy(t.gameObject);
            }

            for(int i = 0; i < gameInfo.characters.Length; i++) {
                CharacterInfo ci = gameInfo.characters[i];
                GameObject go = Instantiate(cSelectCharacterPrefab, cSelectCharactersHolder, false);
                if(ci.CSSPortrait != null) {
                    go.GetComponent<Image>().sprite = ci.CSSPortrait;
                    go.GetComponentInChildren<TextMeshProUGUI>().text = ci.characterName;
                }

                int ind = i;
                EventTrigger et = go.GetComponent<EventTrigger>();
                EventTrigger.Entry entryT = new EventTrigger.Entry();
                entryT.eventID = EventTriggerType.Submit;
                entryT.callback.AddListener((eventData) => { OnSelectCharacter(ind); });
                et.triggers.Add(entryT);

                EventTrigger.Entry entryCl = new EventTrigger.Entry();
                entryCl.eventID = EventTriggerType.PointerClick;
                entryCl.callback.AddListener((eventData) => { OnSelectCharacter(ind); });
                et.triggers.Add(entryCl);

                if ((i+1) == Mathf.Ceil((float)(gameInfo.characters.Length) / 2.0f)) {
                    EventSystem.current.SetSelectedGameObject(go);
                }
            }
        }

        public void OnSelectCharacter(int index) {
            if(matchManager.playerCharacters.Count < 2) {
                matchManager.playerCharacters.Add(gameInfo.characters[index]);
                GameObject go = GameObject.Instantiate(gameInfo.characters[index].costumes[0], cSelectPositions[selectingFor].transform, false);
                go.transform.eulerAngles = new Vector3(0, 180, 0);
                selectingFor++;
                if(selectingFor == 2) {
                    OpenStageSelect();
                }
            }
        }
        #endregion

        #region Stage Select
        public void OpenStageSelect() {
            sSelectScreenGO.SetActive(true);
            cSelectScreenGO.SetActive(false);
            cSelectCamera.SetActive(false);

            foreach(Transform t in sSelectStageHolder.transform) {
                Destroy(t.gameObject);
            }

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

        public void StartMatch(StageInfo sI) {
            for(int i = 0; i < cSelectPositions.Length; i++) {
                foreach (Transform t in cSelectPositions[i].transform) {
                    Destroy(t.gameObject);
                }
            }

            matchManager.stageInfo = sI;
            OnSceneFinishedLoading += matchManager.StartMatch;
            sSelectScreenGO.SetActive(false);
            LoadScene(sI.stageSceneName);
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
}
