using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public string sceneKey;
    public string internalNote;
    public List<Placement> setPieces = new List<Placement>();
    public List<AvailablePieces> availablePieces = new List<AvailablePieces>();
    public List<int> movementSequence = new List<int>();

    public LevelData(SceneConfig scene) => sceneKey = scene.sceneName;

    public void AddToModData()
    {
        Global.modData.levels.Add(this);
        Global.modData.Save();
    }

    public int GetNumAvailablePieces(string pieceKey) =>
        availablePieces.Find(_ => _.pieceKey == pieceKey).numAvailable;
    
    public void SetNumAvailablePieces(string pieceKey, int num)
    {
        var piece = availablePieces.Find(_ => _.pieceKey == pieceKey);
        if (piece == null)
        {
            piece = new AvailablePieces { pieceKey = pieceKey };
            availablePieces.Add(piece);
        }

        piece.numAvailable = num;

        Global.modData.Save();
    }
}

[Serializable]
public class AvailablePieces
{
    public string pieceKey;
    public int numAvailable;
}