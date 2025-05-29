using UnityEngine;
using System.Collections;

public class Respawning : MonoBehaviour
{
    public Transform respawnPoint; // Asigna el transform de tu objeto respawn en el inspector
    public string groundTag = "Ground"; // El tag que le pongas al suelo
    public float respawningDelay = 1.5f;
    public bool isTriggered = false; // Si esta en true, se llama manualmente al respawn

    private bool isRespawning = false; // Nueva variable para controlar el respawn


    private void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el suelo y no está ya en proceso de respawn
        if (!isTriggered && !isRespawning && collision.gameObject.CompareTag(groundTag))
            RespawnObjectAfterDelay();
    }

    public void RespawnObjectAfterDelay(){
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        isRespawning = true; // Marca que el respawn está en curso
        yield return new WaitForSeconds(respawningDelay);
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        // Si usas Rigidbody, resetea la velocidad
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        isRespawning = false; // Permite futuros respawns si es necesario
    }
}
