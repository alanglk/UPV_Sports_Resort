using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 120f; // 3 minutos en segundos
    public TMP_Text timerText;
    public bool timerIsRunning = true;

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                // Cambia el color a rojo si queda 1 minuto o menos
                if (timeRemaining <= 60f)
                {
                    timerText.color = Color.red;
                }

                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);

                // Fin del juego: pantalla negra
                
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timeToDisplay > 0)
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        else
            timerText.text = "Finish!";
    }
}
