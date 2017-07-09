using UnityEngine;
using UnityEditor;

public class GameInfoAsset : MonoBehaviour {
    [MenuItem("Assets/Create/PFE/GameInfo")]
    public static void CreateAsset () {
        CustomAssetUtility.CreateAsset<GameInfo>();
    }
}
