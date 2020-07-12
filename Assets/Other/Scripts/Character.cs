using UnityEngine;

public class Character : MonoBehaviour
{
    const float Y_CUTOFF = -12f;

    public Animator anim => _anim;

#pragma warning disable CS0649
    [SerializeField] Animator _anim;
#pragma warning restore CS0649

    StartPoint _sp;

    bool _finished;

    void Awake() => _sp = GetComponentInParent<StartPoint>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (_finished || collider.GetComponentInParent<FinishPoint>() == null) return;
        _finished = true;

        GetComponent<CharacterMovement>().enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.simulated = false;

        _sp.SetFinished();

        _anim.SetBool("Finished", true);
    }

    void Update()
    {
        if (transform.position.y < Y_CUTOFF) Die();
    }

    void Die()
    {
        _sp.SetDied();
        Destroy(gameObject);
    }
}