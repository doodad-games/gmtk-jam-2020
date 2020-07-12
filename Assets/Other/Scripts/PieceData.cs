using UnityEngine;

[CreateAssetMenu(menuName = "Game/PieceData", fileName = "PieceData")]
public class PieceData : ScriptableObject
{
    public bool isFakeDeletePiece => key == "FakeDeletePiece";

    public string key => name;
    public GameObject[] objs;
    public Sprite thumbnail;
    public Sprite ghost;
}
