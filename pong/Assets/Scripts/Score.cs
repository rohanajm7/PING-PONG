using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] RacketMovement.Side scoreSide;

    GameController game;

    Buttons button;

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        button = FindObjectOfType<Buttons>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallMovement ball = collision.GetComponent<BallMovement>();

        if (ball)
        {
            game.Scores(scoreSide);
        }
    }

}
