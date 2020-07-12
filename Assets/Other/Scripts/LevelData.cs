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
}

[Serializable]
public struct AvailablePieces
{
    public string pieceKey;
    public int numAvailable;
}