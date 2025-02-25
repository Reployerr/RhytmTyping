using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        string key = $"Leaderboard_{songName}";
        List<int> scores = LoadScores(songName);

        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); 


        PlayerPrefs.SetString(key, string.Join(",", scores));
        PlayerPrefs.Save();
    }

    public List<int> LoadScores(string songName)
    {
        string key = $"Leaderboard_{songName}";
        string savedScores = PlayerPrefs.GetString(key, "");

        List<int> scores = new List<int>();
        if (!string.IsNullOrEmpty(savedScores))
        {
            string[] scoreArray = savedScores.Split(',');
            foreach (string score in scoreArray)
            {
                if (int.TryParse(score, out int parsedScore))
                {
                    scores.Add(parsedScore);
                }
            }
        }

        return scores;
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

}
