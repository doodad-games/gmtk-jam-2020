using TMPro;
using UnityEngine;

public class StartStopButtonText : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Awake() => _text = GetComponent<TextMeshProUGUI>();

    void OnEnable()
    {
        Player.onStartStopped += Refresh;
        Refresh();
    }
    void OnDisable() => Player.onStartStopped += Refresh;

    void Refresh() => _text.text = Player.rolling ? "Stop" : "Play";
}
