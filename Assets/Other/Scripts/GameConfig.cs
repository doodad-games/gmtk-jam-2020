using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfig", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public static IReadOnlyDictionary<string, SceneConfig> scenes => _i._sceneDict;

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
#pragma warning restore CS0649

    Dictionary<string, SceneConfig> _sceneDict;

    void SetUp()
    {
        _sceneDict = new Dictionary<string, SceneConfig>();
        foreach (var scene in _scenes)
            _sceneDict[scene.sceneName] = scene;
    }
}
