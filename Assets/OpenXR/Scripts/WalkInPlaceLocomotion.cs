using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkInPlaceLocomotion : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject leftHand, rightHand;
    Vector3 previousPosLeft, previousPosRight, direction;
    Vector3 gravity = new Vector3(0,-9.8f,0);
    [SerializeField] float speed = 4;
    // Start is called before the first frame update
    void Start()
    {
        setPreviousPos();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate velocity of the player hand movement
        Vector3 leftHandVelocity = leftHand.transform.position - previousPosLeft;
        Vector3 rightHandVelocity = rightHand.transform.position - previousPosRight;

        float totalVelocity = +leftHandVelocity.magnitude * 0.8f + rightHandVelocity.magnitude * 0.8f;

        // If TRUE the player has swind the hand
        if(totalVelocity >= 5f)
        {
            //Getting the direction the player is facing
            direction = Camera.main.transform.forward;
            //Move the player using the character controller
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction,Vector3.up));
        }

        // Apply gravity
        characterController.Move(gravity * Time.deltaTime);
    }

    void setPreviousPos()
    {
        previousPosLeft = leftHand.transform.position;
        previousPosRight = rightHand.transform.position;
    }
}
