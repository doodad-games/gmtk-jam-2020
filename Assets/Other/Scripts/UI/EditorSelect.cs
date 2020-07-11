﻿using System.Linq;
using UnityEngine;

public class EditorSelect : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] GameObject _noLocalMods;
    [SerializeField] Transform _localContainer;
    [SerializeField] GameObject _noModIOMods;
    [SerializeField] Transform _modIOContainer;
#pragma warning restore CS0649

    void OnEnable()
    {
        AvailableMods.Refresh();
        UpdateModLists();

        AvailableMods.onUpdated += UpdateModLists;
    }

    void OnDisable() => AvailableMods.onUpdated -= UpdateModLists;

    void UpdateModLists()
    {
        _noLocalMods.SetActive(AvailableMods.localMods.Count == 0);
        foreach (var mod in AvailableMods.localMods)
            Instantiate(Resources.Load<GameObject>("EditorSelectExistingLocalMod"), _localContainer)
                .GetComponent<EditorSelectExistingMod>()
                .Init(mod);

        var modIOMods = AvailableMods.modIOMods.Where(_ => _.isFromCurrentUser).ToArray();
        _noModIOMods.SetActive(modIOMods.Length != 0);
        foreach (var mod in modIOMods)
            Instantiate(Resources.Load<GameObject>("EditorSelectExistingModIOMod"), _modIOContainer)
                .GetComponent<EditorSelectExistingMod>()
                .Init(mod);
    }
}