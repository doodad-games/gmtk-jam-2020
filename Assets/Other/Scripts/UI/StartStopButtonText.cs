using TMPro;
using UnityEngine;

public class StartStopButtonText : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Awake() => _text = GetComponent<TextMeshProUGUI>();

    void OnEnable()
    {
        Player.onPreStartStopped += Refresh;
        Refresh();
    }
    void OnDisable() => Player.onPreStartStopped += Refresh;

    void Refresh() => _text.text = Player.playing ? "Stop" : "Play";
}
