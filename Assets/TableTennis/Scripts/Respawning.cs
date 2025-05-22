using UnityEngine;
using System.Collections;


public class Respawning : MonoBehaviour
{
    public Transform respawnPoint; // Asigna el transform de tu objeto respawn en el inspector
    public string groundTag = "Ground"; // El tag que le pongas al suelo

    private void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el suelo
        if (collision.gameObject.CompareTag(groundTag))
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        // Si usas Rigidbody, resetea la velocidad
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}