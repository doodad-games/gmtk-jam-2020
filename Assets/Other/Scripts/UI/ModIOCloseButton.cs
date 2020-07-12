using UnityEngine;

public class ModIOCloseButton : MonoBehaviour
{
    public void Pressed() => ModIOController.HideMain();

    void Start() => transform.SetSiblingIndex(0);
}
