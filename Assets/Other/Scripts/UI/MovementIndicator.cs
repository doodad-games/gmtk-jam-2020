using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    public void Init(Movement movement) =>
        transform.Rotate(new Vector3(0, 0, 1), movement.buttonRotation);
}