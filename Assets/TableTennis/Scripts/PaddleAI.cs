using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class PaddleAI : MonoBehaviour
{

    public Transform ball;
    public BallTrajectoryPredictor ballPredictor;
    public BallController ballController;
    public PointsCounter ballPointsCounter;

    public Transform targetTransform;
    public float maxHeight = 6.0f;

    public float minDistance = 0.5f;    // min distance (m) to compute the target point and speed

    private Transform paddleTransform;
    private Rigidbody paddleRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        paddleTransform = GetComponent<Transform>();
        paddleRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ballPointsCounter.ballState == PointsCounter.BallState.BouncedOnOpponentField)
            OpponentAction();

    }


    void OpponentAction()
    {
        // Get current ball trajectory
        ODE.ODEProblemSolution<double>? ballTrajectory = ballPredictor.GetBallTrajectory();

        // Move paddle to catch the ball
        if (ballTrajectory != null)
            MovePaddleToCatchBall(ballTrajectory.Value);

        // Check if the ball collides with the paddle and launch the ball if so
        Vector3 direction = ball.position - paddleRigidbody.position;
        float distanceToBall = direction.magnitude;

        if (distanceToBall < minDistance){
            direction.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(paddleRigidbody.position, direction, out hit, minDistance)){
                
                if (hit.transform == ball){
                    Debug.Log("La pala ha colisionado con la pelota!");
                    Vector3 targetPoint = GetRandomTargetPoint(targetTransform, 1.0f);
                    ballController.LaunchBallToTarget(targetTransform.position, maxHeight);
                }
            }
        }
    }


    void MovePaddleToCatchBall(ODE.ODEProblemSolution<double> ballTrajectory){
        float paddleX = paddleTransform.position.x;
        Vector3? interceptPoint = null;

        // Recorremos la trayectoria buscando un cruce de X
        for (int i = 1; i < ballTrajectory.UU.Count; i++)
        {
            Vector3 prev = new Vector3(
                (float)ballTrajectory.UU[i - 1][0],
                (float)ballTrajectory.UU[i - 1][1],
                (float)ballTrajectory.UU[i - 1][2]);

            Vector3 current = new Vector3(
                (float)ballTrajectory.UU[i][0],
                (float)ballTrajectory.UU[i][1],
                (float)ballTrajectory.UU[i][2]);

            // Si hay un cruce del plano X = paddleX entre prev.x y current.x
            if ((prev.x - paddleX) * (current.x - paddleX) <= 0)
            {
                // Interpolación lineal para encontrar el punto exacto
                float t = (paddleX - prev.x) / (current.x - prev.x);
                float y = Mathf.Lerp(prev.y, current.y, t);
                float z = Mathf.Lerp(prev.z, current.z, t);
                interceptPoint = new Vector3(paddleX, y, z);
                break;
            }
        }

        if (interceptPoint.HasValue)
        {
            Vector3 point = interceptPoint.Value;
            // Mostrar punto con Debug
            Debug.DrawRay(point, Vector3.up * 0.5f, Color.red, 1f);
            // Mover pala (solo en Y y Z)
            paddleTransform.position = new Vector3(paddleTransform.position.x, point.y, point.z);
        }
        else
        {
            Debug.LogWarning("No se encontró intersección de la trayectoria con el plano X de la pala.");
        }
    }


    Vector3 GetRandomTargetPoint(Transform plane, float difficulty){
        float bias = Mathf.Lerp(3f, 1f, difficulty); // 3 = más al centro, 1 = uniforme
        // Multiplicamos por 5 porque Unity plane por defecto es de 10x10 en world units
        float x = BiasedRandom(bias) * (plane.localScale.x * 5f);
        float z = BiasedRandom(bias) * (plane.localScale.z * 5f);

        Vector3 localPoint = new Vector3(x, 0f, z);
        Vector3 worldPoint = plane.TransformPoint(localPoint);

        // Debug
        Debug.DrawRay(worldPoint, Vector3.up * 0.5f, Color.green, 1f);
        return worldPoint;
    }

    float BiasedRandom(float bias){
        float u =  UnityEngine.Random.value * 2 - 1; // en [-1, 1]
        u = Mathf.Sign(u) * Mathf.Pow(Mathf.Abs(u), bias);
        return u;
    }

    
}
