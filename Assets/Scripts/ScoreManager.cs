using UnityEngine;

public class ScoreManager
{
    private int _currentScore;
    private int _maxScore;

    public int CurrentScore => _currentScore;
    public int MaxScore => _maxScore;

    public void AddScore(int points)
    {
        _currentScore += points;
        if (_currentScore > _maxScore)
        {
            _maxScore = _currentScore;
        }
    }

    public void ResetScore()
    {
        _currentScore = 0;
    }

    public void SaveScore(string songName)
    {
        PlayerPrefs.SetInt($"{songName}_MaxScore", _maxScore);
        PlayerPrefs.Save();
    }

    public void LoadScore(string songName)
    {
        _maxScore = PlayerPrefs.GetInt($"{songName}_MaxScore", 0);
    }
}
