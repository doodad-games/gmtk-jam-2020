using System.Collections;
using ModIO;
using TMPro;
using UnityEngine;

public class Editor : MonoBehaviour
{
    const float NAME_SAVE_DELAY = 0.3f;
    const float RESET_SAVE_TEXT_AFTER = 2f;
    const float UPLOAD_COMPLETE_DELAY = 4f;

    static ModData _upcoming;

    public static void Prepare(ModData data) => _upcoming = data;

#pragma warning disable CS0649
    [SerializeField] TMP_InputField _name;
    [SerializeField] TMP_InputField _summary;
    [SerializeField] TextMeshProUGUI _saveText;
    [SerializeField] TextMeshProUGUI _uploadText;
    [SerializeField] GameObject _loginDialog;
#pragma warning restore CS0649

    ModData _data;

    bool _modioUserExists;

    bool _hasStarted;
    bool _resetSaveText;
    float _resetSaveTextAfter;
    bool _resave;
    float _resaveAfter;
    bool _uploadInProgress;
    float _completeUploadAfter;

    void Awake() => _data = _upcoming;

    void OnEnable()
    {
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

        _name.text = _data.name;
        _summary.text = _data.summary;

        _hasStarted = true;
        OnEnable();
    }

    void Update()
    {
        if (_resave && Time.unscaledTime > _resaveAfter)
        {
            _resave = false;
            _data.name = _name.text;
            _data.summary = _summary.text;
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
        if (!_hasStarted) return;

        _name.onValueChanged.RemoveListener(ValuesChanged);
        _summary.onValueChanged.RemoveListener(ValuesChanged);
    }

    public void Save()
    {
        _data.Save();
        _saveText.text = "Saved!";
        _resetSaveText = true;
        _resetSaveTextAfter = Time.unscaledTime + RESET_SAVE_TEXT_AFTER;
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
        else _data.Upload(UploadSuccessful, UploadFailed);
    }

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

    void ValuesChanged(string _)
    {
        _resave = true;
        _resaveAfter = Time.unscaledTime + NAME_SAVE_DELAY;
    }
}