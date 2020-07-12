using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    public void PlaySound(string sound) => SoundController.Play(sound);
}
