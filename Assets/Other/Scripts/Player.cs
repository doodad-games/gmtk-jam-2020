using UnityEngine;

public class Player : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] GameObject _editorUI;
    [SerializeField] GameObject _playModeUI;
#pragma warning restore CS0649

    void Awake()
    {
        _editorUI.SetActive(Global.isEditMode);
        _playModeUI.SetActive(!Global.isEditMode);
    }
}
