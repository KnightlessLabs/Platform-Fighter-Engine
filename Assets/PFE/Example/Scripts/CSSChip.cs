﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class CSSChip : MonoBehaviour {

    public int CSSPanelNumber = 0;
    public int CurrentState = 0; //0 = Dropped, 1 = Grabbed
    public Button ChipButton;
    public Text ChipText;

	void Awake () {
        transform.parent = MainMenu.instance.CSSInfoHolder.CSSParent;
        ChipButton.onClick.AddListener(() => ChangeState());
    }

    void OnDisable () {
        ChipButton.onClick.RemoveAllListeners();
    }

    //Pick up/drop chip
    public void ChangeState () {
        CurrentState = CurrentState == 0 ? 1 : 0;
    }

    [PunRPC]
    public void ChangeText (string Text) {
        ChipText.text = Text;
    }

    [PunRPC]
    public void SetPanelNumber (int PanelNum) {
        CSSPanelNumber = PanelNum;
    }
}