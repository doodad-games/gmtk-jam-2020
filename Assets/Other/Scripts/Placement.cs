using System;
using UnityEngine;

[Serializable]
public class Placement
{
    public string pieceKey;
    public int x;
    public int y;

    public void Delete()
    {
        if (Global.isEditMode) 
        {
            Global.levelData.setPieces.Remove(this);
            Global.modData.Save();
        }
    }
}