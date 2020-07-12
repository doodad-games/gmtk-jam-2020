using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfig", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public static IReadOnlyDictionary<string, SceneConfig> scenes => _i._sceneDict;
    public static IReadOnlyDictionary<string, PieceData> pieces => _i._piecesDict;

    static GameConfig __i;
    static GameConfig _i
    {
        get
        {
            if (__i == null)
            {
                __i = Resources.Load<GameConfig>("GameConfig");
                __i.SetUp();
            }

            return __i;
        }
    }

#pragma warning disable CS0649
    [SerializeField] SceneConfig[] _scenes;
    [SerializeField] PieceData[] _pieces;
#pragma warning restore CS0649

    Dictionary<string, SceneConfig> _sceneDict;
    Dictionary<string, PieceData> _piecesDict;

    void SetUp()
    {
        _sceneDict = new Dictionary<string, SceneConfig>();
        foreach (var scene in _scenes)
            _sceneDict[scene.sceneName] = scene;
        
        _piecesDict = new Dictionary<string, PieceData>();
        foreach (var piece in _pieces)
            _piecesDict[piece.key] = piece;
    }
}
