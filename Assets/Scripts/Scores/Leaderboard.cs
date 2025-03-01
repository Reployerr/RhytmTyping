using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;
    [SerializeField] private Transform scoresContainer; 
    [SerializeField] private GameObject scorePrefab; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SaveScore(int score, string songName)
    {
        List<ScoreEntry> scores = LoadScores(songName);
        scores.Add(new ScoreEntry { score = score, date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

        // Сортируем по убыванию очков
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        LeaderboardData data = new LeaderboardData { scores = scores };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetLeaderboardFilePath(songName), json);
    }

    public List<ScoreEntry> LoadScores(string songName)
    {
        string path = GetLeaderboardFilePath(songName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            return data.scores;
        }
        else
        {
            LeaderboardData newData = new LeaderboardData();
            string json = JsonUtility.ToJson(newData, true);
            File.WriteAllText(path, json);
            return newData.scores;
        }
    }

    public void DisplayScores(string songName)
    {
        foreach (Transform child in scoresContainer)
        {
            Destroy(child.gameObject);
        }

        List<ScoreEntry> scores = LoadScores(songName);
        foreach (ScoreEntry entry in scores)
        {
            GameObject scoreEntry = Instantiate(scorePrefab, scoresContainer);
            scoreEntry.GetComponentInChildren<TMP_Text>().text = $"{entry.score} ({entry.date})";
        }
    }
    private string GetLeaderboardFilePath(string songName)
    {
        return Path.Combine(Application.persistentDataPath, $"{songName}_leaderboard.json");
    }

}

[System.Serializable]
public class ScoreEntry
{
    public int score;
    public string date;
}

[System.Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}
