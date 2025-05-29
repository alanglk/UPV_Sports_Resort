using UnityEngine;
using TMPro;

public class PointsCounter : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;

    public Respawning ballRespawning;

    public AudioClip sonidoPunto;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        if (scoreText != null)
            scoreText.text = "Score: " + score;
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
                    PlayerPoint();
                    isFirstServe = false; // Después del primer punto, cambia a modo normal
                }
                else
                {
                    // Se sigue jugando, pero no se puntúa
                    ballState = BallState.BouncedOnOpponentField;
                    isFirstServe = false;
                }
            }
        }


        // MODO NORMAL
        else
        {
            if (other.gameObject.CompareTag("MyField") && ballState == BallState.HitByOpponent)
                ballState = BallState.WaitingForHit;

            if (other.gameObject.CompareTag("OpponentField") && ballState == BallState.HitByPaddle)
                PlayerPoint();
        }
    }

    void PlayerPoint(){
        score++;
        ballState = BallState.BouncedOnOpponentField;

        audioSource.PlayOneShot(sonidoPunto);

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
