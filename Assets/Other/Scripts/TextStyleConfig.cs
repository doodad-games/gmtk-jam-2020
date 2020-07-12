// Stolen from Item TD

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TextStyleConfig", menuName = "Game/TextStyleConfig")]
public class TextStyleConfig : ScriptableObject {
#pragma warning disable CS0649
    [SerializeField] Font[] _fonts;
    [SerializeField] InspectorTextStyle[] _styles;
#pragma warning restore CS0649

    Dictionary<string, InspectorTextStyle> _styleDict;

    public InspectorTextStyle Get(string style) {
        PrepareStyles();

        if (!_styleDict.ContainsKey(style)) throw new Exception();
        return _styleDict[style];
    }

    void PrepareStyles() {
        if (_styleDict != null) return;

        var fontDict = new Dictionary<string, Font>();
        foreach (var font in _fonts) fontDict[font.key] = font;

        _styleDict = new Dictionary<string, InspectorTextStyle>();
        for (var i = 0; i != _styles.Length; ++i) {
            var style = _styles[i];

            if (!fontDict.ContainsKey(style.fontKey))
                throw new System.Exception();
            
            var font = fontDict[style.fontKey];
            style.fontAsset = font.asset;
            style.fontMaterial = font.material;
            
            _styleDict[style.key] = style;
        }
    }

    [Serializable]
    struct Font {
    #pragma warning disable CS0649
        public string key;
        public TMP_FontAsset asset;
        public Material material;
    #pragma warning restore CS0649
    }
}

[Serializable]
public struct InspectorTextStyle {
#pragma warning disable CS0649
    public string key;
    public string fontKey;
    public bool skipColour;
    public Color32 colour;
    public bool bold;
    public bool skipSize;
    public int size;
#pragma warning restore CS0649

    [HideInInspector] public TMP_FontAsset fontAsset;
    [HideInInspector] public Material fontMaterial;
}