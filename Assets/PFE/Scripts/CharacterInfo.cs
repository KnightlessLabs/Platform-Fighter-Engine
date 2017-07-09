using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInfo : ScriptableObject {
    public string Name;
    public Sprite BigPortrait;
    public Sprite SmallPortrait;
    public Sprite CSSPortrait;
    public Sprite StockIcon;

    public int HealthPoints;
    public int MaxMeter;

    public List<GameObject> Costumes = new List<GameObject>();

    public HurtboxSetupHolder HurtboxSetupInfo;
    public AttributesHolder AttributesInfo;

    //Movesets
    public float ExecutionTiming = 0.3f;
    public float BlendingDuration = 0.1f;
    public List<CombatStanceHolder> CombatStances = new List<CombatStanceHolder>();
}
