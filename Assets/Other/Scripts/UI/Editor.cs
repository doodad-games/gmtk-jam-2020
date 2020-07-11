using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class Editor : MonoBehaviour
{
    const float NAME_SAVE_DELAY = 0.3f;
    const float RESET_SAVE_TEXT_AFTER = 2f;
    const float UPLOAD_COMPLETE_DELAY = 4f;

    static Editor _i;

    public static void Save()
    {
        if (_i == null) throw new System.Exception();

        Global.modData.name = _i._name.text;
        Global.modData.summary = _i._summary.text;

        var levelI = 0;
        Global.modData.levels = _i._levelContainer
            .GetComponentsInChildren<EditorLevelRow>()
            .Where(_ => _?.isActiveAndEnabled ?? false)
            .Select(_ => {
                _.level.internalNote = _.noteField.text;

                _.UpdateIndex(levelI);
                ++levelI;

                return _.level;
            })
            .ToList();

        Global.modData.Save();
        _i._saveText.text = "Saved!";
        _i._resetSaveText = true;
        _i._resetSaveTextAfter = Time.unscaledTime + RESET_SAVE_TEXT_AFTER;
    }

    public static void ValuesChanged(string _)
    {
        _i._resave = true;
        _i._resaveAfter = Time.unscaledTime + NAME_SAVE_DELAY;
    }

#pragma warning disable CS0649
    [SerializeField] TMP_InputField _name;
    [SerializeField] TMP_InputField _summary;
    [SerializeField] TextMeshProUGUI _saveText;
    [SerializeField] TextMeshProUGUI _uploadText;
    [SerializeField] Transform _levelContainer;
    [SerializeField] GameObject _sceneSelectPopup;
    [SerializeField] Transform _sceneContainer;
#pragma warning restore CS0649

    bool _modioUserExists;

    bool _hasStarted;
    bool _resetSaveText;
    float _resetSaveTextAfter;
    bool _resave;
    float _resaveAfter;
    bool _uploadInProgress;
    float _completeUploadAfter;

    void Awake()
    {
        for (var i = 0; i != Global.modData.levels.Count; ++i)
        {
            var obj = Instantiate(Resources.Load<GameObject>("EditorLevelRow"), _levelContainer);
            obj.GetComponent<EditorLevelRow>().Init(i);
            obj.transform.SetSiblingIndex(obj.transform.GetSiblingIndex() - 1);
        }

        foreach (var scene in GameConfig.scenes)
            Instantiate(Resources.Load<GameObject>("EditorSceneChoice"), _sceneContainer)
                .GetComponent<EditorSceneChoice>()
                .Init(scene.Value);
    }

    void OnEnable()
    {
        _i = this;

        if (!_hasStarted) return;

        _name.onValueChanged.AddListener(ValuesChanged);
        _summary.onValueChanged.AddListener(ValuesChanged);

        _modioUserExists = ModIOController.userExists;
        UpdateUploadText();
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _name.text = Global.modData.name;
        _summary.text = Global.modData.summary;

        _hasStarted = true;
        OnEnable();
    }

    void Update()
    {
        if (_resave && Time.unscaledTime > _resaveAfter)
        {
            _resave = false;
            Save();
        }

        if (_resetSaveText && Time.unscaledTime > _resetSaveTextAfter)
        {
            _resetSaveText = false;
            _saveText.text = "Save";
        }

        if (_uploadInProgress && Time.unscaledTime > _completeUploadAfter)
        {
            _uploadInProgress = false;
            UpdateUploadText();
        }

        if (!_modioUserExists)
        {
            _modioUserExists = ModIOController.userExists;
            if (_modioUserExists) UpdateUploadText();
        }
    }

    void OnDisable()
    {
        _i = null;

        if (!_hasStarted) return;

        _name.onValueChanged.RemoveListener(ValuesChanged);
        _summary.onValueChanged.RemoveListener(ValuesChanged);
    }

    public void Upload()
    {
        if (_uploadInProgress) return;
        _uploadInProgress = true;
        _completeUploadAfter = float.PositiveInfinity;

        _uploadText.text = "... Uploading ...";

        Save();

        if (!_modioUserExists)
        {
            ModIOController.ShowLogin();
            UploadComplete();
        }
        else Global.modData.Upload(UploadSuccessful, UploadFailed);
    }

    public void ChooseNewLevelScene() => _sceneSelectPopup.SetActive(true);

    void UploadSuccessful()
    {
        _uploadText.text = "Share successful!";
        UploadComplete();
    }

    void UploadFailed()
    {
        _uploadText.text = "Share failed :(";
        UploadComplete();
    } 

    void UploadComplete() =>
        _completeUploadAfter = Time.unscaledTime + UPLOAD_COMPLETE_DELAY;

    void UpdateUploadText()
    {
        if (_uploadInProgress) return;
        _uploadText.text = _modioUserExists
            ? "Share via mod.io"
            : "Login to mod.io to share";
    }
}