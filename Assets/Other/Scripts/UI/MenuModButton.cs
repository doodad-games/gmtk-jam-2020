using TMPro;
using UnityEngine;

public class MenuModButton : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] TextMeshProUGUI _name;
#pragma warning restore CS0649

    ModData _data;

    public void Init(ModData data)
    {
        _data = data;
        _name.text = data.name;
    }

    public void Pressed() => Menu.SelectMod(_data);
}