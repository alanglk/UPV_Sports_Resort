using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Rigidbody))]
public class SceneController : MonoBehaviour
{
    public Rigidbody baseBall;
    public Rigidbody footBall;
    public Rigidbody soccerBall;
    public Rigidbody basketBall;

    public Transform triggerPointBaseball;
    public Transform triggerPointFootball;
    public Transform triggerPointSoccer;
    public Transform triggerPointBasketball;

    public float maxHeight = 6.0f; // maximum height the ball will reach during the arc
    public bool debugPath;

    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        baseBall = GetComponent<Rigidbody>();
        baseBall.useGravity = false;
        footBall = GetComponent<Rigidbody>();
        footBall.useGravity = false;
        soccerBall = GetComponent<Rigidbody>();
        soccerBall.useGravity = false;
        basketBall = GetComponent<Rigidbody>();
        basketBall.useGravity = false;
    
        rand = new System.Random();
    }

    void Update() {
        if (Input.GetKeyDown (KeyCode.Space)) {
            int ball = rand.Next(0, 4);

            if( ball == 0 ){
                baseBall.useGravity = true;
                LaunchBallToTarget(triggerPointBaseball.position, maxHeight, ball);
            } else if (ball == 1) {
                footBall.useGravity = true;
                LaunchBallToTarget(triggerPointFootball.position, maxHeight, ball);
            } else if (ball == 2) {
                soccerBall.useGravity = true;
                LaunchBallToTarget(triggerPointSoccer.position, maxHeight, ball);
            } else {
                basketBall.useGravity = true;
                LaunchBallToTarget(triggerPointBasketball.position, maxHeight, ball);
            }
		}
    }

    public void LaunchBallToTarget(Vector3 target, float h, int ballIndx){
		var launchData = CalculateLaunchData(target, h, ballIndx);
        if( ballIndx == 0 ){
            baseBall.velocity = launchData.initialVelocity;
        } else if (ballIndx == 1) {
            footBall.velocity = launchData.initialVelocity;
        } else if (ballIndx == 2) {
            soccerBall.velocity = launchData.initialVelocity;
        } else {
            basketBall.velocity = launchData.initialVelocity;
        }
	}

    LaunchData CalculateLaunchData(Vector3 target, float h, int ballIndx) {
        // Target: the point to reach
        // h: max height of the trajectory
        float gravity = Physics.gravity.y;
		// displacementY = distance in Y-axis between the target and the current position of the object 
        float displacementY;
        Vector3 displacementXZ;
        if (ballIndx == 0) {
    		displacementY = target.y - baseBall.position.y;
		    displacementXZ = new Vector3 (target.x - baseBall.position.x, 0, target.z - baseBall.position.z);
        } else if (ballIndx == 1) {
            displacementY = target.y - footBall.position.y;
		    displacementXZ = new Vector3 (target.x - footBall.position.x, 0, target.z - footBall.position.z);
        } else if (ballIndx == 2) {
            displacementY = target.y - soccerBall.position.y;
		    displacementXZ = new Vector3 (target.x - soccerBall.position.x, 0, target.z - soccerBall.position.z);
        } else {
            displacementY = target.y - basketBall.position.y;
		    displacementXZ = new Vector3 (target.x - basketBall.position.x, 0, target.z - basketBall.position.z);
        }
		// time the ball will be in the air
		float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
		// vertical and horizontal velocity to reach the target
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	struct LaunchData {
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget){
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
	}
}
