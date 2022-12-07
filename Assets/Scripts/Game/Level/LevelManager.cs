using UnityEngine;

public class LevelManager : BaseManager
{
    [SerializeField] private OutBound outBound;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private Padding player;
    [SerializeField] private DisplayManager display;
    [SerializeField] private Controller controller;

    private int currentLevel = 1;
    private int currentScore = 0;
    private int highScore = 0;

    private int bricksMustDestroy = 0;

    private int liveCount = 3;

    private void Awake()
    {
        LoadData();
        InstallScene();
    }

    private void Start()
    {
        InitDisplay();
    }

    private void OnEnable()
    {
        foreach (var brick in levelGenerator.GetBricks())
        {
            brick.OnBallTouch += AddScore;
            brick.OnBrickDestroy += BrickDestroy;
        }
        controller.OnClickPause += Pause;
        outBound.OnBallOut += LostLive;
    }

    private void OnDisable()
    {
        foreach (var brick in levelGenerator.GetBricks())
        {
            brick.OnBallTouch -= AddScore;
            brick.OnBrickDestroy += BrickDestroy;
        }
        controller.OnClickPause -= display.Pause;
        outBound.OnBallOut -= LostLive;
    }

    private void OnApplicationQuit()
    {
        DataSerialize.Instance.SaveLevel(levelGenerator.GetCurentLevel());
        DataSerialize.Instance.SaveRecord(new Records { currentScore = currentScore, highScore = highScore, levelNumber = currentLevel, live = liveCount });
    }

    public override void LoadScene(string name)
    {
        if (liveCount > 0)
        {
            DataSerialize.Instance.SaveLevel(levelGenerator.GetCurentLevel());
            DataSerialize.Instance.SaveRecord(new Records { currentScore = currentScore, highScore = highScore, levelNumber = currentLevel, live = liveCount });
        }
        else
        {
            DataSerialize.Instance.SaveRecord(new Records { currentScore = 0, highScore = highScore, levelNumber = 1, live = 3 });
        }
        Time.timeScale = 1;
        base.LoadScene(name);
    }

    public void Pause()
    {
        display.Pause();
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    public void NewGame()
    {
        currentLevel = GameState.Instance.records.levelNumber;
        currentScore = GameState.Instance.records.currentScore;
        liveCount = GameState.Instance.records.live;
        controller.isEnable = true;
        InitDisplay();
        levelGenerator.StartLevel(currentLevel);
        bricksMustDestroy = levelGenerator.GetActiveBricks();
        player.RespawnBall();
        display.GameOver(false);
    }

    private void LoadData()
    {
        currentLevel = GameState.Instance.records.levelNumber;
        currentScore = GameState.Instance.records.currentScore;
        highScore = GameState.Instance.records.highScore;
        liveCount = GameState.Instance.records.live;
    }

    private void InstallScene()
    {
        levelGenerator.SpawnBricks();

        if (GameState.Instance.curentLevel != null)
            levelGenerator.StartLevel(GameState.Instance.curentLevel);
        else
            levelGenerator.StartLevel(currentLevel);

        bricksMustDestroy = levelGenerator.GetActiveBricks();
    }

    private void InitDisplay()
    {
        display.SetCurentScore(currentScore);
        display.SetHighScore(highScore);
        display.SetLive(liveCount);
        display.SetLevel(currentLevel);
    }

    private void AddScore(int score)
    {
        currentScore += score;
        display.SetCurentScore(currentScore);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            display.SetHighScore(highScore);
        }
    }

    private void LostLive()
    {
        liveCount--;
        if (liveCount == 0)
        {
            display.SetLive(liveCount);
            LevelEnd();
        }
        else
        {
            display.SetLive(liveCount);
            player.RespawnPadding();
            player.RespawnBall();
        }
    }

    private void BrickDestroy()
    {
        bricksMustDestroy--;
        if (bricksMustDestroy == 0) LevelEnd();
    }

    private void LevelEnd()
    {
        if (bricksMustDestroy == 0)
        {
            if (levelGenerator.GetCountLevels() == currentLevel)
            {
                GameOver();
            }
            else
            {
                player.RespawnBall();
                player.RespawnPadding();
                NextLevel();
            }
        }
        else GameOver();
    }

    private void NextLevel()
    {
        currentLevel++;
        display.SetLevel(currentLevel);
        levelGenerator.StartLevel(currentLevel);
    }

    private void GameOver()
    {
        GameState.Instance.NewGame();
        controller.isEnable = false;
        display.GameOver(true);
    }
}
