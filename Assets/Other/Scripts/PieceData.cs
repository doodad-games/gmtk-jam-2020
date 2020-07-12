using UnityEngine;

[CreateAssetMenu(menuName = "Game/PieceData", fileName = "PieceData")]
public class PieceData : ScriptableObject
{
    public bool isFakeDeletePiece => key == "FakeDeletePiece";

    public string key => name;
    public GameObject[] objs;
    public bool isSetPiece;
    public Sprite thumbnail;
    public Sprite ghost;

    public void PlaceAt(int x, int y)
    {
        var placement = new Placement { pieceKey = key, x = x, y = y };

        if (Global.isEditMode) Global.levelData.setPieces.Add(placement);
        Global.modData.Save();

        placement.Place(Global.isEditMode);
    }
}
