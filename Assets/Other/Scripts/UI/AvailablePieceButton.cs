using UnityEngine;
using UnityEngine.UI;

public class AvailablePieceButton : MonoBehaviour
{
    public PieceData piece;

#pragma warning disable CS0649
    [SerializeField] Image _thumb;
    [SerializeField] GameObject _selectedHighlight;
#pragma warning restore CS0649

    bool _hasStarted;

    bool _isSelected;

    void OnEnable()
    {
        Player.onPlaced += HandlePlacement;
        Player.onUnplaced += HandleUnplacement;

        TryPostStart();
    }

    void OnDisable() 
    {
        Player.onPlaced -= HandlePlacement;
        Player.onUnplaced -= HandleUnplacement;

        if (!_hasStarted) return;
        Player.onSelectedPieceChanged -= HandleSelectedPieceChanged;
    }

    void Start()
    {
        _thumb.sprite = piece.thumbnail;

        _hasStarted = true;
        TryPostStart();
    }
    
    public void Pressed()
    {
        if (Player.selectedPiece == piece) Player.selectedPiece = null;
        else Player.selectedPiece = piece;
    }

    // For overriding...
    protected virtual void HandlePlacement(Placement placement) {}
    protected virtual void HandleUnplacement(Placement placement) {}

    void TryPostStart()
    {
        if (!_hasStarted) return;
        Player.onSelectedPieceChanged += HandleSelectedPieceChanged;
        Refresh();
    }

    void HandleSelectedPieceChanged()
    {
        var isSelected = Player.selectedPiece == piece;
        if (isSelected == _isSelected) return;
        _isSelected = isSelected;

        Refresh();
    }

    void Refresh() => _selectedHighlight.SetActive(_isSelected);
}
