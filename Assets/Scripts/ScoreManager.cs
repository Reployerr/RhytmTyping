using System;
using System.IO;
using UnityEngine;

public class ScoreManager
{
    private int _currentScore;
    private int _maxScore;
    private string _lastScoreDate;

    public int CurrentScore => _currentScore;
    public int MaxScore => _maxScore;
    public string LastScoreDate => _lastScoreDate;

    public void AddScore(int points)
    {
        _currentScore += points;
        if (_currentScore > _maxScore)
        {
            _maxScore = _currentScore;
            _lastScoreDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }
    }

    public void ResetScore()
    {
        _currentScore = 0;
    }

    public void SaveScore(string songName)
    {
        string path = GetScoreFilePath(songName);
        ScoreData scoreData = new ScoreData
        {
            maxScore = _maxScore,
            lastScoreDate = _lastScoreDate
        };
        string json = JsonUtility.ToJson(scoreData);
        File.WriteAllText(path, json);
    }

    public void LoadScore(string songName)
    {
        string path = GetScoreFilePath(songName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoreData scoreData = new ScoreData { maxScore = _maxScore };
            _maxScore = scoreData.maxScore;
            _lastScoreDate = scoreData.lastScoreDate;
        }
        else
        {
            _maxScore = 0;
            _lastScoreDate = "No record";
        }
    }

    private string GetScoreFilePath(string songName)
    {
        return Path.Combine(Application.persistentDataPath, $"{songName}_score.json");
    }
}
[System.Serializable]
public class ScoreData
{
    public int maxScore;
    public string lastScoreDate;
}