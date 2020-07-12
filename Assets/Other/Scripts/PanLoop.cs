using UnityEngine;

/*
Object to loop should have a plain transform (0 local position/rotation, 1 scale).
Make modifications to a parent transform instead of changing this.

Script will create a copy of the game object and move both along the X axis only -
which is why rotating this game object will ruin the illusion.
 */
public class PanLoop : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] float _xPerSec;
    [SerializeField] float _xPeriod;
#pragma warning restore CS0649

    GameObject _other;

    void Start()
    {
        _other = Instantiate(
            gameObject,
            transform.parent
        );
        _other.transform.localPosition = new Vector3(-_xPeriod, 0f, 0f);
        _other.transform.rotation = transform.rotation;

        Destroy(_other.GetComponent<PanLoop>());
    }

    void Update()
    {
        var curX = transform.localPosition.x;
        var newX = (curX + _xPerSec * Time.deltaTime) % _xPeriod;

        transform.localPosition = new Vector3(newX, 0f, 0f);
        _other.transform.localPosition = new Vector3(-_xPeriod + newX, 0f, 0f);
    }
}
