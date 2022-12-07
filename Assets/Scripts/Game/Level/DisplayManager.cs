using UnityEngine;
using TMPro;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI liveText;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    [SerializeField] private GameObject[] lives;
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private GameObject pauseWindow;

    [Header("SFX")]
    [SerializeField] private AudioClip buttonSound;

    public void SetLive(int count)
    {
        liveText.text = count.ToString();
        for (int i = 0; i < lives.Length; i++)
        {
            if (i > count - 1) lives[i].SetActive(false);
            else lives[i].SetActive(true);
        }
    }

    public void SetHighScore(int score)
    {
        highScore.text = score.ToString();
    }

    public void SetCurentScore(int score)
    {
        currentScore.text = score.ToString();
    }

    public void SetLevel(int level)
    {
        currentLevel.text = level.ToString();
    }

    public void GameOver(bool enable)
    {
        gameOverScore.text = "Your Score: " + currentScore.text;
        gameOverWindow.SetActive(enable);
    }

    public void Pause()
    {
        pauseWindow.SetActive(pauseWindow.activeSelf ? false : true);
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlayOnSFX(buttonSound);
    }
}
