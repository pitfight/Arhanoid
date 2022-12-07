using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator: MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private Vector2 startPosSpawn;
    [SerializeField] private int countColumn = 10;
    [SerializeField] private int countRow = 10;
    [SerializeField] private TextAsset[] levels;

    private Vector2 difSizeBrick = new Vector2(0.7f, 0.4f);

    private List<Brick> bricks = new List<Brick>();

    public void SpawnBricks()
    {
        Vector2 spawnPoint = startPosSpawn;
        for (int column = 0; column < countColumn; column++)
        {
            for (int row = 0; row < countRow; row++)
            {
                var brick = Instantiate(this.brick, spawnPoint, Quaternion.identity, transform).GetComponentInChildren<Brick>();
                bricks.Add(brick);
                spawnPoint.x += difSizeBrick.x;
            }
            spawnPoint.x = startPosSpawn.x;
            spawnPoint.y -= difSizeBrick.y;
        }
    }

    public void StartLevel(int number)
    {
        SetLevel(ReadLevel(levels[number - 1]));
        SetPowerBrick();
    }

    public void StartLevel(int[] level)
    {
        SetLevel(level);
    }

    public int[] ReadLevel(TextAsset level)
    {
        List<int> bricks = new List<int>();
        foreach (var brick in level.text)
            if (char.IsDigit(brick))
                bricks.Add((int)brick - '0');
        return bricks.ToArray();
    }

    public int[] ReadLevel(List<Brick> level)
    {
        List<int> bricks = new List<int>();
        foreach (var brick in level)
                bricks.Add(brick.GetGrade());
        return bricks.ToArray();
    }

    public void SetLevel(int[] level)
    {
        for (int i = 0; i < level.Length; i++)
            bricks[i].SetGrade(level[i]);
    }

    public void SetPowerBrick()
    {
        int countBrick = GetActiveBricks() / 10;
        foreach (Brick brick in bricks)
            if (brick.isActive)
            {
                countBrick--;
                brick.hasPowerUp = true;
                if (countBrick == 0)
                    break;
            }
    }

    public string GetCurentLevel()
    {
        string bricks = string.Empty;
        foreach (var brick in this.bricks)
            bricks += brick.GetGrade() + "";
        return bricks;
    }

    public int GetActiveBricks()
    {
        return bricks.FindAll(x => x.isActive).Count;
    }

    public List<Brick> GetBricks()
    {
        return bricks;
    }

    public int GetCountLevels()
    {
        return levels.Length;
    }
}
