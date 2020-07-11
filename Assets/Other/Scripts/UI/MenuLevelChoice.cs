using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelChoice : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] Image _thumb;
    [SerializeField] TextMeshProUGUI _levelIndicator;
#pragma warning restore CS0649

    LevelData _level;

    public void Init(int i)
    {
        _level = Global.modData.levels[i];

        _thumb.sprite = GameConfig.scenes[_level.sceneKey].thumbnail;
        _levelIndicator.text = (i + 1).ToString();
    }

    public void Pressed() => Navigation.GoToPlayLevel(_level);
}
