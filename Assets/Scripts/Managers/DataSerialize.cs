using System.Collections.Generic;
using UnityEngine;

public class DataSerialize : PlayerPrefs
{
    private static DataSerialize instance;

    private const string RECORD_LEVEL = "Level";
    private const string RECORD_LEVEL_NUMBER = "Level_Number";
    private const string RECORD_CURECNT_SCORE = "CurScore";
    private const string RECORD_HIGH_SCORE = "HighScore";
    private const string RECORD_LIVE = "Live";

    public static DataSerialize Instance
    {
        get
        {
            if (instance != null) return instance;
            else return instance = new DataSerialize();
        }
    }

    public Records LoadRecord => new Records
    {
        currentScore = GetInt(RECORD_CURECNT_SCORE, 0),
        highScore = GetInt(RECORD_HIGH_SCORE, 0),
        live = GetInt(RECORD_LIVE, 3),
        levelNumber = GetInt(RECORD_LEVEL_NUMBER, 1)
    };

    public void SaveRecord(Records records)
    {
        SetInt(RECORD_CURECNT_SCORE, records.currentScore);
        SetInt(RECORD_HIGH_SCORE, records.highScore);
        SetInt(RECORD_LIVE, records.live);
        SetInt(RECORD_LEVEL_NUMBER, records.live);

        Save();
    }

    public void SaveLevel(string level)
    {
        if (level.Length < 2) DeleteKey(RECORD_LEVEL);
        else SetString(RECORD_LEVEL, level);

        Save();
    }

    public int[] LoadLevel()
    {
        if (HasKey(RECORD_LEVEL))
        {
            List<int> level = new List<int>();
            foreach (char brick in GetString(RECORD_LEVEL))
                level.Add((int)brick - '0');
            return level.ToArray();
        }
        return null;
    }

    public bool GetBool(string name)
    {
        return GetInt(name, 1) == 1 ? true : false;
    }

    public void SetBool(string name, bool value)
    {
        SetInt(name, value ? 1 : 0);
    }
}
