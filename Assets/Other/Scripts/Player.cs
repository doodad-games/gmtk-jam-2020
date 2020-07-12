using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)]
public class Player : MonoBehaviour
{
    public static event Action onSelectedPieceChanged;
    public static event Action onStartStopped;

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
    public static bool playing => Time.timeScale == 1;

    static Player _i;

    static Dictionary<int, Movement> _movements = new Dictionary<int, Movement>
    {
        { 0, new Movement { dir = Vector2.right, buttonRotation = 0 } },
        { 1, new Movement { dir = new Vector2(1, 1), buttonRotation = 45 } },
        { 2, new Movement { dir = new Vector2(-1, 1), buttonRotation = 135 } },
        { 3, new Movement { dir = Vector2.left, buttonRotation = 180 } }
    };

#pragma warning disable CS0649
    [SerializeField] GameObject[] _editorSpecific;
    [SerializeField] GameObject[] _playModeSpecific;
    [SerializeField] Transform[] _movementContainers;
    [SerializeField] ScrollRect _sequenceScrollView;
    [SerializeField] GameObject _editSequencer;
    [SerializeField] GameObject _testSequencer;
#pragma warning restore CS0649

    PieceData _selected;
    int _justAddedToScrollView;

    void Awake() => Time.timeScale = 0;

    void OnEnable() => _i = this;

    void Start()
    {
        var toEnable = Global.isEditMode ? _editorSpecific : _playModeSpecific;
        foreach (var obj in toEnable) obj.SetActive(true);

        DoPlacements();

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

        foreach (var container in _movementContainers)
            Destroy(container.GetChild(container.childCount - 1).gameObject);

        sequence.RemoveAt(sequence.Count - 1);
        Global.modData.Save();
    }

    public void ToggleStartStop() 
    {
        var wasPlaying = playing;
        Time.timeScale = playing ? 0 : 1;

        onStartStopped?.Invoke();

        if (wasPlaying)
        {
            Time.timeScale = 0;

            if (Global.isEditMode)
            {
                _editSequencer.SetActive(true);
                _testSequencer.SetActive(false);
            }

            DoPlacements();
        }
        else
        {
            Time.timeScale = 1;

            if (Global.isEditMode)
            {
                _editSequencer.SetActive(false);
                _testSequencer.SetActive(true);
            }
        }
    }

    void DoPlacements()
    {
        foreach (var placement in Global.levelData.setPieces)
            placement.Place(true);
    }

    void InstantiateMovement(int dir)
    {
        var movement = _movements[dir];

        foreach (var container in _movementContainers)
            Instantiate(Resources.Load<GameObject>("MovementIndicator"), container)
                .GetComponent<MovementIndicator>().Init(movement);
    }
}

[Serializable]
public struct Movement
{
    public Vector2 dir;
    public float buttonRotation;
}