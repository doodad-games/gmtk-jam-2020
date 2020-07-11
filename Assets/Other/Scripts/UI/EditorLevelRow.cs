using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorLevelRow : MonoBehaviour
{
    const float DELETE_CONFIRM_TIME = 2f;

    public LevelData level => _data;

    public TMP_InputField noteField;

#pragma warning disable CS0649
    [SerializeField] Image _thumb;
    [SerializeField] TextMeshProUGUI _levelIndicator;
    [SerializeField] TMP_InputField _reorderField;
    [SerializeField] TextMeshProUGUI _numSetPieces;
    [SerializeField] TextMeshProUGUI _numPuzzlePieces;
    [SerializeField] TextMeshProUGUI _deleteButtonText;
#pragma warning restore CS0649

    LevelData _data;

    bool _hasStarted;
    bool _isConfirmDelete;
    float _confirmDeleteUntil;

    void OnEnable()
    {
        if (!_hasStarted) return;
        noteField.onValueChanged.AddListener(Editor.ValuesChanged);
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        noteField.text = _data.internalNote;

        _hasStarted = true;
        OnEnable();
    }

    void Update()
    {
        if (_isConfirmDelete && Time.unscaledTime > _confirmDeleteUntil)
        {
            _isConfirmDelete = false;
            _deleteButtonText.text = "Delete";
        }
    }

    void OnDisable()
    {
        if (!_hasStarted) return;
        noteField.onValueChanged.RemoveListener(Editor.ValuesChanged);
    }

    public void Init(int i)
    {
        _data = Global.modData.levels[i];

        _thumb.sprite = GameConfig.scenes[_data.sceneKey].thumbnail;
        _numSetPieces.text = _data.setPieces.Count.ToString();
        _numPuzzlePieces.text = _data.availablePieces.Sum(_ => _.numAvailable).ToString();
        noteField.text = _data.internalNote;

        UpdateIndex(i);
    }

    public void UpdateIndex(int i) =>
        _levelIndicator.text = _reorderField.text = (i + 1).ToString();

    public void Reorder()
    {
        var target = Int32.Parse(_reorderField.text) - 1;
        var max = transform.parent.childCount - 2; // Don't count new level button
        transform.SetSiblingIndex(Mathf.Max(0, Mathf.Min(max, target)));
        Editor.Save();
    }

    public void Edit() => Navigation.GoToEditLevel(_data);

    public void Delete()
    {
        if (_isConfirmDelete)
        {
            Destroy(gameObject);
            Editor.Save();
        }
        else
        {
            _isConfirmDelete = true;
            _confirmDeleteUntil = Time.unscaledTime + DELETE_CONFIRM_TIME;
            _deleteButtonText.text = "Really delete?";
        }
    }
}