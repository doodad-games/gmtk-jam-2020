// Stolen from Item TD

using TMPro;
using UnityEngine;

public class TextStyle : MonoBehaviour
{
    static TextStyleConfig _config;

	[RuntimeInitializeOnLoadMethod]
	static void ResetStatics() => Resources.UnloadAsset(_config);

#pragma warning disable CS0649
    [SerializeField] string _style;
#pragma warning restore CS0649

    void Awake()
    {
        if (_config == null)
            _config = Resources.Load<TextStyleConfig>("TextStyleConfig");

        var style = _config.Get(_style);

        var text = GetComponent<TextMeshPro>();
        if (text != null)
        {
            text.font = style.fontAsset;
            text.fontSharedMaterial = style.fontMaterial;
            if (!style.skipColour) text.color = style.colour;
            text.fontStyle = 0
                | (style.bold ? FontStyles.Bold : 0);

            if (!style.skipSize) text.fontSize = style.size;
        }
        else
        {
            var uiText = GetComponent<TextMeshProUGUI>();
            if (uiText == null) throw new System.Exception();

            uiText.font = style.fontAsset;
            uiText.fontSharedMaterial = style.fontMaterial;
            if (!style.skipColour) uiText.color = style.colour;
            uiText.fontStyle = 0
                | (style.bold ? FontStyles.Bold : 0);

            if (!style.skipSize) uiText.fontSize = style.size;
        }

        Destroy(this);
    }
}