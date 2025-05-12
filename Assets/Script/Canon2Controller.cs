using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon2Controller : MonoBehaviour
{
    public GameObject prefabPelota;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 4f;
    public float intervalo = 2f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Disparar), 1f, intervalo);
    }

    void Disparar()
    {
        GameObject pelota = Instantiate(prefabPelota, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = pelota.GetComponent<Rigidbody>();
        rb.AddForce(puntoDisparo.forward * fuerzaDisparo, ForceMode.Impulse);
    }
}
