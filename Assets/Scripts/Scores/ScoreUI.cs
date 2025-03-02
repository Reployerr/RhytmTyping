using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _finalScore;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private GameObject _finalUI;
    [SerializeField] private GameObject _playUI;

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
    public void ShowFinalResult(string finalResult)
    {
        ClearUI();

        _playUI.SetActive(false);
        _finalUI.SetActive(true);
        _finalScore.text = finalResult;
    }

    public void BackToSelection()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   private void PlayScoreAnimation()
   {
        _animator.Play("GetScore");
   }

    private void ClearUI()
    {
        _scoreText.text = "0";
    }
}
