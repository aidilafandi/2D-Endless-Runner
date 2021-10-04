using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int currentScore = 0;
    private int lastScoreHighLight = 0;

    [SerializeField] private int scoreHighlightRange;
    [SerializeField] private Transform mainCharacter;

    private CharacterAudioController audioController;
    private CharacterMoveController characterController;

    private void Start()
    {
        audioController = mainCharacter.GetComponent<CharacterAudioController>();
        characterController = mainCharacter.GetComponent<CharacterMoveController>();
        currentScore = 0;
        lastScoreHighLight = 0;
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public void incrementCurrentScore(int increment)
    {
        currentScore += increment;
        if (currentScore - lastScoreHighLight > scoreHighlightRange)
        {
            audioController.PlayScoreHighlight();
            lastScoreHighLight += Mathf.FloorToInt(scoreHighlightRange*1.6f);
            characterController.speedUp();
        }
    }

    public void Finish() => ScoreScript.highScore = currentScore > ScoreScript.highScore ? 
        currentScore : ScoreScript.highScore;
}
