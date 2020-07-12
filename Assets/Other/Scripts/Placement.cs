using System;
using UnityEngine;

[Serializable]
public class Placement
{
    public string pieceKey;
    public int x;
    public int y;

    public void Place(bool isSetPiece) 
    {
        var piece = GameConfig.pieces[pieceKey];

        GameObject.Instantiate(
            piece.objs.PickRandom(),
            new Vector3(x, y, 0f), Quaternion.identity
        )
            .GetComponent<Piece>()
            .Init(this, isSetPiece);
    }

    public void Delete()
    {
        if (Global.isEditMode) 
        {
            Global.levelData.setPieces.Remove(this);
            Global.modData.Save();
        }
    }
}