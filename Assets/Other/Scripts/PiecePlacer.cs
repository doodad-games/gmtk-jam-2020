using UnityEngine;

public class PiecePlacer : MonoBehaviour
{
    static Color _none = new Color(0, 0, 0, 0);
    static Color _valid = new Color(0.4f, 1, 0, 0.15f);
    static Color _invalid = new Color(1, 0.3f, 0.3f, 0.3f);

#pragma warning disable CS0649
    [SerializeField] SpriteRenderer _validity;
    [SerializeField] SpriteRenderer _ghost;
#pragma warning restore CS0649

    Color _ghostStartingColour;

    bool _isPieceValid => _curPiece != null && !_curPiece.isFakeDeletePiece;
    PieceData _curPiece;

    void Awake() => _ghostStartingColour = _ghost.color;

    void OnEnable()
    {
        Player.onSelectedPieceChanged += Refresh;
        Refresh();

        PlayerInputController.onGameWorldClicked += HandleGameWorldClicked;
    }

    void Update()
    {
        if (_curPiece != null)
        {
            var mousePos = MainCamera.c.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(
                Mathf.Round(mousePos.x),
                Mathf.Round(mousePos.y),
                0f
            );
        }
    }

    void OnDisable()
    {
        Player.onSelectedPieceChanged -= Refresh;
        PlayerInputController.onGameWorldClicked -= HandleGameWorldClicked;
    }

    void Refresh()
    {
        _curPiece = Player.selectedPiece;

        if (_isPieceValid)
        {
            _ghost.color = _ghostStartingColour;
            _ghost.sprite = _curPiece.ghost;

            _validity.color = _isPieceValid ? _valid : _invalid;
        }
        else _ghost.color = _validity.color = _none;
    }

    void HandleGameWorldClicked()
    {
        if (!_isPieceValid) return;

        var x = (int)transform.position.x;
        var y = (int)transform.position.y;

        var placement = new Placement { pieceKey = _curPiece.key, x = x, y = y };

        Player.Place(placement);
    }
}
