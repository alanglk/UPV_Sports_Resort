using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class PaddleAI : MonoBehaviour{

    public Transform ball;
    public BallTrajectoryPredictor ballPredictor;
    public BallController ballController;

    public Transform targetTransform;
    public float maxHeight = 6.0f;

    public float maxSpeed = 1.0f;       // max entity possible speed (m/s)
    public float minDistance = 0.5f;    // min distance (m) to compute the target point and speed

    private Transform paddleTransform;
    private Rigidbody paddleRigidbody;

    // Start is called before the first frame update
    void Start() {
        paddleTransform = GetComponent<Transform>();
        paddleRigidbody = GetComponent<Rigidbody>();
    }

    void Update(){

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
            if ( Physics.Raycast(paddleRigidbody.position, direction, out hit, minDistance) ){
                if (hit.transform == ball){
                    Debug.Log("La pala ha colisionado con la pelota!");
                    ballController.LaunchBallToTarget(targetTransform.position, maxHeight);
                }
            }
        }


    }
    
    void MovePaddleToCatchBall(ODE.ODEProblemSolution<double> ballTrajectory ){
        // Calc interception point
        Vector3 interceptPoint = paddleTransform.position;

        for (int i = 0; i < ballTrajectory.TT.Count; i++){
            Vector3 t_position = new Vector3((float) ballTrajectory.UU[i][0], (float) ballTrajectory.UU[i][1], (float) ballTrajectory.UU[i][2] );
            float t_time = (float) ballTrajectory.TT[i];

            float distanceToPoint = Vector3.Distance(paddleRigidbody.position, t_position);
            float timeToReach = distanceToPoint / maxSpeed;

            if(timeToReach <= t_time){
                interceptPoint = t_position;
                break;
            }
        }

        paddleTransform.position = new Vector3(paddleTransform.position.x, interceptPoint.y, interceptPoint.z);
    } 

}
