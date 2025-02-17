using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource para la pista 2

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true; // Configura el audio para que haga loop mientras se reproduce
            audioSource.Stop();      // Asegura que el audio no comience a reproducirse al inicio
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en la zona.");
            if (audioSource != null && !audioSource.isPlaying)
            {
                Debug.Log("Reproduciendo audio.");
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió de la zona.");
            if (audioSource != null && audioSource.isPlaying)
            {
                Debug.Log("Deteniendo audio.");
                audioSource.Stop();
            }
        }
    }
}