using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelotaMovimiento : MonoBehaviour
{
    public Transform jugador;         
    public float velocidad = 12f;       

    void Update()
    {
        if (jugador != null)
        {
            // Mover la pelota despacio hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        }
    }
}