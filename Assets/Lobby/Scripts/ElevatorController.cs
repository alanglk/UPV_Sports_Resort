using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public GameObject elevatorDoor;


    // Start is called before the first frame update
    void Start()
    {
        OpenDoor();
    }

    public void OpenDoor(){
        elevatorDoor.SetActive(false);
        elevatorDoor.transform.position = new Vector3(100,100,100);
    }

    public void CloseDoor(){
        elevatorDoor.SetActive(true);
    }
}
