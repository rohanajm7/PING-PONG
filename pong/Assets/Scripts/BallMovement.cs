using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 velocity { get; private set; }
    [SerializeField] float Movespeed;

    Vector2 OriginalPos;

    [SerializeField] float MaxAngle = 45f;

    [SerializeField] private float serveAngle;

    private bool OverridePos;
    [SerializeField] private float resetTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.left * Movespeed;
        OriginalPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (OverridePos == false)
        {
            rb.velocity = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "paddle")
        {
            BounceFromPaddle(collision.collider);
        }
        else
        {
            BounceBall();
        }
    }

    private void Serve(RacketMovement.Side side)
    {
        Vector2 ServeDir = new Vector2(Mathf.Cos(serveAngle * Mathf.Deg2Rad) , Mathf.Sin(serveAngle * Mathf.Deg2Rad));
        ServeDir.y = -ServeDir.y;
        if(side == RacketMovement.Side.Left)
        {
            ServeDir.x = -ServeDir.x;
        }

        velocity = ServeDir * Movespeed;
    }

    private void BounceBall()
    {
        velocity = new Vector2(velocity.x , -velocity.y);
    }

    private void BounceFromPaddle(Collider2D collider)
    {
        float ColYExtents = collider.bounds.extents.y;
        float yOffset = transform.position.y - collider.transform.position.y;

        float yRatio = yOffset / ColYExtents;

        float bounceAngle = MaxAngle * yRatio * Mathf.Deg2Rad;

        Vector2 BounceDirection = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));

        BounceDirection *= Mathf.Sign(-velocity.x);

        velocity = BounceDirection * Movespeed;
    }

    public void Reset(RacketMovement.Side side)
    {
        StartCoroutine(resetRoutine(side));
    }

    private IEnumerator resetRoutine(RacketMovement.Side side)
    {
        transform.position = OriginalPos;
        rb.velocity = Vector2.zero;
        OverridePos = true;
        yield return new WaitForSeconds(resetTime);
        OverridePos = false;
        Serve(side);
    }
}
