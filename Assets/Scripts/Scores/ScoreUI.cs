using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Animator _animator;

    public static ScoreUI Instance;
    void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(string score)
  {
        _scoreText.text = score;
        PlayScoreAnimation();
  }

  private void PlayScoreAnimation()
    {
        _animator.Play("GetScore");
    }
}
