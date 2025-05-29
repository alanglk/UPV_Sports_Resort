using UnityEngine;
using TMPro;

public class PointsCounter : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;

    public Respawning ballRespawning;

    public enum BallState
    {
        WaitingForHit,
        HitByPaddle,
        BouncedOnMyField,
        HitByOpponent,
        BouncedOnOpponentField
    }

    public BallState ballState = BallState.WaitingForHit;
    private bool isFirstServe = true;

    void Start()
    {
        UpdateScoreText(score);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bat") && (ballState == BallState.WaitingForHit || ballState == BallState.HitByOpponent))
            ballState = BallState.HitByPaddle;

        else if (collision.gameObject.CompareTag("OpponentBat") && (ballState == BallState.BouncedOnOpponentField))
            ballState = BallState.HitByOpponent;

        else if (collision.gameObject.CompareTag("Ground"))
            RespawnBall();
    }

    void OnTriggerEnter(Collider other)
    {
        // MODO SAQUE
        if (isFirstServe)
        {
            if (other.gameObject.CompareTag("MyField"))
            {
                if (ballState == BallState.HitByPaddle)
                    ballState = BallState.BouncedOnMyField;
                else
                    ballState = BallState.WaitingForHit;
            }
            else if (other.gameObject.CompareTag("OpponentField"))
            {
                if (ballState == BallState.BouncedOnMyField)
                {
                    score++;
                    UpdateScoreText(score);
                    isFirstServe = false; // Después del primer punto, cambia a modo normal
                    ballState = BallState.BouncedOnOpponentField;
                }
                else
                    RespawnBall();
            }
        }


        // MODO NORMAL
        else
        {
            if (other.gameObject.CompareTag("MyField") && ballState == BallState.HitByOpponent)
                ballState = BallState.WaitingForHit;
            
            if (other.gameObject.CompareTag("OpponentField") && ballState == BallState.HitByPaddle){
                score++;
                UpdateScoreText(score);
                ballState = BallState.BouncedOnOpponentField;
            }
        }
    }

    void UpdateScoreText(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void RespawnBall(){
        // Ha tocado el suelo. Se respawnea y hay que volver a sacar
        ballRespawning.RespawnObjectAfterDelay();
        ballState = BallState.WaitingForHit;
        isFirstServe = true;
    }
}
