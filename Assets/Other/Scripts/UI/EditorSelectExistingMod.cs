using TMPro;
using UnityEngine;

public class EditorSelectExistingMod : MonoBehaviour
{
    const float DELETE_CONFIRM_TIME = 2f;

#pragma warning disable CS0649
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] GameObject _delete;
    [SerializeField] TextMeshProUGUI _deleteButtonText;
#pragma warning restore CS0649

    ModData _data;

    bool _isConfirmDelete;
    float _confirmDeleteUntil;

    void Update()
    {
        if (_isConfirmDelete && Time.unscaledTime > _confirmDeleteUntil)
        {
            _isConfirmDelete = false;
            _deleteButtonText.text = "Delete";
        }
    }

    public void Init(ModData data)
    {
        _data = data;
        _name.text = data.name;

        if (data.source == ModData.Source.ModIO) Destroy(_delete);
    }

    public void Edit() => Navigation.GoToEditor(_data);

    public void Delete()
    {
        if (_isConfirmDelete)
        {
            _data.DeleteLocal();
            Destroy(gameObject);
        }
        else
        {
            _isConfirmDelete = true;
            _confirmDeleteUntil = Time.unscaledTime + DELETE_CONFIRM_TIME;
            _deleteButtonText.text = "Really delete?";
        }
    }
}