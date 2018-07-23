using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour {

    public static bool init = false;
    public string initialLevel = "MainMenu";

	void Start () {
        if (!init) {
            init = true;
            GameManager.instance.LoadScene(initialLevel);
        }
	}
}
