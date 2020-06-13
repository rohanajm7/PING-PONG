using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 5f;

    public bool isAI;

    private BoxCollider2D col;

    private float RandomYOffset;

    private bool firstIncoming;

    private bool OverridePos;

    BallMovement ball;

    private Vector2 ForwardDir;

    [SerializeField] private float resetTime;

    public enum Side { Left , Right};
    [SerializeField] Side side;

    private void Start()
    {
        ball = FindObjectOfType<BallMovement>();
        col = GetComponent<BoxCollider2D>();

        if(side == Side.Left)
        {
            ForwardDir = Vector2.right;
        }
        else if(side == Side.Right)
        {
            ForwardDir = Vector2.left;
        }
    }

    private void Update()
    {
        if (!OverridePos)
        {
            MovePaddle();
        }
    }

    private void MovePaddle()
    {
        float TargetYpos = NewY();

        ClampPos(ref TargetYpos);

        transform.position = new Vector3(transform.position.x, TargetYpos, transform.position.z);
    }

    private void ClampPos(ref float Ypos)
    {
        float MinY = Camera.main.ScreenToWorldPoint(new Vector3(0, 1)).y;
        float MaxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 1)).y;

        Ypos = Mathf.Clamp(Ypos, MinY, MaxY);
    }

    private float NewY()
    {
        float result = transform.position.y;

        if (isAI)
        {
            if (ballincoming())
            {
                if (firstIncoming)
                {
                    firstIncoming = false;
                    RandomYOffset = ROffset();
                }
                result = Mathf.MoveTowards(transform.position.y, ball.transform.position.y, MoveSpeed * Time.deltaTime);
            }
            else
            {
                firstIncoming = true;
            }
        }
        else
        {
            float movement = Input.GetAxisRaw("Vertical") * Time.deltaTime * MoveSpeed;
            result = transform.position.y + movement;
        }
        return result;
    }

    private bool ballincoming()
    {
        float DotP = Vector2.Dot(ball.velocity, ForwardDir);
        return DotP < 0f;
    }

    private float ROffset()
    {
        float MaxOffset = col.bounds.extents.y;
        return UnityEngine.Random.Range(-MaxOffset , MaxOffset);
    }

    public void Reset()
    {
        StartCoroutine(resetRoutine());
    }

    private IEnumerator resetRoutine()
    {
        OverridePos = true;
        float CurrentYPos = transform.position.y;
        for(float timer = 0; timer < resetTime; timer += Time.deltaTime)
        {
            float TargetPos = Mathf.Lerp(CurrentYPos, 6f, timer / resetTime);
            transform.position = new Vector3(transform.position.x, TargetPos, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 6f, transform.position.z);
        OverridePos = false;
    }

   


}
