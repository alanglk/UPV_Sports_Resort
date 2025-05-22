using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController_SinForce : MonoBehaviour
{
    public GameObject prefabPelota;
    public Transform puntoDisparo;
    public float intervalo = 4f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Disparar), 1f, intervalo);
    }

    void Disparar()
    {
        Instantiate(prefabPelota, puntoDisparo.position, puntoDisparo.rotation);
    }
}
