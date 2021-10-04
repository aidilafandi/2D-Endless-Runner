using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroundScript : MonoBehaviour
{
    [SerializeField] private Transform[] movingGround;

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.name=="MainCharacter")
        {
            StartCoroutine(move());
        }
    }

    private IEnumerator move()
    {
        if (movingGround[0].position.x > movingGround[2].position.x+1.2f)
        {
            yield return new WaitForSeconds(0.02f);
            movingGround[0].position = new Vector2(movingGround[0].position.x-0.2f, movingGround[0].position.y);
            movingGround[1].position = new Vector2(movingGround[1].position.x-0.2f, movingGround[1].position.y);
            StartCoroutine(move());
        }
    }

    private void OnEnable()
    {
        movingGround[0].position = new Vector2(movingGround[2].position.x+4.8f, movingGround[2].position.y);
        movingGround[1].position = new Vector2(movingGround[2].position.x+6f, movingGround[2].position.y);
    }
}
