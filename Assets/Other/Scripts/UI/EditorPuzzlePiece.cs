using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorPuzzlePiece : MonoBehaviour
{
// Set initial amount and icon (on init)
// Increment, save
// Decrement, save
#pragma warning disable CS0649
    [SerializeField] PieceData _piece;
    [SerializeField] Image _thumb;
    [SerializeField] TextMeshProUGUI _amountText;
#pragma warning restore CS0649

    int _amount;

    void Awake()
    {
        _thumb.sprite = _piece.thumbnail;

        _amount = Global.levelData.GetNumAvailablePieces(_piece.key);
        Refresh();
    }

    public void Increment()
    {
        ++_amount;
        Save();
    }

    public void Decrement()
    {
        if (_amount == 0) return;
        --_amount;
        Save();
    }

    void Save()
    {
        Global.levelData.SetNumAvailablePieces(_piece.key, _amount);
        Refresh();
    }

    void Refresh() => _amountText.text = _amount.ToString();
}
