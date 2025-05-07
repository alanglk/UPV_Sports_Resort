using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour{   
    public Transform initialTargetTransform;
    public float maxHeight = 6.0f;
    public bool debugPath;
    private Rigidbody ball;

    // Start is called before the first frame update
    void Start(){
        ball = GetComponent<Rigidbody>();
        ball.useGravity = false;
    }

    // Update is called once per frame
    void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			
            // LaunchBall
		    ball.useGravity = true;
            LaunchBallToTarget(initialTargetTransform.position, maxHeight);
		}

    }

	public void LaunchBallToTarget(Vector3 target, float h){
		var launchData = CalculateLaunchData(target, h);
		ball.velocity = launchData.initialVelocity;
	}

    LaunchData CalculateLaunchData(Vector3 target, float h) {
        // Target: the point to reach
        // h: max height of the trajectory
        float gravity = Physics.gravity.y;
		float displacementY = target.y - ball.position.y;
		Vector3 displacementXZ = new Vector3 (target.x - ball.position.x, 0, target.z - ball.position.z);
		float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
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
