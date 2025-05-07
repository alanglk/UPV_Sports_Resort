using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canon_controller : MonoBehaviour
{
    public GameObject baseball_ball;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 15f;
    public float intervalo = 4f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Disparar), 1f, intervalo);
    }

    void Disparar()
    {
        GameObject pelota = Instantiate(baseball_ball, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = pelota.GetComponent<Rigidbody>();
        rb.AddForce(puntoDisparo.forward * fuerzaDisparo, ForceMode.Impulse);
    }
}
