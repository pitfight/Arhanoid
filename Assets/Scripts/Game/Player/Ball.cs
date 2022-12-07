using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private float _speed = 3f;
    private Transform positionOnPadding;
    private Vector2 velocity = Vector2.zero;

    public float speed
    {
        get => _speed;
        set
        {
            _speed = value;
            rb.velocity = rb.velocity.normalized * value;
        }
    }

    public bool isFly = false;
    public bool isSticky = false;

    [Header("SFX")]
    public AudioClip touchPading;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpawnTransform(Transform point)
    {
        positionOnPadding = point;
    }

    public void Push()
    {
        if (!isFly)
        {
            transform.SetParent(null);
            if (velocity == Vector2.zero)
                rb.velocity = Vector2.down * 1 * _speed;
            else
                rb.velocity = velocity;
            isFly = true;
        }
    }

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        velocity = Vector2.zero;
        speed = 3f;
        isFly = false;
        isSticky = false;
        transform.position = positionOnPadding.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Padding")
        {
            SoundManager.Instance.PlayOnSFX(touchPading);
            float x = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            if (isSticky)
            {
                rb.velocity = Vector2.zero;
                transform.SetParent(positionOnPadding.parent);
                isFly = false;
                velocity = dir * speed;
            }
            else
            {
                rb.velocity = dir * speed;
                speed += 0.05f;
            }
        }
        else
        {
            speed += 0.01f;
            rb.velocity += DirectionBounce(rb.velocity.normalized);
        }
    }

    private float HitFactor(Vector2 ballPos, Vector2 racketPos, float padding)
    {
        return (ballPos.x - racketPos.x) / padding;
    }

    private Vector2 DirectionBounce(Vector2 velocity)
    {
        float minVelocity = 0.2f;
        float velocityDelta = 0.5f;
        Vector2 direction = Vector2.zero;
        if (Mathf.Abs(velocity.x) < minVelocity) direction += new Vector2(velocity.x > 0 ? velocityDelta : -velocityDelta, 0);
        if (Mathf.Abs(velocity.y) < minVelocity) direction += new Vector2(0, velocity.y > 0 ? velocityDelta : -velocityDelta);
        return direction;
    }
}
