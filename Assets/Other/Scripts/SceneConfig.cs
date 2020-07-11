using UnityEngine;

[CreateAssetMenu(menuName = "Game/SceneConfig", fileName = "SceneConfig")]
public class SceneConfig : ScriptableObject
{
    public string sceneName => name;
    public Sprite thumbnail;
}

