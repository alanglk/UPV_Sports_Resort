using UnityEngine;

public class CanonController : MonoBehaviour {
    public GameObject ballPrefab;
    public Transform firePoint;
    public Transform targetPoint;
    public float maxHeight = 6f;
    public float launchDelay = 3f;

    private float launchTimer;

    void Update() {
        launchTimer += Time.deltaTime;

        if (launchTimer >= launchDelay) {
            launchTimer = 0f;
            LaunchBall();
        }
    }

    void LaunchBall() {
        GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = true;

        Vector3 velocity = CalculateLaunchVelocity(targetPoint.position, firePoint.position, maxHeight);
        rb.velocity = velocity;
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
}
