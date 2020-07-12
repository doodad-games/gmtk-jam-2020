using System;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class Player : MonoBehaviour
{
    public static event Action onSelectedPieceChanged;

    public static PieceData selectedPiece
    {
        get => _i._selected;
        set
        {
            _i._selected = value;
            onSelectedPieceChanged?.Invoke();
        }
    }

    public static bool deleting => selectedPiece?.isFakeDeletePiece ?? false;

    static Player _i;

#pragma warning disable CS0649
    [SerializeField] GameObject[] _editorSpecific;
    [SerializeField] GameObject[] _playModeSpecific;
#pragma warning restore CS0649

    PieceData _selected;

    void OnEnable() => _i = this;

    void Start()
    {
        var toEnable = Global.isEditMode ? _editorSpecific : _playModeSpecific;
        foreach (var obj in toEnable) obj.SetActive(true);

        foreach (var placement in Global.levelData.setPieces)
            placement.Place(true);
    }

    void OnDisable() => _i = null;
}
