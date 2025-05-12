using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarBate : MonoBehaviour
{
    public Transform manoJugador; 
    private bool enZonaAgarrar = false;
    private GameObject bateEnMano = null;

    void Update()
    {
        if (enZonaAgarrar && bateEnMano == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Agarrar();
            }
        }
    }

    void Agarrar()
    {
        bateEnMano = gameObject;
        bateEnMano.transform.SetParent(manoJugador);
        bateEnMano.transform.localPosition = Vector3.zero;
        bateEnMano.transform.localRotation = Quaternion.identity;

        Rigidbody rb = bateEnMano.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // Deja de usar física
        }

        Debug.Log("Bate agarrado");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZonaAgarrar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZonaAgarrar = false;
        }
    }
}
