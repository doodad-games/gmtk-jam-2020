using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)]
public class Player : MonoBehaviour
{
    const float TIME_PER_MOVEMENT = 1f;
    const float SEQUENCER_UI_DIST_PER_SEC = 60f;

    public static event Action onSelectedPieceChanged;
    public static event Action onPreStartStopped;
    public static event Action onPostStartStopped;
    public static event Action onNewMovementStarted;
    public static event Action onVictoryChanged;

    public static PieceData selectedPiece
    {
        get => _i._selectedPiece;
        set
        {
            _i._selectedPiece = value;
            onSelectedPieceChanged?.Invoke();
        }
    }

    public static bool deleting => selectedPiece?.isFakeDeletePiece ?? false;
    public static bool playing => _i._playing;
    public static Movement curMovement => _i._curMovement;
    public static float passedTime => playing ? Time.time - _i._playTime : 0;
    public static float sequenceTotalTime => Global.levelData.movementSequence.Count * TIME_PER_MOVEMENT;
    public static bool outOfTime => passedTime > sequenceTotalTime;
    public static bool isGameOver => isLose || isVictory;
    public static bool isLose => _i._startPoints.Any(_ => _.died);
    public static bool isVictory => !isLose && _i._startPoints.All(_ => _.finished);

    static Player _i;

    static Dictionary<int, Movement> _movements = new Dictionary<int, Movement>
    {
        { 0, new Movement { dir = Vector2.right, buttonRotation = 0 } },
        { 1, new Movement { dir = new Vector2(1f, 1), buttonRotation = 45 } },
        { 2, new Movement { dir = new Vector2(-1f, 1), buttonRotation = 135 } },
        { 3, new Movement { dir = Vector2.left, buttonRotation = 180 } }
    };

    public static void Place(Placement placement) 
    {
        var piece = GameConfig.pieces[placement.pieceKey];

        GameObject.Instantiate(
            piece.objs.PickRandom(),
            new Vector3(placement.x, placement.y, 0f), Quaternion.identity
        )
            .GetComponent<Piece>()
            .Init(placement, true);
    }

    public static void RegisterStartPoint(StartPoint sp)
    {
        _i._startPoints.Add(sp);
        HandleVictoryChanges();
    }
    public static void DeregisterStartPoint(StartPoint sp)
    {
        if (_i == null) return;
        _i._startPoints.Remove(sp);
        HandleVictoryChanges();
    }

    static void HandleVictoryChanges()
    {
        if (!playing)
        {
            _i._winLosePopup.SetActive(false);
            return;
        }

        if (isGameOver)
        {
            _i._winLosePopup.SetActive(true);

            if (isVictory)
            {
                _i._winLoseText.text = "Victory!";
                SoundController.Play("win");
            }
            else
            {
                _i._winLoseText.text = "Lose :(";
                SoundController.Play("lose");
            }
        }
        else _i._winLosePopup.SetActive(false);

        onVictoryChanged?.Invoke();
    }

#pragma warning disable CS0649
    [SerializeField] GameObject[] _editorSpecific;
    [SerializeField] GameObject[] _playModeSpecific;
    [SerializeField] Transform[] _movementContainers;
    [SerializeField] ScrollRect _sequenceScrollView;
    [SerializeField] RectTransform[] _liveSequencersToOffset;
    [SerializeField] GameObject _editSequencer;
    [SerializeField] GameObject _testSequencer;
    [SerializeField] GameObject _editPuzzlePieces;
    [SerializeField] GameObject _playPuzzlePieces;
    [SerializeField] GameObject _winLosePopup;
    [SerializeField] TextMeshProUGUI _winLoseText;
    [SerializeField] GameObject _nextLevelButton;
#pragma warning restore CS0649

    PieceData _selectedPiece;
    int _justAddedToScrollView;
    bool _playing;
    float _playTime;
    int _curSequenceId;
    Movement _curMovement;

    HashSet<StartPoint> _startPoints = new HashSet<StartPoint>();

    void Awake() => Time.timeScale = 0;

    void OnEnable()
    {
        _i = this;

        StartPoint.onAStartPointFinished += HandleVictoryChanges;
    }

    void Start()
    {
        var toEnable = Global.isEditMode ? _editorSpecific : _playModeSpecific;
        foreach (var obj in toEnable) obj.SetActive(true);

        DoPlacements();

        foreach (var dir in Global.levelData.movementSequence)
            InstantiateMovement(dir);
        _justAddedToScrollView = 1;

        if (Global.modData.levels.IndexOf(Global.levelData) == Global.modData.levels.Count - 1)
            _nextLevelButton.SetActive(false);
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

        if (playing)
        {
            UpdateLiveSequencers();

            var newSequenceId = Mathf.FloorToInt(passedTime / TIME_PER_MOVEMENT);
            if (newSequenceId != _curSequenceId)
            {
                _curSequenceId = newSequenceId;
                _curMovement = _curSequenceId < Global.levelData.movementSequence.Count
                    ? _movements[Global.levelData.movementSequence[_curSequenceId]]
                    : null;
                
                if (_curMovement != null) onNewMovementStarted?.Invoke();
            }
        }
    }

    void OnDisable()
    {
        _i = null;

        StartPoint.onAStartPointFinished -= HandleVictoryChanges;
    }

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
        selectedPiece = null;

        var wasPlaying = _playing;
        _playing = !wasPlaying;

        onPreStartStopped?.Invoke();

        if (wasPlaying)
        {
            Time.timeScale = 0;
            _playing = false;

            if (Global.isEditMode)
            {
                _editSequencer.SetActive(true);
                _testSequencer.SetActive(false);

                _editPuzzlePieces.SetActive(true);
                _playPuzzlePieces.SetActive(false);
            }

            DoPlacements();
            UpdateLiveSequencers();
        }
        else
        {
            Time.timeScale = 1;

            _playing = true;
            _playTime = Time.time;
            _curSequenceId = -1;

            if (Global.isEditMode)
            {
                _editSequencer.SetActive(false);
                _testSequencer.SetActive(true);

                _editPuzzlePieces.SetActive(false);
                _playPuzzlePieces.SetActive(true);
            }
        }

        onPostStartStopped?.Invoke();
    }

    public void PlayNextLevel() => Navigation.GoToPlayLevel(
        Global.modData.levels[Global.modData.levels.IndexOf(Global.levelData) + 1]
    );

    void DoPlacements()
    {
        foreach (var placement in Global.levelData.setPieces)
            Place(placement);
    }

    void InstantiateMovement(int dir)
    {
        var movement = _movements[dir];

        foreach (var container in _movementContainers)
            Instantiate(Resources.Load<GameObject>("MovementIndicator"), container)
                .GetComponent<MovementIndicator>().Init(movement);
    }

    void UpdateLiveSequencers()
    {
        var seqOffset = -Mathf.Min(sequenceTotalTime, passedTime) * SEQUENCER_UI_DIST_PER_SEC;
        foreach (var tfm in _liveSequencersToOffset)
            tfm.SetInsetAndSizeFromParentEdge(
                RectTransform.Edge.Left,
                seqOffset,
                tfm.sizeDelta.x
            );
    }
}

[Serializable]
public class Movement
{
    public Vector2 dir;
    public float buttonRotation;
}