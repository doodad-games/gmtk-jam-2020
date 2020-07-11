using UnityEngine;
using UnityEngine.UI;

public class EditorSceneChoice : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] Image _thumb;
#pragma warning restore CS0649

    SceneConfig _scene;

    public void Init(SceneConfig scene)
    {
        _scene = scene;
        _thumb.sprite = scene.thumbnail;
    }

    public void Pressed()
    {
        var level = new LevelData(_scene);
        level.AddToModData();

        Navigation.GoToEditLevel(level);
    }
}