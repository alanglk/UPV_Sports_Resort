using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatCollider : MonoBehaviour
{
    public float hitForce = 20f;
    public GameManager gameManager;

    void OnCollisionEnter(Collision collision)
    {
        GameObject ball = collision.gameObject;
        if (ball.CompareTag("ball_baseball") || ball.CompareTag("ball_soccer") || ball.CompareTag("ball_tennis") || ball.CompareTag("ball_basketball"))
        {
            StoreResults(ball);
            LaunchBall(collision);
        }
    }

    private void StoreResults(GameObject ball)
    {
        // int pointsToAdd = 0;

        if (ball.CompareTag("ball_baseball"))
            gameManager.AddScore(3);
        else if (ball.CompareTag("ball_tennis"))
            gameManager.AddScore(2);
        else if (ball.CompareTag("ball_soccer"))
            gameManager.AddScore(1);
        else if (ball.CompareTag("ball_basketball"))
            gameManager.AddScore(1);

    }

    private void LaunchBall(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            // Direcci�n desde el bate hacia el centro de contacto
            Vector3 hitDirection = collision.contacts[0].point - transform.position;
            hitDirection = hitDirection.normalized;

            // A�adir impulso
            rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
        }
    }
}
