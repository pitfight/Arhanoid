using UnityEngine;

public class Padding : MonoBehaviour
{
    public enum State
    {
        Normal,
        Bigger
    }

    private Ball ball;
    [SerializeField] private Controller controller;

    [Header("Ball References.")]
    [SerializeField] private Transform pointSpawnBall;
    [SerializeField] private GameObject ballPrefab;

    [Header("Padding References.")]
    [SerializeField] private Vector2 startPosition = new Vector2(0f, -4f);
    [SerializeField] private float paddingSpeed = 4f;
    [SerializeField] private float minPosX = -3.3f;
    [SerializeField] private float maxPosX = 3.3f;

    [SerializeField] private GameObject normalPad;
    [SerializeField] private GameObject biggerPad;

    [Header("SFX")]
    [SerializeField] private AudioClip powerUp;

    private void Awake()
    {
        SpawnBall(new Vector2(pointSpawnBall.position.x, pointSpawnBall.position.y));
    }

    private void OnEnable()
    {
        this.controller.OnBallPush += ball.Push;
        this.controller.OnMovement += Movement;
    }

    private void OnDisable()
    {
        controller.OnBallPush -= ball.Push;
        controller.OnMovement -= Movement;
    }

    private void SpawnBall(Vector2 point)
    {
        ball = Instantiate(ballPrefab, point, Quaternion.identity).GetComponent<Ball>();
        ball.SetSpawnTransform(pointSpawnBall);
        ball.transform.SetParent(transform);
    }

    public void RespawnBall()
    {
        ball.Respawn();
        ball.transform.SetParent(transform);
    }

    public void RespawnPadding()
    {
        ChangeState(State.Normal);
        transform.position = startPosition;
    }

    private void Movement(float axis)
    {
        float translation = Input.GetAxis("Horizontal") * paddingSpeed * Time.deltaTime;
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minPosX, maxPosX) + translation, transform.position.y);
    }

    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Normal:
                biggerPad.SetActive(false);
                normalPad.SetActive(true);

                minPosX = -3.3f;
                maxPosX = 3.3f;
                break;
            case State.Bigger:
                biggerPad.SetActive(true);
                normalPad.SetActive(false);

                minPosX = -3.2f;
                maxPosX = 3.2f;
                break;
        }
    }

    [ContextMenu("UpSpeed")]
    public void UpSpeed()
    {
        SoundManager.Instance.PlayOnSFX(powerUp);
        ball.speed += 1.5f;
    }

    [ContextMenu("Sticky")]
    public void StickyBall()
    {
        SoundManager.Instance.PlayOnSFX(powerUp);
        ball.isSticky = true;
    }

    [ContextMenu("Bigger")]
    public void StateBigger()
    {
        SoundManager.Instance.PlayOnSFX(powerUp);
        ChangeState(State.Bigger);
    }
}
