using UnityEngine;

public class MainCamera : MonoBehaviour
{
    static MainCamera _i;

    public static Camera c => _i._c;

    Camera _c;

    void Awake() => _c = GetComponent<Camera>();

    void OnEnable() => _i = this;
    void OnDisable() => _i = null;
}