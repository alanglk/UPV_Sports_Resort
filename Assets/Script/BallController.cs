using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public Rigidbody ball;
    public Transform targetPoint;
    
    public float maxHeight = 6.0f; // maximum height the ball will reach during the arc
    public bool debugPath;

    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        ball.useGravity = false;
    }

    public void Update() {
        ball.useGravity = true;
        LaunchBallToTarget(targetPoint.position, maxHeight);
    }

    public void LaunchBallToTarget(Vector3 target, float h){
		var launchData = CalculateLaunchData(target, h);
        ball.velocity = launchData.initialVelocity;
	}

    LaunchData CalculateLaunchData(Vector3 target, float h) {
        // Target: the point to reach
        // h: max height of the trajectory
        float gravity = Physics.gravity.y;
		// displacementY = distance in Y-axis between the target and the current position of the object 
        float displacementY = target.y - ball.position.y;
        Vector3 displacementXZ = new Vector3 (target.x - ball.position.x, 0, target.z - ball.position.z);
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
