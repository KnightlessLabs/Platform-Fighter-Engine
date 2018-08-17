using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFE {
    [System.Serializable]
    [CreateAssetMenu(menuName = "PFE/CharacterInfo", order = 2)]
    public class CharacterInfo : ScriptableObject {
        public string characterName;
        public Sprite bigPortrait;
        public Sprite smallPortrait;
        public Sprite CSSPortrait;
        public Sprite stockIcon;

        public int healthPoints;
        public int maxMeter;

        public GameObject prefab;
        public List<GameObject> costumes = new List<GameObject>();
        public List<GameObject> portraits = new List<GameObject>();

        public CharacterAttributes attributes;
    }
}