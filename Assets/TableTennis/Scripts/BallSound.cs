using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    public AudioClip sonidoMesa;
    public AudioClip sonidoPala;
    public AudioClip sonidoSuelo;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bat"))
        {
            audioSource.PlayOneShot(sonidoMesa);
        }
        else if (collision.gameObject.CompareTag("Table"))
        {
            audioSource.PlayOneShot(sonidoPala);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            audioSource.PlayOneShot(sonidoSuelo);
        }
    }

}
