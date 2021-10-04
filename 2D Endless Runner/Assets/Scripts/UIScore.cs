using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScore : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text highScore;

    [SerializeField] private ScoreController scoreController;

    private void Update()
    {
        score.text = "My Score :" + scoreController.GetCurrentScore();
        highScore.text = "High Score :" + ScoreScript.highScore;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
