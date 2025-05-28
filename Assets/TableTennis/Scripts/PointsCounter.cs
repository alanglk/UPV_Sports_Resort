using UnityEngine;
using TMPro;

public class PointsCounter : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;

    private enum BallState
    {
        WaitingForHit,
        HitByPaddle,
        BouncedOnMyField
    }

    private BallState ballState = BallState.WaitingForHit;
    private bool isFirstServe = true;

    void Start()
    {
        UpdateScoreText(score);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bat") && ballState == BallState.WaitingForHit)
        {
            ballState = BallState.HitByPaddle;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isFirstServe)
        {
            // MODO SAQUE
            if (other.gameObject.CompareTag("MyField"))
            {
                if (ballState == BallState.HitByPaddle)
                {
                    ballState = BallState.BouncedOnMyField;
                }
                else
                {
                    ballState = BallState.WaitingForHit;
                }
            }
            else if (other.gameObject.CompareTag("OpponentField"))
            {
                if (ballState == BallState.BouncedOnMyField)
                {
                    score++;
                    UpdateScoreText(score);
                    isFirstServe = false; // Después del primer punto, cambia a modo normal
                }
                ballState = BallState.WaitingForHit;
            }
        }
        else
        {
            // MODO NORMAL: suma punto solo por tocar el campo contrario
            if (other.gameObject.CompareTag("OpponentField"))
            {
                score++;
                UpdateScoreText(score);
                ballState = BallState.WaitingForHit;
            }
            else if (other.gameObject.CompareTag("MyField"))
            {
                ballState = BallState.WaitingForHit;
            }
            else if (other.gameObject.CompareTag("Ground")) // Ha tocado el suelo. Se respawnea y hay que volver a sacar
            {
                ballState = BallState.WaitingForHit;
                isFirstServe = true;
            }
        }
    }

    void UpdateScoreText(int score)
    {
        if (scoreText != null)
            scoreText.text = "Puntuación: " + score;
    }
}
