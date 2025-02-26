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
        List<int> scores = LoadScores(songName);
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a));

        LeaderboardData data = new LeaderboardData { scores = scores };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetLeaderboardFilePath(songName), json);
    }

    public List<int> LoadScores(string songName)
    {
        string path = GetLeaderboardFilePath(songName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            return data.scores;
        }
        return new List<int>();
    }

    public void DisplayScores(string songName)
    {
        foreach (Transform child in scoresContainer)
        {
            Destroy(child.gameObject);
        }

        List<int> scores = LoadScores(songName);
        foreach (int score in scores)
        {
            GameObject scoreEntry = Instantiate(scorePrefab, scoresContainer);
            scoreEntry.GetComponent<TMP_Text>().text = score.ToString();
        }
    }
    private string GetLeaderboardFilePath(string songName)
    {
        return Path.Combine(Application.persistentDataPath, $"{songName}_leaderboard.json");
    }

}

[System.Serializable]
public class LeaderboardData
{
    public List<int> scores;
    public string date;
}
