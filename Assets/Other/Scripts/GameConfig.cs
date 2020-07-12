using System.Collections.Generic;
using UnityEngine;

/**
mod.io test:
id = 535
api key = f137893bbc7139576efd5b9513fd835f

mod.io prod:
id = 685
api key = 72f3cd2e6bb6faf69c2f9034904d40f3
*/

[CreateAssetMenu(menuName = "Game/GameConfig", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public static IReadOnlyDictionary<string, SceneConfig> scenes => _i._sceneDict;
    public static IReadOnlyDictionary<string, PieceData> pieces => _i._piecesDict;
    public static ModData officialMod => _i._officialMod;

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
    ModData _officialMod;

    void SetUp()
    {
        _sceneDict = new Dictionary<string, SceneConfig>();
        foreach (var scene in _scenes)
            _sceneDict[scene.sceneName] = scene;
        
        _piecesDict = new Dictionary<string, PieceData>();
        foreach (var piece in _pieces)
            _piecesDict[piece.key] = piece;
        
        _officialMod = JsonUtility.FromJson<ModData>(
            Resources.Load<TextAsset>("official-mod").text
        );
    }
}
