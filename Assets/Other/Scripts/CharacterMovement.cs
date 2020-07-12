using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] float _speed;
    [SerializeField] float _jumpStrength;
#pragma warning restore CS0649

    Character _char;
    Rigidbody2D _rb;

    void Awake()
    {
        _char = GetComponent<Character>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable() => Player.onNewMovementStarted += HandleNewMovement;

    void FixedUpdate()
    {
        var mv = Player.curMovement;
        if (mv != null)
        {
            var x = mv.dir.x * _speed;
            _rb.AddForce(new Vector2(x, 0));
        }

        _char.anim.SetFloat("VelocityX", _rb.velocity.x);
        _char.anim.SetFloat("VelocityY", _rb.velocity.y);

        transform.localScale = new Vector3(Mathf.Sign(_rb.velocity.x), 1f, 1f);
    }

    void OnDisable() => Player.onNewMovementStarted -= HandleNewMovement;

    void HandleNewMovement()
    {
        var mv = Player.curMovement;
        if (mv.dir.y != 0)
        {
            _rb.AddForce(new Vector2(0, mv.dir.y * _jumpStrength));
        
            _char.anim.SetTrigger("Jump");
        }
    }
}
