using UnityEngine;
using TMPro;

public class Basketball : MonoBehaviour
{
    [Header("Componentes del minijuego")]
    public BallSpawner            ballSpawner;       // tu script que lanza pelotas
    public HoopGenerator          hoopGenerator;     // tu script que genera aros
    public InvisibleCapsuleWall   scoreManager;      // tu script que cuenta puntos

    [Header("Configuración de partida")]
    [Tooltip("Duración total del minijuego en segundos")]
    public float gameDuration = 180f;

    private float timer;

    public int temporizador;
    public int puntuacion;
    void Start()
    {
        temporizador = 180;    // por ejemplo, 60 segundos de partida
        puntuacion  = 0;
        // Arranca el minijuego
        timer = gameDuration;

        // Los Start() de BallSpawner, HoopGenerator e InvisibleCapsuleWall
        // se ejecutan aquí mismo, así que no necesitas llamarles nada más.
    }

    void Update()
    {
        // Cuenta atrás
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndMinigame();
        }
    }

    private void EndMinigame()
    {
        // 1) Detener la generación de nuevas pelotas
        // BallSpawner usa InvokeRepeating en su Start(), así que
        // le pedimos que cancele todas sus invokes.
        ballSpawner.CancelInvoke();

        // 2) Limpiar los aros que ha generado HoopGenerator
        foreach (Transform segment in hoopGenerator.transform)
        {
            Destroy(segment.gameObject);
        }

        // 3) Desactivar el trigger de puntuación
        scoreManager.enabled = false;

        // 4) Mostrar resultado final por consola (o ponte aquí tu UI)
        puntuacion = scoreManager.CurrentScore;
        Debug.Log($"🏀 Juego terminado – Puntuación final = {puntuacion}");

        // Opcional: desactivar este script para que no siga contando Update()
        this.enabled = false;
    }
}
