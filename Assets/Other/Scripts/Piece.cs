using UnityEngine;

public class Piece : MonoBehaviour
{
    bool _canBeDeleted => Player.deleting &&
        _isSetPiece == Global.isEditMode;

#pragma warning disable CS0649
    [SerializeField] SpriteRenderer _ghost;
#pragma warning restore CS0649

    Placement _placement;
    PieceData _piece;
    bool _isSetPiece;

    void OnEnable() => Player.onSelectedPieceChanged += Refresh;

    void OnDisable() => Player.onSelectedPieceChanged -= Refresh;

    public void Init(Placement placement, bool isSetPiece)
    {
        _placement = placement;
        _piece = GameConfig.pieces[placement.pieceKey];
        _isSetPiece = isSetPiece;

        _ghost.sprite = _piece.ghost;
    }

    public void Clicked()
    {
        if (_canBeDeleted)
        {
            Destroy(gameObject);
            _placement.Delete();
        }
    }

    void Refresh() =>
        _ghost.gameObject.SetActive(_canBeDeleted);
}