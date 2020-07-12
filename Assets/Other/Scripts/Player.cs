using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    static Dictionary<int, Movement> _movements = new Dictionary<int, Movement>
    {
        { 0, new Movement { dir = Vector2.right, buttonRotation = 0 } },
        { 1, new Movement { dir = new Vector2(1, 1).normalized, buttonRotation = 45 } },
        { 2, new Movement { dir = new Vector2(-1, 1).normalized, buttonRotation = 135 } },
        { 3, new Movement { dir = Vector2.left, buttonRotation = 180 } }
    };

#pragma warning disable CS0649
    [SerializeField] GameObject[] _editorSpecific;
    [SerializeField] GameObject[] _playModeSpecific;
    [SerializeField] Transform _editorSequence;
    [SerializeField] ScrollRect _sequenceScrollView;
#pragma warning restore CS0649

    PieceData _selected;
    int _justAddedToScrollView;

    void OnEnable() => _i = this;

    void Start()
    {
        var toEnable = Global.isEditMode ? _editorSpecific : _playModeSpecific;
        foreach (var obj in toEnable) obj.SetActive(true);

        foreach (var placement in Global.levelData.setPieces)
            placement.Place(true);

        foreach (var dir in Global.levelData.movementSequence)
            InstantiateMovement(dir);
        _justAddedToScrollView = 1;
    }

    void Update()
    {
        if (_justAddedToScrollView != 0)
        {
            if (_justAddedToScrollView == 2)
            {
                _justAddedToScrollView = 0;
                _sequenceScrollView.horizontalNormalizedPosition = 1;
            } else ++_justAddedToScrollView;
        }
    }

    void OnDisable() => _i = null;

    public void AddMovement(int dir)
    {
        InstantiateMovement(dir);
        _justAddedToScrollView = 1;

        Global.levelData.movementSequence.Add(dir);
        Global.modData.Save();
    }

    public void RemoveLastMovement()
    {
        var sequence = Global.levelData.movementSequence;
        if (sequence.Count == 0) return;

        Destroy(_editorSequence.GetChild(_editorSequence.childCount - 1).gameObject);

        sequence.RemoveAt(sequence.Count - 1);
        Global.modData.Save();
    }

    void InstantiateMovement(int dir)
    {
        var movement = _movements[dir];

        Instantiate(Resources.Load<GameObject>("MovementIndicator"), _editorSequence)
            .GetComponent<MovementIndicator>().Init(movement);
    }
}

[Serializable]
public struct Movement
{
    public Vector2 dir;
    public float buttonRotation;
}