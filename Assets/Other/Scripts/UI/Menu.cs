using UnityEngine;

public class Menu : MonoBehaviour
{
    static Menu _i;

    public static void SelectMod(ModData mod)
    {
        if (_i == null) throw new System.Exception();
        Global.modData = mod;
    }

#pragma warning disable CS0649
    [SerializeField] GameObject _noLocalMods;
    [SerializeField] Transform _localContainer;
    [SerializeField] GameObject _noModIOMods;
    [SerializeField] Transform _modIOContainer;
    [SerializeField] Transform _levelContainer;
#pragma warning restore CS0649

    void OnEnable()
    {
        _i = this;
        AvailableMods.Refresh();
        UpdateModLists();
        UpdateLevelList();

        AvailableMods.onUpdated += UpdateModLists;
        AvailableMods.onUpdated += UpdateLevelList;
        Global.onModDataChanged += UpdateLevelList;
    }

    void OnDisable()
    {
        _i = null;
        AvailableMods.onUpdated -= UpdateModLists;
        AvailableMods.onUpdated -= UpdateLevelList;
        Global.onModDataChanged -= UpdateLevelList;
    }

    void UpdateModLists()
    {
        for (var i = _localContainer.childCount - 1; i != -1; --i)
            Destroy(_localContainer.GetChild(i).gameObject);
        _noLocalMods.SetActive(AvailableMods.localMods.Count == 0);
        foreach (var mod in AvailableMods.localMods)
            Instantiate(Resources.Load<GameObject>("MenuModButton"), _localContainer)
                .GetComponent<MenuModButton>()
                .Init(mod);

        for (var i = _modIOContainer.childCount - 1; i != -1; --i)
            Destroy(_modIOContainer.GetChild(i).gameObject);
        _noModIOMods.SetActive(AvailableMods.modIOMods.Count == 0);
        foreach (var mod in AvailableMods.modIOMods)
            Instantiate(Resources.Load<GameObject>("MenuModButton"), _modIOContainer)
                .GetComponent<MenuModButton>()
                .Init(mod);
    }

    void UpdateLevelList()
    {
        for (var i = _levelContainer.childCount - 1; i != -1; --i)
            Destroy(_levelContainer.GetChild(i).gameObject);

        if (Global.modData == null) return;

        for (var i = 0; i != Global.modData.levels.Count; ++i)
            Instantiate(Resources.Load<GameObject>("MenuLevelChoice"), _levelContainer)
                .GetComponent<MenuLevelChoice>()
                .Init(i);
    }
}
