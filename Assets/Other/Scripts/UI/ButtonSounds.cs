using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData) =>
        SoundController.Play("uiClick");

    public void OnPointerEnter(PointerEventData eventData) =>
        SoundController.Play("uiMouseEnter");
}
