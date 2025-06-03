using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager  : MonoBehaviour
{
    [Header("Objects")]
    public GameObject[] gameObjects;
    public bool isActive;

    [Header("Cannons")]
    public Transform[] cannonFirePoints;

    [Header("Ball")]
    public GameObject[] ballPrefabs;
    public Transform target;
    public float maxHeight = 6f;

    [Header("Timing")]
    public float launchInterval = 4f;
    
    [Header("Timing")]
    private float timer = 120f;
    public TMP_Text timerText;
    public TMP_Text scoreText;

    private int score = 0;
    private bool juegoActivo = true;

    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start() {
        isActive = false;
    }
    void InitializeMinigame()
    {
        score = 0;
        timer = 120f;
        isActive = true;

        scoreText.text = "Score: 0";
        timerText.text = "Time: 02:00";
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(LaunchRoutine());
    }

    void Update() {
        if (isActive) {
            if (!juegoActivo) return;
            timer -= Time.deltaTime;

            if(timer <= 2f)
            {
                timer = 0f;
                EndMiniGame();
            }
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"Tiempo: {minutes:00}:{seconds:00}";
        }
    }

    IEnumerator LaunchRoutine() {
        while (timer > 0f) {
            yield return new WaitForSeconds(launchInterval);
            LaunchRandomCannon();
            audioSource.PlayOneShot(hitSound);
        }
    }

    void LaunchRandomCannon() {
        int index = Random.Range(0, cannonFirePoints.Length);
        Transform cannon = cannonFirePoints[index];

        GameObject ball = Instantiate(ballPrefabs[index], cannon.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = true;

        Vector3 velocity = CalculateLaunchVelocity(target.position, cannon.position, maxHeight);
        rb.velocity = velocity;
        // Tag it for the bat to detect
        Destroy(ball, 5f);
    }

    Vector3 CalculateLaunchVelocity(Vector3 target, Vector3 origin, float h) {
        float gravity = Physics.gravity.y;
        float displacementY = target.y - origin.y;
        Vector3 displacementXZ = new Vector3(target.x - origin.x, 0, target.z - origin.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }
    
    public void AddScore(int amount)
    {
        if(launchInterval > 1.0f) {
            launchInterval = launchInterval * 0.96f;
        }
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void EndMiniGame() {
        scoreText.text = "Game Over\nScore: " + score;
        timerText.text = "Time: 00:00";
        isActive = false;
    }

    int getScore() {
        return score;
    }

    void loadMiniGame() {
        foreach(GameObject obj in gameObjects) { obj.SetActive(true); } 
    }

    void unloadMiniGame() {
        foreach(GameObject obj in gameObjects) { obj.SetActive(false); } 
    }
}