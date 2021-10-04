using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip scoreHighlight;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip skill;
    [SerializeField] private AudioClip endSkill;


    private AudioSource audioPlayer;

    private void Start() => audioPlayer = GetComponent<AudioSource>();

    public void PlayJump() => audioPlayer.PlayOneShot(jump);

    public void PlayScoreHighlight() => audioPlayer.PlayOneShot(scoreHighlight);

    public void PlayGameOver() => audioPlayer.PlayOneShot(gameOver);

    public void PlaySkill() => audioPlayer.PlayOneShot(skill);
    

    public void PlayEndSkill() => audioPlayer.PlayOneShot(endSkill);
}
