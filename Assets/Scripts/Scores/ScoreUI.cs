using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _finalScore;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _selectionUI;

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

    public void ShowFinalResult(string finalResult)
    {
        gameObject.SetActive(true);
        _finalScore.text = finalResult;
    }

    public void BackToSelection()
    {
        _selectionUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
