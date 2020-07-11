using UnityEngine;

public class Global : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Load() => DontDestroyOnLoad(
        Instantiate(Resources.Load<GameObject>("Global"))
    );

    public static ModData modData => _i._modData;
    public static LevelData levelData => _i._levelData;
    public static bool isEditMode => _i._isEditMode;

    static Global _i;

    public static void SetCurrentModData(ModData data) =>
        _i._modData = data;
    public static void SetCurrentLevelData(LevelData data, bool isEditMode)
    {
        _i._levelData = data;
        _i._isEditMode = isEditMode;
    }

    ModData _modData;
    LevelData _levelData;
    bool _isEditMode;
    
    void OnEnable() => _i = this;
    void OnDisable() => _i = null;
}
