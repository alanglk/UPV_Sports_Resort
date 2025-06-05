using UnityEngine;
using TMPro;

public class PointsCounter : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text debugText;
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
    public bool isFirstServe = true;
    public GameTimer gameTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        UpdateDebug();
    }

    void Update()
    {
        UpdateDebug();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameTimer.timerIsRunning){
            if (collision.gameObject.CompareTag("Bat") && (ballState == BallState.WaitingForHit || ballState == BallState.HitByOpponent))
                ballState = BallState.HitByPaddle;

            else if (collision.gameObject.CompareTag("OpponentBat") && (ballState == BallState.BouncedOnOpponentField))
                ballState = BallState.HitByOpponent;
        }

        if (collision.gameObject.CompareTag("Ground"))
            RespawnBall();
    }

    void OnTriggerEnter(Collider other)
    {
        // JUEGO EN CURSO
        if (gameTimer.timerIsRunning)
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
                    if (ballState == BallState.BouncedOnMyField || ballState == BallState.HitByPaddle)
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
        // JUEGO FNIALIZADO
        else{
            ballState = BallState.WaitingForHit;
        }
    }

    void PlayerPoint()
    {
        score++;
        ballState = BallState.BouncedOnOpponentField;

        audioSource.PlayOneShot(sonidoPunto);

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void RespawnBall()
    {
        // Ha tocado el suelo. Se respawnea y hay que volver a sacar
        ballRespawning.RespawnObjectAfterDelay();
        ballState = BallState.WaitingForHit;
        isFirstServe = true;
    }

    void UpdateDebug()
    {
        if (debugText != null)
            debugText.text = "DebugState: " + ballState.ToString() + "\nIsFirstServe: " + isFirstServe.ToString();
    }
}
