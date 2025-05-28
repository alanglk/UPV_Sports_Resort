using UnityEngine;

public class BatRV : MonoBehaviour {
    public float hitForce = 8f;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ball")) {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Estimate swing direction from bat's velocity
            Vector3 batVelocity = GetComponent<Rigidbody>().velocity;

            // Apply impulse to the ball
            rb.AddForce(batVelocity.normalized * hitForce, ForceMode.Impulse);
        
            // Notify the manager
            // FindObjectOfType<CannonManager>()?.RegisterHit();
        }
    }
}
