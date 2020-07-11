using UnityEngine;

public class Global : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Load() => DontDestroyOnLoad(
        Instantiate(Resources.Load<GameObject>("Global"))
    );
}
