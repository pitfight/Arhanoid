using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum Grade
    {
        None = 0,
        Grede1 = 1,
        Grede2 = 2,
        Grede3 = 3,
        Grede4 = 4,
        Grede5 = 5
    }

    private int[] scoresGrade = { 100, 90, 80, 70, 60 };
    public bool isActive { get; private set; }
    public bool hasPowerUp = false;
    [SerializeField] private GameObject powerBrick;
    [SerializeField] private ParticleSystem destroy;

    private Grade _grade;
    [SerializeField] private SpriteRenderer rendererBrick;
    [SerializeField] private Sprite[] spritesGrades;

    public Action<int> OnBallTouch;
    public Action OnBrickDestroy;

    [Header("SFX")]
    [SerializeField] private AudioClip brickTouch;
    [SerializeField] private AudioClip brickDestroy;

    private void Awake()
    {
        rendererBrick = GetComponent<SpriteRenderer>();
        //gameObject.SetActive(false);
    }

    public Grade grade
    {
        get => _grade;
        set
        {
            _grade = value;
            if ((int)_grade == 0)
                rendererBrick.sprite = null;
            else
                rendererBrick.sprite = spritesGrades[(int)_grade - 1];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Touch();
    }

    public int GetGrade()
    {
        if (isActive) return (int)grade;
        else return 0;
    }

    public void SetGrade(int grade)
    {
        if (grade == 0)
        {
            isActive = false;
            gameObject.SetActive(false);
        }
        else
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            isActive = true;
            this.grade = (Grade)grade;
        }
    }

    public void Touch()
    {
        if (grade == (Grade)1)
        {
            SoundManager.Instance.PlayOnSFX(brickDestroy);
            gameObject.SetActive(false);
            OnBallTouch?.Invoke(scoresGrade[(int)grade - 1]);
            OnBrickDestroy?.Invoke();
            if (hasPowerUp) Instantiate(powerBrick, transform.position, Quaternion.identity);
            destroy.Play();
            grade--;
        }
        else
        {
            SoundManager.Instance.PlayOnSFX(brickTouch);
            OnBallTouch?.Invoke(scoresGrade[(int)grade - 1]);
            grade--;
        }
    }
}
