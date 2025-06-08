using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public AudioClip bounceClip;         // Sonido al tocar suelo
    public AudioClip wallBounceClip;     // Sonido al tocar muro u otro objeto
    public AudioClip woodBounceClip;     // Sonido al tocar muro u otro objeto


    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Floor" && bounceClip != null)
        {
            audioSource.PlayOneShot(bounceClip);
        }
        else if (tag == "Wall" && wallBounceClip != null)
        {
            audioSource.PlayOneShot(wallBounceClip);
        }
        else if (tag == "Wood" && wallBounceClip != null)
        {
            audioSource.PlayOneShot(woodBounceClip);
        }
        // Puedes agregar más condiciones para otros tags
    }
}
