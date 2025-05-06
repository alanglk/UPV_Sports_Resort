using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour{   
    public Vector3 initialVelocity;
    private Rigidbody ball;

    // Start is called before the first frame update
    void Start(){
        ball = GetComponent<Rigidbody>();
        ball.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update(){
        
    }

}
