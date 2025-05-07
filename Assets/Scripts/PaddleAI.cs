using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    public Transform ball;
    public float speed = 5f;
    public float followAxis = 0f;

    // Start is called before the first frame update
    void Start() {}

    void Update()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.z = Mathf.MoveTowards(transform.position.z, ball.position.z, speed * Time.deltaTime);
        transform.position = targetPosition;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ball")){
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = (Vector3.zero - transform.position).normalized;
            rb.velocity = direction * 5f; // o usa AddForce si quieres un impulso
        }

    }
    public Vector3 FindBestShot(Vector3 start, Vector3 target, Func<Vector3, List<Vector3>> simulateTrajectory, float tolerance = 0.2f){
        float speedMin = 1f;
        float speedMax = 10f;
        int directions = 20;

        for (float speed = speedMin; speed <= speedMax; speed += 0.5f){
            for (int i = 0; i < directions; i++){
                for (int j = 0; j < directions; j++){
                    Vector3 dir = new Vector3(
                        Mathf.Sin(i * Mathf.PI * 2 / directions),
                        Mathf.Sin(j * Mathf.PI / directions),
                        Mathf.Cos(i * Mathf.PI * 2 / directions)
                    ).normalized;

                    Vector3 velocity = dir * speed;
                    List<Vector3> trajectory = simulateTrajectory(velocity);

                    if (trajectory.Any(p => Vector3.Distance(p, target) < tolerance)){
                        return velocity;
                    }
                }
            }
        }

        return Vector3.zero; // No se encontró
    }

}
