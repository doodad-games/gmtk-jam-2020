using TMPro;
using UnityEngine;

public class TextLevelNum : MonoBehaviour
{
    void Awake() => GetComponent<TextMeshProUGUI>().text =
        (Global.modData.levels.IndexOf(Global.levelData) + 1).ToString();
}
