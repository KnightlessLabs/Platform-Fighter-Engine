using UnityEngine;
using UnityEditor;

public class CharacterInfoAsset : MonoBehaviour {
    [MenuItem("Assets/Create/PFE/CharacterInfo")]
    public static void CreateAsset () {
        CustomAssetUtility.CreateAsset<CharacterInfo>();
    }
}
