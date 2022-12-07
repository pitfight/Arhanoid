public class Records
{
    public int levelNumber;
    public int currentScore;
    public int live;
    public int highScore;
}

public class GameState
{
    private static GameState instance;

    public static GameState Instance
    {
        get
        {
            if (instance != null) return instance;
            else return instance = new GameState();
        }
    }

    public int[] curentLevel { get; private set; }
    public Records records { get; private set; }

    public void LoadGame()
    {
        curentLevel = DataSerialize.Instance.LoadLevel();
        records = DataSerialize.Instance.LoadRecord;
    }

    public void NewGame()
    {
        curentLevel = null;
        records.live = 3;
        records.currentScore = 0;
        records.highScore = records.highScore;
        records.levelNumber = 1;
        DataSerialize.Instance.SaveRecord(records);
        DataSerialize.Instance.SaveLevel("0");
    }

}
