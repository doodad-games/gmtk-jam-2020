using UnityEngine;

public class StartPoint : MonoBehaviour
{
    void Start()
    {
        Instantiate(Resources.Load<GameObject>("Character"), transform);
    }
}