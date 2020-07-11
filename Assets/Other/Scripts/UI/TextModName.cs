using TMPro;
using UnityEngine;

public class TextModName : MonoBehaviour
{
    TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    void OnEnable() => Global.onModDataChanged += UpdateText;
    void OnDisable() => Global.onModDataChanged -= UpdateText;

    void UpdateText()
    {
        if (Global.modData == null) return;
        _text.text = Global.modData.name;
    }
}
