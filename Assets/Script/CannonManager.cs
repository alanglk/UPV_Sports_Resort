using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI display

public class CannonManager : MonoBehaviour {
    [Header("Cannons")]
    public Transform[] cannonFirePoints;

    [Header("Ball")]
    public GameObject[] ballPrefabs;
    public Transform target;
    public float maxHeight = 6f;

    [Header("Timing")]
    public float launchInterval = 3f;

    // [Header("UI")]
    // public Text hitCounterText;

    // private int hitCount = 0;

    void Start() {
        StartCoroutine(LaunchRoutine());
        // UpdateHitUI();
    }

    IEnumerator LaunchRoutine() {
        while (true) {
            yield return new WaitForSeconds(launchInterval);
            LaunchRandomCannon();
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
        ball.tag = "Ball";
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

    // public void RegisterHit() {
    //     hitCount++;
    //     UpdateHitUI();
    // }

    // void UpdateHitUI() {
    //     if (hitCounterText != null)
    //         hitCounterText.text = "Hits: " + hitCount;
    // }
}
