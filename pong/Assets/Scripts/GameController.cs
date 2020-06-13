using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private int LeftScore;
    [SerializeField] private int RightScore;
    [SerializeField] private int ScoreToWin;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private RacketMovement leftPaddle;
    [SerializeField] private RacketMovement rightPaddle;

    Buttons button;

    BallMovement ball;

    private RacketMovement.Side ServeSide;

    private void Start()
    {
        ball = FindObjectOfType<BallMovement>();
        leftPaddle = rightPaddle = FindObjectOfType<RacketMovement>();
        button = FindObjectOfType<Buttons>();
    }

    public void Scores(RacketMovement.Side side)
    {
        if(side == RacketMovement.Side.Left)
        {
            LeftScore++;
        }
        else if(side == RacketMovement.Side.Right)
        {
            RightScore++;
        }

        UIManager.scoreUpdate(LeftScore, RightScore);
        ServeSide = side;

        if (IsGameOver() == true)
        {
            SceneManager.LoadScene(0);
            LeftScore = RightScore = 0;
        }
        else
        {
            ResetGame();
        }
    }

    private bool IsGameOver()
    {
        bool result = false;
        if(LeftScore >= ScoreToWin || RightScore >= ScoreToWin)
        {
            result = true;
        }
       
        return result;
    }

    
    public void ResetGame()
    {
        ball.Reset(ServeSide);
        leftPaddle.Reset();
        rightPaddle.Reset();
    }

    public void PlayOnePlayer()
    {
        LeftScore = RightScore = 0;
        ResetGame();
        rightPaddle.isAI = true;
        leftPaddle.isAI = false;
        
    }

   
}
