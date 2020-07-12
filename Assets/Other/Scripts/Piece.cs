using UnityEngine;

public class Piece : MonoBehaviour
{
    bool _canBeDeleted => Player.deleting && _isSetPiece != Player.isPlayMode;

#pragma warning disable CS0649
    [SerializeField] SpriteRenderer _ghost;
#pragma warning restore CS0649

    Placement _placement;
    PieceData _piece;
    bool _isSetPiece;

    void OnEnable()
    {
        Player.onSelectedPieceChanged += Refresh;
        Player.onShouldClear += HandleClear;
    }

    void OnDisable()
    {
        Player.onSelectedPieceChanged -= Refresh;
        Player.onShouldClear -= HandleClear;
    }

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
            Player.UnregisterPlaced(_placement);
        }
    }

    void Refresh() =>
        _ghost.gameObject.SetActive(_canBeDeleted);
    
    void HandleClear()
    {
        if (!Player.rolling) Destroy(gameObject);
    }
}