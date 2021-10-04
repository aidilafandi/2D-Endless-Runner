using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveAccel;
    [SerializeField] private float maxSpeed;

    private Rigidbody2D rig;

    [Header("Jump")]
    [SerializeField] private float jumpAccel;

    private bool isJumping;
    private bool isOnGround;

    [Header("Ground Raycast")]
    [SerializeField] private float groundRaycastDistance;
    [SerializeField] private LayerMask groundLayerMask;

    private Animator anim;
    private CharacterAudioController sound;

    [Header("Scoring")]
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private float scoringRatio;

    private float lastPosition;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private float fallPosition;
    [SerializeField] CameraMoveController cameraMove;

    [Header("Skill")]
    [SerializeField] private float skillValue;
    [SerializeField] private Slider skillBar;
    [SerializeField] private Button skillButton;
    [SerializeField] private Color[] clr;
    private Image skillBarColor;
    private bool charging = true;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterAudioController>();
        skillBarColor = skillBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        skillBarColor.color = Color.red;
        lastPosition = transform.position.x;
        skillBar.maxValue = skillValue;
    }

    private void Update()
    {
        // read input
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;

                sound.PlayJump();
            }
        }

        // change animation
        anim.SetBool("OnGround", isOnGround);

        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPosition);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            scoreController.incrementCurrentScore(scoreIncrement);
            lastPosition += distancePassed;
        }

        if (charging)
        {
            if (skillBar.value == skillBar.maxValue) fullSkillBar();
            else skillBar.value++;
        }
        if (transform.position.y < fallPosition)
        {
            gameOver();
        }
    }

    private void FixedUpdate()
    {
        // raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if (!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        // calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }

    private void fullSkillBar()
    {
        skillBarColor.color = Color.green;
        skillButton.interactable = true;
    }

    public void OnSkill()
    {
        sound.PlaySkill();
        Time.timeScale = 0.5f;
        skillButton.interactable = false;
        skillBar.value = 0;
        skillBarColor.color = Color.red;
        charging = false;
        Camera.main.backgroundColor = clr[1];
        StartCoroutine(endSkill());
    }

    private IEnumerator endSkill()
    {
        yield return new WaitForSeconds(5);
        Time.timeScale = 1f;
        Camera.main.backgroundColor = clr[0];
        charging = true;
        sound.PlayEndSkill();
    }

    public void speedUp()
    {
        moveAccel+=0.5f;
        maxSpeed += 1;
        jumpAccel = jumpAccel < 1.4f ? jumpAccel : jumpAccel - 0.2f;
        skillBar.maxValue += skillValue*0.6f;
        skillButton.interactable = false;
        skillBarColor.color = Color.red;
    }

    private void gameOver()
    {
        scoreController.Finish();
        cameraMove.enabled = false;
        Time.timeScale = 1f;
        Camera.main.backgroundColor = clr[0];
        sound.PlayGameOver();
        gameOverText.SetActive(true);

        this.enabled = false;
    }
}
