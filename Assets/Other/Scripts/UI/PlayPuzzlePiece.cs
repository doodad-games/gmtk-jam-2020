using TMPro;
using UnityEngine;

public class PlayPuzzlePiece : AvailablePieceButton
{
#pragma warning disable CS0649
    [SerializeField] TextMeshProUGUI _amountText;
    [SerializeField] GameObject _cover;
#pragma warning restore CS0649

    int _numAvailable;

    protected override bool CanPlace => _numAvailable != 0;

    public void Init(AvailablePieces ap)
    {
        piece = GameConfig.pieces[ap.pieceKey];
        _numAvailable = ap.numAvailable;

        Refresh();
    }

    protected override void HandlePlacement(Placement placement)
    {
        if (placement.pieceKey != piece.key) return;
        --_numAvailable;
        Refresh();
    }

    protected override void HandleUnplacement(Placement placement)
    {
        if (placement.pieceKey != piece.key) return;
        ++_numAvailable;
        Refresh();
    }

    void Refresh()
    {
        _amountText.text = _numAvailable.ToString();
        _cover.SetActive(_numAvailable == 0);

        if (_numAvailable == 0 && Player.selectedPiece == piece)
            Player.selectedPiece = null;
    }
}
