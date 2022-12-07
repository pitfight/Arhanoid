using UnityEngine;
using TMPro;

public class PowerBrick : MonoBehaviour
{
    private enum PowerType
    {
        StickBall = 0,
        LongPad = 1,
        AddSpeedBall = 2
    }

    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector2.down;
    private float speed = 1f;

    private PowerType type;

    private void OnEnable()
    {
        type = (PowerType)Random.Range(0, 3);
        switch (type)
        {
            case PowerType.StickBall:
                textMesh.text = "SB";
                spriteRenderer.color = Color.blue;
                break;
            case PowerType.LongPad:
                textMesh.text = "LP";
                spriteRenderer.color = Color.yellow;
                break;
            case PowerType.AddSpeedBall:
                textMesh.text = "SU";
                spriteRenderer.color = Color.cyan;
                break;
        }
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent?.name == "Padding")
        {
            Padding padding = collision.transform.parent.GetComponent<Padding>();
            switch (type)
            {
                case PowerType.StickBall:
                    padding.StickyBall();
                    break;
                case PowerType.LongPad:
                    padding.StateBigger();
                    break;
                case PowerType.AddSpeedBall:
                    padding.UpSpeed();
                    break;
            }
            Destroy(gameObject);
        }      
    }
}
